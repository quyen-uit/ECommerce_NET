using Core.Common;
using Core.Dtos;
using Core.Dtos.Sizes;
using Core.Entities;
using Core.Specifications.Colors;
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
    }
}
