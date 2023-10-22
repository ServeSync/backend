using MediatR;
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
    public async Task<IActionResult> SignInAsync(SignInDto dto)
    {
        var authCredential = await _mediator.Send(new SignInCommand(dto.UserNameOrEmail, dto.Password));
        return Ok(authCredential);
    }
    
    [HttpPost("forget-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestForgetPasswordAsync(RequestForgetPasswordDto dto)
    {
        await _mediator.Send(new RequestForgetPasswordTokenCommand(dto.UserNameOrEmail, dto.CallBackUrl));
        return Ok();
    }
    
    [HttpPost("forget-password/callback")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
    {
        await _mediator.Send(new ResetPasswordByTokenCommand(dto.Token, dto.NewPassword));
        return Ok();
    }
    
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthCredentialDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignInAsync(RefreshTokenDto dto)
    {
        var authCredential = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken, dto.AccessToken));
        return Ok(authCredential);
    }
}