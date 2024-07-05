using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<IReadOnlyList<Color>> GetAllColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task<int> AddColorAsync(Color color);
        Task<int> AddRangeColorAsync(IReadOnlyList<Color> colors);
        Task<int> UpdateColorAsync(Color color);
        Task<int> DeleteColorAsync(int id);
    }

}
