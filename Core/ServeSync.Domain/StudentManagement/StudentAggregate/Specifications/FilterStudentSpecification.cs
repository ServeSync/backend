using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

public class FilterStudentSpecification : PagingAndSortingSpecification<Student, Guid>
{
    private readonly string? _search;
    private readonly bool? _gender;

    public FilterStudentSpecification(int page, int size, string sorting, string? search, bool? gender) 
        : base(page, size, sorting)
    {
        _search = search;
        _gender = gender;
        
        AddInclude(x => x.HomeRoom);
        AddInclude(x => x.EducationProgram);
        AddInclude("HomeRoom.Faculty");
    }
    
    public override Expression<Func<Student, bool>> ToExpression()
    {
        return x => ((string.IsNullOrWhiteSpace(_search)) || (x.FullName.ToLower().Contains(_search.ToLower()) || x.Code.Contains(_search.ToLower()))) &&
                            (!_gender.HasValue || _gender.Value == x.Gender);
    }

    public override string BuildSorting()
    {
        Sorting = Sorting.Replace(nameof(HomeRoom), "HomeRoom.Name");
        Sorting = Sorting.Replace(nameof(Faculty), "HomeRoom.Faculty.Name");
        Sorting = Sorting.Replace(nameof(EducationProgram), "EducationProgram.Name");
        return Sorting;
    }
}