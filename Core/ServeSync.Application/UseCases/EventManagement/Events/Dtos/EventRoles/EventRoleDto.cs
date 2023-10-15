﻿namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

public class EventRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsNeedApprove { get; set; }
    public double Score { get; set; }
    public int Quantity { get; set; }
    public int Registered { get; set; }
}