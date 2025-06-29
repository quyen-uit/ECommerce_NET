using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Dtos.Sizes;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Core.Specifications.Products;
using Core.Specifications.Sizes;
using Mapster;

namespace API.Services
{
    public class SizeService : ISizeService
    {
        private readonly IGenericRepository<Size> _sizeRepository;

        public SizeService(IGenericRepository<Size> sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<SizeDto> AddOrUpdateSizeAsync(CreateSizeDto dto)
        {
            var entity = await _sizeRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                entity = dto.Adapt<Size>();
                _sizeRepository.Add(entity);
            }
            else
            {
                entity = dto.Adapt<Size>();
                _sizeRepository.Update(entity);
            }
            await _sizeRepository.Complete();
            return entity.Adapt<SizeDto>();
        }

        public async Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(
            IReadOnlyList<CreateSizeDto> dtos
        )
        {
            var entities = dtos.Adapt<IReadOnlyList<Size>>();
            _sizeRepository.AddRange(entities);
            await _sizeRepository.Complete();
            return entities.Adapt<IReadOnlyList<SizeDto>>();
        }

        public async Task DeleteSizeAsync(long id)
        {
            var existing = await _sizeRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);

            _sizeRepository.Delete(id);
            await _sizeRepository.Complete();
        }

        public async Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams)
        {
            var spec = new SizeSpecification(specParams);
            var entities = await _sizeRepository.GetAllWithSpecAsync(spec);
            var specCount = new SizeSpecification(specParams, false);
            var count = await _sizeRepository.CountAsync(specCount);
            return new Pagination<SizeDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: entities.Adapt<IReadOnlyList<SizeDto>>()
                );
        }

        public async Task<SizeDto> GetSizeByIdAsync(long id)
        {
            var entity = await _sizeRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);
            return entity.Adapt<SizeDto>();
        }
    }
}
