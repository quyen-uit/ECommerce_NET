using AutoMapper;
using Core.Dtos;
using Core.Dtos.CreateDto;
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

        public async Task<SizeDto> AddSizeAsync(CreateSizeDto dto)
        {
            var entity = dto.Adapt<Size>();
            _sizeRepository.Add(entity);
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

        public async Task<bool> DeleteSizeAsync(long id)
        {
            var existing = await _sizeRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _sizeRepository.Delete(id);
            await _sizeRepository.Complete();
            return true;
        }

        public async Task<IReadOnlyList<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams)
        {
            var spec = new SizeWithParamsSpec(specParams);
            var entities = await _sizeRepository.GetAllWithSpecAsync(spec);
            return entities.Adapt<IReadOnlyList<SizeDto>>();
        }

        public async Task<SizeDto> GetSizeByIdAsync(long id)
        {
            var entity = await _sizeRepository.GetByIdAsync(id);
            return entity?.Adapt<SizeDto>();
        }

        public async Task<SizeDto> UpdateSizeAsync(UpdateSizeDto dto)
        {
            var existing = await _sizeRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                return null;

            existing = dto.Adapt<Size>();
            _sizeRepository.Update(existing);
            await _sizeRepository.Complete();
            return existing.Adapt<SizeDto>();
        }
    }
}
