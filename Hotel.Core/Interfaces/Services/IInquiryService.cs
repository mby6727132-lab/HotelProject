using Hotel.Core.DTOs.Inquiries;

namespace Hotel.Core.Interfaces.Services;

public interface IInquiryService
{
    Task<InquiryResponseDto> CreateAsync(int userId, CreateInquiryDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<InquiryResponseDto>> GetMyInquiriesAsync(int userId, CancellationToken cancellationToken = default);
    Task<InquiryResponseDto?> GetMyInquiryByIdAsync(int userId, int inquiryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdminInquiryResponseDto>> GetAllForAdminAsync(CancellationToken cancellationToken = default);
    Task<AdminInquiryResponseDto?> RespondAsync(int inquiryId, RespondToInquiryDto dto, CancellationToken cancellationToken = default);
}
