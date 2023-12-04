using OfficeOpenXml;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ExportStudentAttendanceEventsCommandHandler : ICommandHandler<ExportStudentAttendanceEventsCommand, byte[]>
{
    private readonly IProofRepository _proofRepository;
    private readonly IBasicReadOnlyRepository<EventCategory, Guid> _eventCategoryRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    
    public ExportStudentAttendanceEventsCommandHandler(
        IProofRepository proofRepository,
        IBasicReadOnlyRepository<EventCategory, Guid> eventCategoryRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        IEventReadModelRepository eventReadModelRepository)
    {
        _proofRepository = proofRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _studentRepository = studentRepository;
        _eventActivityRepository = eventActivityRepository;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<byte[]> Handle(ExportStudentAttendanceEventsCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindAsync(new StudentByIdSpecification(request.StudentId));
        if (student == null)
        {
            throw new StudentNotFoundException(request.StudentId);
        }
        
        var (events, _) = await _eventReadModelRepository.GetAttendanceEventsOfStudentAsync(request.StudentId, 1, 10);
        var eventCategories = (await _eventCategoryRepository.FindAllAsync()).OrderBy(x => x.Index).ToList();
        var activities = await _eventActivityRepository.FindAllAsync();
        
        var specification = new ProofByStudentIdSpecification(request.StudentId, true)
            .And(new ProofByStatusSpecification(ProofStatus.Approved));
        var proofs = await _proofRepository.FilterAsync(specification);
        
        return await GenerateExcelFileAsync(student, events, proofs, eventCategories, activities);
    }

    private async Task<byte[]> GenerateExcelFileAsync(
        Student student, 
        IList<EventReadModel> events, 
        IList<Proof> proofs,
        IList<EventCategory> eventCategories,
        IList<EventActivity> activities)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var excelPackage = new ExcelPackage();
        var worksheet = excelPackage.Workbook.Worksheets.Add("Kết quả phục vụ cộng đồng");

        var headerEndAt = GenerateExcelHeader(worksheet, 1);
        var studentInfoSectionEndAt = GenerateStudentInfoSection(worksheet, student, headerEndAt);
        GenerateAttendanceEventsSection(worksheet, student.Id, events, proofs, eventCategories, activities, studentInfoSectionEndAt);
        
        worksheet.Cells.AutoFitColumns();
        return await excelPackage.GetAsByteArrayAsync();
    }
    
    private int GenerateExcelHeader(ExcelWorksheet worksheet, int beginRow)
    {
        var header = "BẢNG TỔNG HỢP ĐIỂM HOẠT ĐỘNG CỘNG ĐỒNG CỦA SINH VIÊN";
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
            { "Giới tính", student.Gender ? "Nam" : "Nữ" },
            { "Ngày sinh", student.DateOfBirth },
            { "Lớp sinh hoạt", student.HomeRoom!.Name },
            { "Hệ đào tạo", student.EducationProgram!.Name },
            { "Khoa", student.HomeRoom!.Faculty!.Name}
        };

        row = row + 2;
        
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
        IList<Proof> proofs,
        IList<EventCategory> eventCategories, 
        IList<EventActivity> activities,
        int beginRow)
    {
        var columnNames = new string[]
        {
            "STT", "Hoạt động", "Đơn vị tổ chức", "Thời gian bắt đầu", "Thời gian kết thúc", "Vai trò", "Điểm", "Thời gian tham gia", "Hình ảnh"
        };

        var isLoopedEventName = false;
        var row = beginRow + 1;
        for (var i = 0; i < columnNames.Length; i++)
        {
            if (columnNames[i] == "Hoạt động")
            {
                worksheet.Cells[row, i + 1].Value = columnNames[i];
                worksheet.Cells[row + 1, i + 1].Value = "Danh mục";
                worksheet.Cells[row + 1, i + 2].Value = "Tên hoạt động";
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
        var sum = 0.0;
        foreach (var category in eventCategories)
        {
            worksheet.Cells[row, 1].Value = stt++;
            worksheet.Cells[row, 2].Value = category.Name;
            worksheet.Cells[row, 2].Style.Font.Bold = true;
            
            var activitiesOfCategory = activities.Where(x => x.EventCategoryId == category.Id).ToList();
            row = row + 1;
            
            foreach (var activity in activitiesOfCategory)
            {
                var startRow = row;
                
                var eventsOfActivity = events.Where(x => x.Activity.Id == activity.Id).ToList();
                var proofsOfActivity = proofs
                    .Where(x =>
                        (x.ProofType == ProofType.External && x.ExternalProof!.ActivityId == activity.Id) ||
                        (x.ProofType == ProofType.Internal && x.InternalProof!.Event!.ActivityId == activity.Id) ||
                        (x.ProofType == ProofType.Special && x.SpecialProof!.ActivityId == activity.Id)
                    )
                    .ToList();
                
                if (eventsOfActivity.Any() || proofsOfActivity.Any())
                {
                    worksheet.Cells[row, 2].Value = activity.Name;
                }
                else
                {
                    continue;
                }
                
                foreach (var @event in eventsOfActivity)
                {
                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 3].Value = @event.Name;
                    worksheet.Cells[row, 4].Value = @event.RepresentativeOrganization.Name;
                    worksheet.Cells[row, 5].Value = @event.StartAt;
                    worksheet.Cells[row, 6].Value = @event.EndAt;
                    worksheet.Cells[row, 7].Value = @event.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == studentId)).Name;
                    worksheet.Cells[row, 8].Value = @event.Roles.First(x => x.RegisteredStudents.Any(y => y.StudentId == studentId)).Score;
                    worksheet.Cells[row, 9].Value = @event.AttendanceInfos.SelectMany(x => x.AttendanceStudents).First(x => x.StudentId == studentId).AttendanceAt;
                    worksheet.Cells[row, 10].Value = "Đã xác nhận tham gia";
                    
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                    worksheet.Cells[row, 6].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                    worksheet.Cells[row, 9].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";

                    sum += (double)worksheet.Cells[row, 8].Value;
                    row++;
                }
                
                foreach (var proof in proofsOfActivity)
                {
                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 3].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.EventName,
                        ProofType.Internal => proof.InternalProof!.Event!.Name,
                        ProofType.Special => proof.SpecialProof!.Title,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 4].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.OrganizationName,
                        ProofType.Internal => proof.InternalProof!.Event!.RepresentativeOrganization!.Organization!.Name,
                        ProofType.Special => string.Empty,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 5].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.StartAt,
                        ProofType.Internal => proof.InternalProof!.Event!.StartAt,
                        ProofType.Special => proof.SpecialProof!.StartAt,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 6].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.EndAt,
                        ProofType.Internal => proof.InternalProof!.Event!.EndAt,
                        ProofType.Special => proof.SpecialProof!.EndAt,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 7].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.Role,
                        ProofType.Internal => proof.InternalProof!.EventRole!.Name,
                        ProofType.Special => proof.SpecialProof!.Role,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 8].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.Score,
                        ProofType.Internal => proof.InternalProof!.EventRole!.Score,
                        ProofType.Special => proof.SpecialProof!.Score,
                        _ => string.Empty
                    };
                    worksheet.Cells[row, 9].Value = proof.ProofType switch 
                    {
                        ProofType.External => proof.ExternalProof!.AttendanceAt,
                        ProofType.Internal => proof.InternalProof!.AttendanceAt,
                        ProofType.Special => proof.SpecialProof!.AttendanceAt,
                        _ => string.Empty
                    };

                    try
                    {
                        worksheet.Cells[row, 10].Hyperlink = new Uri(proof.ImageUrl);
                        worksheet.Cells[row, 10].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                        worksheet.Cells[row, 10].Style.Font.UnderLine = true;
                        worksheet.Cells[row, 10].Value = "Xem hình ảnh";
                    }
                    catch (Exception e)
                    {
                        worksheet.Cells[row, 10].Value = "Link hỏng!";
                    }
                    
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                    worksheet.Cells[row, 6].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                    worksheet.Cells[row, 9].Style.Numberformat.Format = "HH:mm dd/MM/yyyy";
                    
                    sum += (double)worksheet.Cells[row, 8].Value;
                    row++;
                }
                
                var endRow = row - 1;
                worksheet.Cells[startRow, 2, endRow, 2].Merge = true;
                worksheet.Cells[startRow, 2, endRow, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
        }
        
        worksheet.Cells[row, 7].Value = "Tổng điểm";
        worksheet.Cells[row, 8].Style.Font.Bold = true;
        worksheet.Cells[row, 8].Value = sum;
        
        var tableRange = worksheet.Cells[beginRow + 1, 1, row - 1, columnNames.Length + 1];
        
        tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
    }
}