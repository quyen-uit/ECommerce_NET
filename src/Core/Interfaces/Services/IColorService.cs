using Core.Common;
using Core.Dtos;
using Core.Entities;
using Core.Specifications.Colors;

namespace Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<Pagination<ColorDto>> GetAllColorsAsync(ColorSpecParams specParams);
        Task<int> CountAllAsync(ColorSpecParams specParams);
        Task<ColorDto> GetColorByIdAsync(long id);
        Task<ColorDto> AddColorAsync(CreateColorDto colorDto);
        Task<IReadOnlyList<ColorDto>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos);
        Task<ColorDto> UpdateColorAsync(UpdateColorDto colorDto);
        Task DeleteColorAsync(long id);
    }
}
