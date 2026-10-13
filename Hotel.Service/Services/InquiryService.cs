using AutoMapper;
using Hotel.Core.DTOs.Inquiries;
using Hotel.Core.Entities;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Interfaces.Services;

namespace Hotel.Service.Services;

public class InquiryService : IInquiryService
{
    private readonly IInquiryRepository _inquiryRepository;
    private readonly IMapper _mapper;

    public InquiryService(IInquiryRepository inquiryRepository, IMapper mapper)
    {
        _inquiryRepository = inquiryRepository;
        _mapper = mapper;
    }

    public async Task<InquiryResponseDto> CreateAsync(int userId, CreateInquiryDto dto, CancellationToken cancellationToken = default)
    {
        var inquiry = _mapper.Map<Inquiry>(dto);
        inquiry.UserId = userId;
        inquiry.CreatedAt = DateTime.UtcNow;

        await _inquiryRepository.AddAsync(inquiry, cancellationToken);
        await _inquiryRepository.SaveChangesAsync(cancellationToken);

        // Re-fetch so the response can include the Hotel name via navigation.
        var created = await _inquiryRepository.GetByIdAsync(inquiry.Id, cancellationToken);
        return _mapper.Map<InquiryResponseDto>(created);
    }

    public async Task<IEnumerable<InquiryResponseDto>> GetMyInquiriesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var inquiries = await _inquiryRepository.GetByUserAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<InquiryResponseDto>>(inquiries);
    }

    public async Task<InquiryResponseDto?> GetMyInquiryByIdAsync(int userId, int inquiryId, CancellationToken cancellationToken = default)
    {
        var inquiry = await _inquiryRepository.GetByIdAsync(inquiryId, cancellationToken);
        // Indistinguishable 404 whether the inquiry doesn't exist or belongs to someone else.
        if (inquiry is null || inquiry.UserId != userId)
            return null;

        return _mapper.Map<InquiryResponseDto>(inquiry);
    }

    public async Task<IEnumerable<AdminInquiryResponseDto>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
    {
        var inquiries = await _inquiryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AdminInquiryResponseDto>>(inquiries);
    }

    public async Task<AdminInquiryResponseDto?> RespondAsync(int inquiryId, RespondToInquiryDto dto, CancellationToken cancellationToken = default)
    {
        var inquiry = await _inquiryRepository.GetByIdAsync(inquiryId, cancellationToken);
        if (inquiry is null)
            return null;

        inquiry.ResponseText = dto.ResponseText;
        inquiry.RespondedAt = DateTime.UtcNow;

        _inquiryRepository.Update(inquiry);
        await _inquiryRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AdminInquiryResponseDto>(inquiry);
    }
}
