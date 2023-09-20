﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Infrastructure.Identity.Commons.Constants;
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
    [HasPermission(Permissions.Users.ViewProfile)]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile()
    {
        var userInfo = await _mediator.Send(new GetUserInfoQuery());
        return Ok(userInfo);
    }
}