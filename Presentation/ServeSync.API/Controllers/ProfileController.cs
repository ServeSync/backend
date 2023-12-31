﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.Common;
using ServeSync.Application.UseCases.StudentManagement.Students.Commands;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

namespace ServeSync.API.Controllers;

[Route("api/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    // [HasPermission(AppPermissions.Users.ViewProfile)]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileAsync()
    {
        var userInfo = await _mediator.Send(new GetUserInfoQuery());
        return Ok(userInfo);
    }
    
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        await _mediator.Send(new ChangePasswordCommand(dto.CurrentPassword, dto.NewPassword));
        return NoContent();
    }
    
    [HttpGet("student")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(typeof(StudentDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentProfileAsync()
    {
        var student = await _mediator.Send(new GetStudentByIdentityQuery());
        return Ok(student);
    }
    
    [HttpPut("student")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStudentProfileAsync(StudentEditProfileDto dto)
    {
        var command = new UpdateStudentByIdentityCommand(dto.ImageUrl, dto.Email, dto.Phone, dto.HomeTown, dto.Address);
        
        await _mediator.Send(command);
        return NoContent();
    }
}