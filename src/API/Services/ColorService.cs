using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.Colors;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ColorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ColorDto> AddOrUpdateColorAsync(CreateColorDto dto)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            Color? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await colorRepo.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Color>();
                await colorRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await colorRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
            return entity.Adapt<ColorDto>();
        }

        public async Task<IReadOnlyList<ColorDto>> AddRangeColorAsync(
            IReadOnlyList<CreateColorDto> colorDtos
        )
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var colors = colorDtos.Adapt<IReadOnlyList<Color>>();
            await colorRepo.AddRangeAsync(colors);
            await _unitOfWork.SaveChangesAsync();
            return colors.Adapt<IReadOnlyList<ColorDto>>();
        }

        public async Task<int> CountAllAsync(ColorSpecParams specParams)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var countSpec = new ColorSpecification(specParams);
            return await colorRepo.CountAsync(countSpec);
        }

        public async Task DeleteColorAsync(Guid id)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var existing = await colorRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundColor);
            await colorRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<ColorDto>> GetAllColorsAsync(ColorSpecParams specParams)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var spec = new ColorSpecification(specParams);
            var colors = await colorRepo.ListAsync(spec);
            var specCount = new ColorSpecification(specParams, isSearch: false);
            var count = await colorRepo.CountAsync(specCount);
            return new Pagination<ColorDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: colors.Adapt<IReadOnlyList<ColorDto>>()
                );
        }

        public async Task<ColorDto> GetColorByIdAsync(Guid id)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var color = await colorRepo.GetByIdAsync(id);
            if (color == null)
                throw new NotFoundException(CommonMessage.NotFoundColor);
            return color.Adapt<ColorDto>();
        }

    }
}
