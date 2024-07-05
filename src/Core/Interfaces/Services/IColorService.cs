using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<IReadOnlyList<Color>> GetAllColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task<Color> AddColorAsync(CreateColorDto colorDto);
        Task<IReadOnlyList<Color>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos);
        Task<Color> UpdateColorAsync(ColorDto colorDto);
        Task<int> DeleteColorAsync(int id);
    }

}
