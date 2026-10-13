using Hotel.Core.DTOs.Payments;

namespace Hotel.Core.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponseDto> CreateAsync(int userId, CreatePaymentDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentResponseDto>> GetMyPaymentsAsync(int userId, CancellationToken cancellationToken = default);
}
