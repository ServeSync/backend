using MongoDB.Bson;
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

    public async Task<(List<RegisteredStudentInEventReadModel>?, int?)> GetPagedRegisteredStudentsInEventRoleAsync(Guid eventId, int page, int size)
    {
        var registeredStudents = (await Collection.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventId))
            ?.Roles
            .SelectMany(x => x.RegisteredStudents, (role, registeredStudent) => new RegisteredStudentInEventReadModel()
            {
                Id = registeredStudent.Id,
                StudentId = registeredStudent.StudentId,
                Name = registeredStudent.Name,
                Status = registeredStudent.Status,
                ImageUrl = registeredStudent.ImageUrl,
                RegisteredAt = registeredStudent.RegisteredAt,
                Role = role.Name
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
}