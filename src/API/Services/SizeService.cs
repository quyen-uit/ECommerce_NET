using API.Exceptions;
using Core.Common;
using Core.Constants;
using Core.Dtos.Sizes;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Sizes;
using Core.Interfaces.Reposiories;
using Mapster;

namespace API.Services
{
    public class SizeService : ISizeService
    {
        private readonly IRepository<Size> _sizeRepository;

        public SizeService(IRepository<Size> sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<SizeDto> AddOrUpdateSizeAsync(CreateSizeDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            Size? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await _sizeRepository.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Size>();
                await _sizeRepository.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await _sizeRepository.UpdateAsync(entity);
            }
            return entity.Adapt<SizeDto>();
        }

        public async Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(
            IReadOnlyList<CreateSizeDto> dtos
        )
        {
            if (dtos == null || !dtos.Any())
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var entities = dtos.Adapt<IReadOnlyList<Size>>();
            await _sizeRepository.AddRangeAsync(entities);
            return entities.Adapt<IReadOnlyList<SizeDto>>();
        }

        public async Task DeleteSizeAsync(Guid id)
        {
            var existing = await _sizeRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);

            await _sizeRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams)
        {
            var spec = new SizeSpecification(specParams);
            var entities = await _sizeRepository.ListAsync(spec);
            var specCount = new SizeSpecification(specParams, false);
            var count = await _sizeRepository.CountAsync(specCount);
            return new Pagination<SizeDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: entities.Adapt<IReadOnlyList<SizeDto>>()
                );
        }

        public async Task<SizeDto> GetSizeByIdAsync(Guid id)
        {
            var entity = await _sizeRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);
            return entity.Adapt<SizeDto>();
        }

        public async Task DeleteSizesAsync(List<Guid> ids)
        {
            var toDelete = new List<Size>();
            foreach (var id in ids)
            {
                var entity = await _sizeRepository.GetByIdAsync(id);
                if (entity != null)
                {
                    toDelete.Add(entity);
                }
            }
            if (toDelete.Count > 0)
            {
                await _sizeRepository.DeleteRangeAsync(toDelete);
            }
        }
    }
}
