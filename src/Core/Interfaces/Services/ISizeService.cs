using Core.Common;
using Core.Dtos;
using Core.Entities;
using Core.Specifications.Colors;
using Core.Specifications.Sizes;

namespace Core.Interfaces.Services
{
    public interface ISizeService
    {
        Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams);
        Task<SizeDto> GetSizeByIdAsync(long id);
        Task<SizeDto> AddSizeAsync(CreateSizeDto dto);
        Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(IReadOnlyList<CreateSizeDto> dtos);
        Task<SizeDto> UpdateSizeAsync(UpdateSizeDto dto);
        Task DeleteSizeAsync(long id);
    }
}
