using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Infrastructure.MongoDb.Repositories;

public class EventReadModelRepository : MongoDbRepository<EventReadModel, Guid>, IEventReadModelRepository
{
    public EventReadModelRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
    {
    }


    public async Task<EventRoleReadModel?> GetEventRoleByIdAsync(Guid eventRoleId)
    {
        var filter = Builders<EventReadModel>.Filter.ElemMatch(e => e.Roles, r => r.Id == eventRoleId);
        var projection = Builders<EventReadModel>.Projection.Include(x => x.Roles);
        var @event = await Collection.Find(filter).Project<EventReadModel>(projection).FirstOrDefaultAsync();

        return @event?.Roles.FirstOrDefault(x => x.Id == eventRoleId);
    }

    public async Task<List<EventRoleReadModel>?> GetEventRolesAsync(Guid eventId)
    {
        var filter = Builders<EventReadModel>.Filter.Eq(e => e.Id, eventId);
        var projection = Builders<EventReadModel>.Projection.Include(x => x.Roles);
        var @event = await Collection.Find(filter).Project<EventReadModel>(projection).FirstOrDefaultAsync();

        return @event?.Roles;
    }

    public async Task<(List<RegisteredStudentInEventReadModel>?, int?)> GetPagedRegisteredStudentsInEventAsync(Guid eventId, EventRegisterStatus? status, int page, int size)
    {
        var queryable = Collection.AsQueryable()
            .Where(x => x.Id == eventId)
            .SelectMany(x => x.Roles, (eventReadModel, role) => new
            {
                Role = role,
                RegisteredStudents = role.RegisteredStudents
            })
            .SelectMany(x => x.RegisteredStudents, (row, registeredStudent) => new RegisteredStudentInEventReadModel()
            {
                Id = registeredStudent.Id,
                Code = registeredStudent.Code,
                StudentId = registeredStudent.StudentId,
                Name = registeredStudent.Name,
                Email = registeredStudent.Email,
                Phone = registeredStudent.Phone,
                Description = registeredStudent.Description,
                RejectReason = registeredStudent.RejectReason,
                Status = registeredStudent.Status,
                ImageUrl = registeredStudent.ImageUrl,
                RegisteredAt = registeredStudent.RegisteredAt,
                HomeRoomName = registeredStudent.HomeRoomName,
                Role = row.Role.Name,
                IdentityId = registeredStudent.IdentityId
            })
            .Where(x => !status.HasValue || x.Status == status.Value);
            
        var registeredStudents = await queryable
            .OrderBy(x => x.RegisteredAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var total = await queryable.CountAsync();

        return (registeredStudents, total);
    }

    public async Task<(List<AttendanceStudentInEventRoleReadModel>?, int?)> GetPagedAttendanceStudentsInEventAsync(Guid eventId, int page, int size)
    {
        var queryable = Collection.AsQueryable()
            .Where(x => x.Id == eventId)
            .SelectMany(x => x.AttendanceInfos)
            .SelectMany(x => x.AttendanceStudents, (row, attendanceStudent)  => new AttendanceStudentInEventRoleReadModel()
            {
                Id = attendanceStudent.Id,
                Code = attendanceStudent.Code,
                StudentId = attendanceStudent.StudentId,
                Name = attendanceStudent.Name,
                Email = attendanceStudent.Email,
                Phone = attendanceStudent.Phone,
                ImageUrl = attendanceStudent.ImageUrl,
                AttendanceAt = attendanceStudent.AttendanceAt,
                HomeRoomName = attendanceStudent.HomeRoomName,
                Role = attendanceStudent.Role,
                Score = attendanceStudent.Score
            });
        var attendanceStudents = await queryable
            .OrderBy(x => x.AttendanceAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        var total = await queryable.CountAsync();

        return (attendanceStudents, total);
    }

    public async Task<(List<EventReadModel>, int)> GetAttendanceEventsOfStudentAsync(Guid studentId, int page, int size)
    {
        var queryable = Collection.AsQueryable()
            .Where(x =>
                x.AttendanceInfos.Any(y =>
                    y.AttendanceStudents.Any(z => z.StudentId == studentId)));
        
        var attendanceEvents = await queryable
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        var total = await queryable.CountAsync();

        return (attendanceEvents, total);
    }

    public async Task<(List<EventReadModel>, int)> GetRegisteredEventsOfStudentAsync(Guid studentId, int page, int size)
    {
        var queryable = Collection.AsQueryable()
            .Where(x =>
                x.Roles.Any(y =>
                    y.RegisteredStudents.Any(z => z.StudentId == studentId))
                && x.AttendanceInfos.All(y =>
                    y.AttendanceStudents.All(z => z.StudentId != studentId)));

        var registeredEvents = await queryable
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        var total = await queryable.CountAsync();

        return (registeredEvents, total);
    }

    public Task<int> GetCountNumberOfAttendedEventsOfStudentAsync(Guid studentId)
    {
        return Collection.AsQueryable()
                         .CountAsync(x => x.AttendanceInfos.Any(y => 
                                            y.AttendanceStudents.Any(z => z.StudentId == studentId)));
    }

    public Task<double> GetSumScoreOfAttendedEventsOfStudentAsync(Guid studentId)
    {
        return Collection.AsQueryable()
            .Where(x =>
                x.AttendanceInfos.Any(y =>
                    y.AttendanceStudents.Any(z => z.StudentId == studentId)))
            .SumAsync(x => x.Roles
                .First(y => y.RegisteredStudents.Any(z => z.StudentId == studentId))
                .Score)
            ;
    }

    public async Task<EventReadModel?> GetEventByStudentRegistrationAsync(Guid eventRegisterId)
    {
        return await Collection.AsQueryable()
            .FirstOrDefaultAsync(x =>
                x.Roles.Any(y =>
                    y.RegisteredStudents.Any(z => z.Id == eventRegisterId)));

    }

    public Task UpdateStudentInEventsAsync(Student student)
    {
        var filter = Builders<EventReadModel>.Filter.Eq("Roles.RegisteredStudents.StudentId", student.Id.ToString())
                     | Builders<EventReadModel>.Filter.Eq("AttendanceInfos.AttendanceStudents.StudentId", student.Id.ToString());

        var update = Builders<EventReadModel>.Update
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].Name", student.FullName)
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].Code", student.Code)
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].Email", student.Email)
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].Phone", student.Phone)
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].ImageUrl", student.ImageUrl)
            .Set("Roles.$[role].RegisteredStudents.$[registeredStudent].HomeRoomName", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].Name", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].Code", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].Email", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].Phone", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].ImageUrl", student.HomeRoom!.Name)
            .Set("AttendanceInfos.$[attendanceInfo].AttendanceStudents.$[attendanceStudent].HomeRoomName", student.HomeRoom!.Name);
        
        var arrayFilters = new List<ArrayFilterDefinition>
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("role.RegisteredStudents.StudentId", student.Id.ToString())),
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("registeredStudent.StudentId", student.Id.ToString())),
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("attendanceInfo.AttendanceStudents.StudentId", student.Id.ToString())),
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("attendanceStudent.StudentId", student.Id.ToString()))
        };
        
        return Collection.UpdateManyAsync(filter, update, new UpdateOptions()
        {
            ArrayFilters = arrayFilters
        });
    }

    public Task UpdateOrganizationInEventsAsync(EventOrganization eventOrganization)
    {
        var filter = Builders<EventReadModel>.Filter.Eq("RepresentativeOrganization.OrganizationId", eventOrganization.Id.ToString())
                     | Builders<EventReadModel>.Filter.Eq("Organizations.OrganizationId", eventOrganization.Id.ToString());
        
        var update = Builders<EventReadModel>.Update
            .Set("RepresentativeOrganization.Name", eventOrganization.Name)
            .Set("RepresentativeOrganization.ImageUrl", eventOrganization.ImageUrl)
            .Set("RepresentativeOrganization.Email", eventOrganization.Email)
            .Set("RepresentativeOrganization.PhoneNumber", eventOrganization.PhoneNumber)
            .Set("RepresentativeOrganization.Address", eventOrganization.Address)
            .Set("Organizations.$[organization].Name", eventOrganization.Name)
            .Set("Organizations.$[organization].ImageUrl", eventOrganization.ImageUrl)
            .Set("Organizations.$[organization].Email", eventOrganization.Email)
            .Set("Organizations.$[organization].PhoneNumber", eventOrganization.PhoneNumber)
            .Set("Organizations.$[organization].Address", eventOrganization.Address);
        
        var arrayFilters = new List<ArrayFilterDefinition>
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("organization.OrganizationId", eventOrganization.Id.ToString())),
        };
        
        return Collection.UpdateManyAsync(filter, update, new UpdateOptions()
        {
            ArrayFilters = arrayFilters
        });
    }

    public Task UpdateOrganizationContactInEventsAsync(EventOrganizationContact eventOrganizationContact)
    {
        var filter = Builders<EventReadModel>.Filter.Eq("Organizations.Representatives.OrganizationRepId",eventOrganizationContact.Id.ToString());
        
        var update = Builders<EventReadModel>.Update
            .Set("Organizations.$[organization].Representatives.$[representative].Name", eventOrganizationContact.Name)
            .Set("Organizations.$[organization].Representatives.$[representative].ImageUrl", eventOrganizationContact.ImageUrl)
            .Set("Organizations.$[organization].Representatives.$[representative].Email", eventOrganizationContact.Email)
            .Set("Organizations.$[organization].Representatives.$[representative].PhoneNumber", eventOrganizationContact.PhoneNumber)
            .Set("Organizations.$[organization].Representatives.$[representative].Position", eventOrganizationContact.Position);
        
        var arrayFilters = new List<ArrayFilterDefinition>
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("organization.Representatives.OrganizationRepId", eventOrganizationContact.Id.ToString())),
            new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("representative.OrganizationRepId", eventOrganizationContact.Id.ToString())),
        };

        return Collection.UpdateManyAsync(filter, update, new UpdateOptions()
        {
            ArrayFilters = arrayFilters
        });
    }
}