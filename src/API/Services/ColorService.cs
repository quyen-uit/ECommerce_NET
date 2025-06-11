using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Core.Specifications.Products;
using Mapster;

namespace API.Services
{
    public class ColorService : IColorService
    {
        private readonly IGenericRepository<Color> _colorRepository;

        public ColorService(IGenericRepository<Color> colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<ColorDto> AddColorAsync(CreateColorDto colorDto)
        {
            var color = colorDto.Adapt<Color>();
            _colorRepository.Add(color);
            await _colorRepository.Complete();
            return color.Adapt<ColorDto>();
        }

        public async Task<IReadOnlyList<ColorDto>> AddRangeColorAsync(
            IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var colors = colorDtos.Adapt<IReadOnlyList<Color>>();
            _colorRepository.AddRange(colors);
            await _colorRepository.Complete();
            return colors.Adapt<IReadOnlyList<ColorDto>>();
        }

        public async Task<int> CountAllAsync(ColorSpecParams specParams)
        {
            var countSpec = new ColorWithParamsSpec(specParams);
            return await _colorRepository.CountAsync(countSpec);
        }

        public async Task<bool> DeleteColorAsync(long id)
        {
            var existing = await _colorRepository.GetByIdAsync(id);
            if (existing == null)
                return false;
            _colorRepository.Delete(id);
            await _colorRepository.Complete();
            return true;
        }

        public async Task<IReadOnlyList<ColorDto>> GetAllColorsAsync(ColorSpecParams specParams)
        {
            var spec = new ColorWithParamsAndPaginationSpec(specParams);
            var colors = await _colorRepository.GetAllWithSpecAsync(spec);
            return colors.Adapt<IReadOnlyList<ColorDto>>();
        }

        public async Task<ColorDto> GetColorByIdAsync(long id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            return color?.Adapt<ColorDto>();
        }

        public async Task<ColorDto> UpdateColorAsync(UpdateColorDto colorDto)
        {
            var existing = await _colorRepository.GetByIdAsync(colorDto.Id);
            if (existing == null)
                return null;

            existing = colorDto.Adapt<Color>();
            await _colorRepository.Complete();

            return existing.Adapt<ColorDto>();
        }
    }
}
