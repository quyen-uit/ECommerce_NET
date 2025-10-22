using Core.Common;
using Core.Dtos.Colors;
using Core.Specifications.Colors;

namespace Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<Pagination<ColorDto>> GetAllColorsAsync(ColorSpecParams specParams);
        Task<int> CountAllAsync(ColorSpecParams specParams);
        Task<ColorDto> GetColorByIdAsync(long id);
        Task<ColorDto> AddOrUpdateColorAsync(CreateColorDto colorDto);
        Task<IReadOnlyList<ColorDto>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos);
        Task DeleteColorAsync(long id);
    }
}
