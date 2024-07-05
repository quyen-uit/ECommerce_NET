using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;

namespace Infrastructure.Services
{
    public class ColorService : IColorService
    {
        private readonly IGenericRepository<Color> _colorRepository;

        public ColorService(IGenericRepository<Color> colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<int> AddColorAsync(Color color)
        {
            _colorRepository.Add(color);
            return await _colorRepository.Complete();
        }

        public async Task<int> DeleteColorAsync(int id)
        {
            _colorRepository.Delete(id);
            return await _colorRepository.Complete();
        }

        public async Task<IReadOnlyList<Color>> GetAllColorsAsync()
        {
            return await _colorRepository.GetAllAsync();
        }

        public async Task<int> UpdateColorAsync(Color color)
        {
            _colorRepository.Update(color);
            return await _colorRepository.Complete();
        }

        public async Task<int> AddRangeColorAsync(IReadOnlyList<Color> colors)
        {
            _colorRepository.AddRange(colors);
            return await _colorRepository.Complete();
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _colorRepository.GetByIdAsync(id);
        }
    }
}
