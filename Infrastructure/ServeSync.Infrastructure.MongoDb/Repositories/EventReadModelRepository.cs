using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServeSync.Application.ReadModels.Events;

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

    public async Task<(List<RegisteredStudentInEventReadModel>?, int?)> GetPagedRegisteredStudentsInEventAsync(Guid eventId, int page, int size)
    {
        var registeredStudents = (await Collection.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventId))
            ?.Roles
            .SelectMany(x => x.RegisteredStudents, (role, registeredStudent) => new RegisteredStudentInEventReadModel()
            {
                Id = registeredStudent.Id,
                Code = registeredStudent.Code,
                StudentId = registeredStudent.StudentId,
                Name = registeredStudent.Name,
                Email = registeredStudent.Email,
                Phone = registeredStudent.Phone,
                Status = registeredStudent.Status,
                ImageUrl = registeredStudent.ImageUrl,
                RegisteredAt = registeredStudent.RegisteredAt,
                HomeRoomName = registeredStudent.HomeRoomName,
                Role = role.Name,
            })
            .OrderBy(x => x.RegisteredAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        var total = (await Collection.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventId))
            ?.Roles
            .SelectMany(x => x.RegisteredStudents)
            .Count();

        return (registeredStudents, total);
    }

    public async Task<(List<AttendanceStudentInEventRoleReadModel>?, int?)> GetPagedAttendanceStudentsInEventAsync(Guid eventId, int page, int size)
    {
        var attendanceStudents = (await Collection.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventId))
            ?.AttendanceInfos
            .SelectMany(x => x.AttendanceStudents, (attendanceInfo, attendanceStudent) => new AttendanceStudentInEventRoleReadModel()
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
            })
            .OrderBy(x => x.AttendanceAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();
        
        var total = (await Collection.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventId))
            ?.AttendanceInfos
            .SelectMany(x => x.AttendanceStudents)
            .Count();

        return (attendanceStudents, total);
    }

    public Task<(List<EventReadModel>, int)> GetAttendanceEventsOfStudentAsync(Guid studentId, int page, int size)
    {
        var attendanceEvents = Collection.AsQueryable()
            .Where(x =>
                x.AttendanceInfos.Any(y => 
                    y.AttendanceStudents.Any(z => z.StudentId == studentId)))
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();
        
        var total = Collection
            .AsQueryable()
            .Count(x => x.AttendanceInfos.Any(y => 
                y.AttendanceStudents.Any(z => z.StudentId == studentId)));

        return Task.FromResult((attendanceEvents, total));
    }

    public Task<int> GetCountNumberOfAttendedEventsOfStudentAsync(Guid studentId)
    {
        return Task.FromResult(
            Collection
            .AsQueryable()
            .Count(x => x.AttendanceInfos.Any(y => 
                y.AttendanceStudents.Any(z => z.StudentId == studentId))));
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
}