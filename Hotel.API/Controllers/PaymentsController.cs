using Hotel.API.Extensions;
using Hotel.Core.DTOs.Payments;
using Hotel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponseDto>> Create([FromBody] CreatePaymentDto dto, CancellationToken cancellationToken)
    {
        var created = await _paymentService.CreateAsync(User.GetUserId(), dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetMy(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetMyPaymentsAsync(User.GetUserId(), cancellationToken);
        return Ok(payments);
    }
}
