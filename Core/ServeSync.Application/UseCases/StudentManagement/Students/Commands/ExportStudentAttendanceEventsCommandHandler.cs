using OfficeOpenXml;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ExportStudentAttendanceEventsCommandHandler : ICommandHandler<ExportStudentAttendanceEventsCommand, byte[]>
{
    private readonly IBasicReadOnlyRepository<EventCategory, Guid> _eventCategoryRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    
    public ExportStudentAttendanceEventsCommandHandler(
        IBasicReadOnlyRepository<EventCategory, Guid> eventCategoryRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IEventReadModelRepository eventReadModelRepository)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _studentRepository = studentRepository;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<byte[]> Handle(ExportStudentAttendanceEventsCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindAsync(new StudentByIdSpecification(request.StudentId));
        if (student == null)
        {
            throw new StudentNotFoundException(request.StudentId);
        }
        
        var events = await _eventReadModelRepository.GetAttendanceEventsOfStudentAsync(request.StudentId, 1, 10);
        var eventCategories = (await _eventCategoryRepository.FindAllAsync()).OrderBy(x => x.Index).ToList();
        
        return await GenerateExcelFileAsync(student, events.Item1, eventCategories);
    }

    private async Task<byte[]> GenerateExcelFileAsync(Student student, IList<EventReadModel> events, IList<EventCategory> eventCategories)
    {
        using var excelPackage = new ExcelPackage();
        var worksheet = excelPackage.Workbook.Worksheets.Add("Kết quả phục vụ cộng đồng");

        var headerEndAt = GenerateExcelHeader(worksheet, 1);
        var studentInfoSectionEndAt = GenerateStudentInfoSection(worksheet, student, headerEndAt);
        GenerateAttendanceEventsSection(worksheet, student.Id, events, eventCategories, studentInfoSectionEndAt);
        
        worksheet.Cells.AutoFitColumns();
        return await excelPackage.GetAsByteArrayAsync();
    }
    
    private int GenerateExcelHeader(ExcelWorksheet worksheet, int beginRow)
    {
        var header = "ĐIỂM HOẠT ĐỘNG KẾT NỐI CỘNG ĐỒNG CỦA SINH VIÊN";
        worksheet.Cells[beginRow, 1].Value = header;
        worksheet.Cells[beginRow, 1, 1, header.Length - 1].Merge = true;
        worksheet.Cells[beginRow, 1].Style.Font.Bold = true;
        worksheet.Cells[beginRow, 1].Style.Font.Size = 18;

        return beginRow + 1;
    }

    private int GenerateStudentInfoSection(ExcelWorksheet worksheet, Student student, int beginRow)
    {
        var row = beginRow + 1;
        
        worksheet.Cells[row, 1].Value = "Thông tin sinh viên";
        worksheet.Cells[row, 1, row, 2].Merge = true;
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 16;

        var valueByColumnName = new Dictionary<string, object>()
        {
            { "Mã SV", student.Code },
            { "Họ và tên", student.FullName }, 
            { "Email", student.Email },
            { "Số điện thoại", student.Phone },
            { "Số CMND/CCCD", student.CitizenId },
            { "Giới tính", student.Gender ? "Nam" : "Nữ" },
            { "Ngày sinh", student.DateOfBirth },
            { "Lớp sinh hoạt", student.HomeRoom!.Name },
            { "Hệ đào tạo", student.EducationProgram!.Name },
            { "Khoa", student.HomeRoom!.Faculty!.Name}
        };

        row = row + 1;
        
        foreach (var key in valueByColumnName.Keys)
        {
            worksheet.Cells[row, 1].Value = key;
            worksheet.Cells[row, 2].Value = valueByColumnName[key];
            
            if (valueByColumnName[key] is DateTime)
                worksheet.Cells[row, 2].Style.Numberformat.Format = "dd/MM/yyyy";
            
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            row++;
        }
        
        var tableRange = worksheet.Cells[beginRow + 1, 1, row - 1, 2];
        
        tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return row;
    }

    private void GenerateAttendanceEventsSection(
        ExcelWorksheet worksheet, 
        Guid studentId,
        IList<EventReadModel> events, 
        IList<EventCategory> eventCategories, 
        int beginRow)
    {
        var columnNames = new string[]
        {
            "STT", "Sự kiện", "Đơn vị tổ chức", "Thời gian bắt đầu", "Thời gian kết thúc", "Vai trò", "Điểm", "Thời gian điểm danh"
        };

        var isLoopedEventName = false;
        var row = beginRow + 1;
        for (var i = 0; i < columnNames.Length; i++)
        {
            if (columnNames[i] == "Sự kiện")
            {
                worksheet.Cells[row, i + 1].Value = columnNames[i];
                worksheet.Cells[row + 1, i + 1].Value = "Danh mục";
                worksheet.Cells[row + 1, i + 2].Value = "Tên sự kiện";
                worksheet.Cells[row, i + 1, row, i + 2].Merge = true;

                worksheet.Cells[row, i + 1, row + 1, i + 2].Style.Font.Bold = true;
                worksheet.Cells[row, i + 1, row + 1, i + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                isLoopedEventName = true;
                continue;
            }

            if (isLoopedEventName)
            {
                worksheet.Cells[row, i + 2].Value = columnNames[i];
                worksheet.Cells[row, i + 2].Style.Font.Bold = true;
                worksheet.Cells[row, i + 2, row + 1, i + 2].Merge = true;
                
                worksheet.Cells[row, i + 2, row + 1, i + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, i + 2, row + 1, i + 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
            else
            {
                worksheet.Cells[row, i + 1].Value = columnNames[i]; 
                worksheet.Cells[row, i + 1].Style.Font.Bold = true;
                worksheet.Cells[row, i + 1, row + 1, i + 1].Merge = true;
                worksheet.Cells[row, i + 1, row + 1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, i + 1, row + 1, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
        }

        row = row + 2;
        var stt = 1;
        foreach (var category in eventCategories)
        {
            var eventsOfCategory = events.Where(x => x.Activity.EventCategoryId == category.Id).ToList();

            worksheet.Cells[row, 1].Value = stt++;
            worksheet.Cells[row, 2].Value = category.Name;
            worksheet.Cells[row, 2].Style.Font.Bold = true;

            row = row + 1;
            
            foreach (var @event in eventsOfCategory)
            {
                worksheet.Cells[row, 1].Value = stt++;
                worksheet.Cells[row, 2].Value = @event.Activity.Name;
                worksheet.Cells[row, 3].Value = @event.Name;
                worksheet.Cells[row, 4].Value = @event.RepresentativeOrganization.Name;
                worksheet.Cells[row, 5].Value = @event.StartAt;
                worksheet.Cells[row, 6].Value = @event.EndAt;
                worksheet.Cells[row, 7].Value = @event.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == studentId)).Name;
                worksheet.Cells[row, 8].Value = @event.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == studentId)).Score;
                worksheet.Cells[row, 9].Value = @event.AttendanceInfos.SelectMany(x => x.AttendanceStudents).First(x => x.StudentId == studentId).AttendanceAt;
                
                worksheet.Cells[row, 5].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                worksheet.Cells[row, 6].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                worksheet.Cells[row, 9].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                
                row++;
            }
        }
        
        var tableRange = worksheet.Cells[beginRow + 1, 1, row - 1, columnNames.Length + 1];
        
        tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
    }
}