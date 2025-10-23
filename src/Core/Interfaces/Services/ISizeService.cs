using Core.Common;
using Core.Dtos.Sizes;
using Core.Specifications.Sizes;

namespace Core.Interfaces.Services
{
    public interface ISizeService
    {
        Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams);
        Task<SizeDto> GetSizeByIdAsync(Guid id);
        Task<SizeDto> AddOrUpdateSizeAsync(CreateSizeDto dto);
        Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(IReadOnlyList<CreateSizeDto> dtos);
        Task DeleteSizeAsync(Guid id);
        Task DeleteSizesAsync(List<Guid> ids);
    }
}
