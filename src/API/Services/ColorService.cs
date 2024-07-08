using AutoMapper;
using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Products;

namespace API.Services
{
    public class ColorService : IColorService
    {
        private readonly IGenericRepository<Color> _colorRepository;
        private readonly IMapper _mapper;

        public ColorService(IGenericRepository<Color> colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<Color> AddColorAsync(CreateColorDto colorDto)
        {
            var color = _mapper.Map<Color>(colorDto);

            _colorRepository.Add(color);
            await _colorRepository.Complete();

            return color;
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

        public async Task<Color> UpdateColorAsync(int id, CreateColorDto colorDto)
        {
            var color = _mapper.Map<Color>(colorDto);
            color.Id = id;

            _colorRepository.Update(color);
            await _colorRepository.Complete();
            return color;
        }

        public async Task<IReadOnlyList<Color>> AddRangeColorAsync(IReadOnlyList<CreateColorDto> colorDtos)
        {
            var colors = _mapper.Map<IReadOnlyList<Color>>(colorDtos);

            _colorRepository.AddRange(colors);
            await _colorRepository.Complete();
            return colors;
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _colorRepository.GetByIdAsync(id);
        }

    }
}
