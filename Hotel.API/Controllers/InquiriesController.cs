using Hotel.API.Extensions;
using Hotel.Core.DTOs.Inquiries;
using Hotel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/inquiries")]
[Authorize]
public class InquiriesController : ControllerBase
{
    private readonly IInquiryService _inquiryService;

    public InquiriesController(IInquiryService inquiryService)
    {
        _inquiryService = inquiryService;
    }

    [HttpPost]
    public async Task<ActionResult<InquiryResponseDto>> Create([FromBody] CreateInquiryDto dto, CancellationToken cancellationToken)
    {
        var created = await _inquiryService.CreateAsync(User.GetUserId(), dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<InquiryResponseDto>>> GetMy(CancellationToken cancellationToken)
    {
        var inquiries = await _inquiryService.GetMyInquiriesAsync(User.GetUserId(), cancellationToken);
        return Ok(inquiries);
    }

    [HttpGet("my/{id:int}")]
    public async Task<ActionResult<InquiryResponseDto>> GetMyById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var inquiry = await _inquiryService.GetMyInquiryByIdAsync(User.GetUserId(), id, cancellationToken);
        return inquiry is null ? NotFound() : Ok(inquiry);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<AdminInquiryResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var inquiries = await _inquiryService.GetAllForAdminAsync(cancellationToken);
        return Ok(inquiries);
    }

    [HttpPost("{id:int}/respond")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminInquiryResponseDto>> Respond(
        [FromRoute] int id, [FromBody] RespondToInquiryDto dto, CancellationToken cancellationToken)
    {
        var updated = await _inquiryService.RespondAsync(id, dto, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }
}
