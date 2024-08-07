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
        Task<Color> GetColorByIdAsync(int id);
        Task<Color> AddColorAsync(CreateColorDto colorDto);
        Task<IReadOnlyList<Color>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos);
        Task<Color> UpdateColorAsync(int id, CreateColorDto colorDto);
        Task<int> DeleteColorAsync(int id);
    }

}
