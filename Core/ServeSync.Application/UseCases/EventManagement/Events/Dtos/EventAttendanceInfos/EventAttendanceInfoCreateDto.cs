﻿namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;

public class EventAttendanceInfoCreateDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}