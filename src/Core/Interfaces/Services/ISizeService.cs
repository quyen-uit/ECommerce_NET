using Core.Common;
using Core.Dtos.Sizes;
using Core.Specifications.Sizes;

namespace Core.Interfaces.Services
{
    public interface ISizeService
    {
        Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams);
        Task<SizeDto> GetSizeByIdAsync(long id);
        Task<SizeDto> AddOrUpdateSizeAsync(CreateSizeDto dto);
        Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(IReadOnlyList<CreateSizeDto> dtos);
        Task DeleteSizeAsync(long id);
        Task DeleteSizesAsync(List<long> ids);
    }
}
