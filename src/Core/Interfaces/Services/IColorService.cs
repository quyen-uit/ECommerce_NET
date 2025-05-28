using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Specifications.Categories;
using Core.Specifications.Products;

namespace Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<IReadOnlyList<Color>> GetAllColorsAsync(ColorSpecParams specParams);
        Task<int> CountAllAsync(ColorSpecParams specParams);
        Task<Color> GetColorByIdAsync(long id);
        Task<Color> AddColorAsync(CreateColorDto colorDto);
        Task<IReadOnlyList<Color>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos);
        Task<Color> UpdateColorAsync(long id, CreateColorDto colorDto);
        Task<long> DeleteColorAsync(long id);
    }

}
