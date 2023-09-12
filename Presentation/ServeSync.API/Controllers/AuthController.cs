﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthCredentialDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignInAsync(SignInCommand signInCommand)
    {
        var authCredential = await _mediator.Send(signInCommand);
        return Ok(authCredential);
    }
}