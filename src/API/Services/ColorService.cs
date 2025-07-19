using API.Exceptions;
using Core.Common;
using Core.Constants;
using Core.Dtos.Colors;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
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

        public async Task<ColorDto> AddOrUpdateColorAsync(CreateColorDto dto)
        {
            var entity = await _colorRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                entity = dto.Adapt<Color>();
                _colorRepository.Add(entity);
            }
            else
            {
                entity = dto.Adapt<Color>();
                _colorRepository.Update(entity);
            }
            await _colorRepository.Complete();
            return entity.Adapt<ColorDto>();
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
            var countSpec = new ColorSpecification(specParams);
            return await _colorRepository.CountAsync(countSpec);
        }

        public async Task DeleteColorAsync(long id)
        {
            var existing = await _colorRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundColor);
            _colorRepository.Delete(existing);
            await _colorRepository.Complete();
        }

        public async Task<Pagination<ColorDto>> GetAllColorsAsync(ColorSpecParams specParams)
        {
            var spec = new ColorSpecification(specParams);
            var colors = await _colorRepository.GetAllWithSpecAsync(spec);
            var count = await _colorRepository.CountAsync(spec);
            return new Pagination<ColorDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: colors.Adapt<IReadOnlyList<ColorDto>>()
                );
        }

        public async Task<ColorDto> GetColorByIdAsync(long id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
                throw new NotFoundException(CommonMessage.NotFoundColor);
            return color.Adapt<ColorDto>();
        }

    }
}
