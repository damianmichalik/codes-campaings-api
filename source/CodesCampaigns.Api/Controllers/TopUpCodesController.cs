using CodesCampaigns.Api.DTO;
using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CodesCampaigns.Api.Controllers;

[ApiController]
[Route("api/v1/top_up_codes")]
public class TopUpCodesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UseCode(
        [FromBody] UseTopUpCodeDto dto,
        ICommandHandler<UseTopUpCodeCommand, UseTopUpCodeResult> commandHandler,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(
            new UseTopUpCodeCommand(dto.PartnerCode, dto.Code, dto.Email),
            cancellationToken);

        return Ok(new UseTopUpCodeResponseDto
        {
            Success = result.Success,
            ErrorCode = result.ErrorCode
        });
    }
}
