using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;

public class EventActivityScoreOutOfRangeException : ResourceInvalidDataException
{
    public EventActivityScoreOutOfRangeException(Guid activityId, double score) 
        : base($"Score is out of range for activity with id {activityId}. Score: {score}", ErrorCodes.EventActivityScoreOutOfRange)
    {
    }
}