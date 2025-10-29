using Core.Common;
using Core.Constants;
using Core.Dtos.PriceAdjustments;
using Core.Entities;
using Ardalis.Specification;
using Core.Interfaces.Services;
using Core.Specifications.PriceAdjustments;
using Mapster;

namespace API.Services
{
    public class PriceAdjustmentService : IPriceAdjustmentService
    {
        private readonly IRepositoryBase<PriceAdjustment> _priceAdjustmentRepository;

        public PriceAdjustmentService(IRepositoryBase<PriceAdjustment> PriceAdjustmentRepository)
        {
            _priceAdjustmentRepository = PriceAdjustmentRepository;
        }

        public async Task<PriceAdjustmentDto> AddOrUpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto priceAdjustmentDto)
        {
            PriceAdjustment? existingAdjustment = null;
            if (priceAdjustmentDto.Id.HasValue && priceAdjustmentDto.Id != Guid.Empty)
            {
                existingAdjustment = await _priceAdjustmentRepository.GetByIdAsync(priceAdjustmentDto.Id.Value);
            }

            PriceAdjustment resultEntity;
            if (existingAdjustment == null)
            {
                var adjustment = priceAdjustmentDto.Adapt<PriceAdjustment>();
                await _priceAdjustmentRepository.AddAsync(adjustment);
                resultEntity = adjustment;
            }
            else
            {
                // Map scalar fields
                priceAdjustmentDto.Adapt(existingAdjustment);

                // Synchronize child items
                var existingItems = existingAdjustment.PriceAdjustmentItems.ToList();

                // Remove items not present in DTO
                var dtoItemIds = priceAdjustmentDto.PriceAdjustmentItems
                    .Where(i => i.Id.HasValue && i.Id != Guid.Empty)
                    .Select(i => i.Id!.Value)
                    .ToHashSet();
                var itemsToRemove = dtoItemIds.Count == 0
                    ? existingItems
                    : existingItems.Where(i => !dtoItemIds.Contains(i.Id)).ToList();
                foreach (var item in itemsToRemove)
                {
                    existingAdjustment.PriceAdjustmentItems.Remove(item);
                }

                // Update or add items
                foreach (var dtoItem in priceAdjustmentDto.PriceAdjustmentItems)
                {
                    var existingItem = dtoItem.Id.HasValue ? existingItems.FirstOrDefault(i => i.Id == dtoItem.Id.Value) : null;
                    if (existingItem != null)
                    {
                        // Map into tracked entity
                        dtoItem.Adapt(existingItem);
                    }
                    else
                    {
                        existingAdjustment.PriceAdjustmentItems.Add(dtoItem.Adapt<PriceAdjustmentItem>());
                    }
                }
                await _priceAdjustmentRepository.UpdateAsync(existingAdjustment);
                resultEntity = existingAdjustment;
            }

            return resultEntity.Adapt<PriceAdjustmentDto>();
        }

        public async Task DeletePriceAdjustmentAsync(Guid id)
        {
            var existing = await _priceAdjustmentRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundPriceAdjustment);
            await _priceAdjustmentRepository.DeleteAsync(existing);
        }

        public async Task<Pagination<PriceAdjustmentDto>> GetAllPriceAdjustmentAsync(PriceAdjustmentSpecParams specParams)
        {
            var spec = new PriceAdjustmentSpecification(specParams);
            var priceAdjustments = await _priceAdjustmentRepository.ListAsync(spec);
            var specCount = new PriceAdjustmentSpecification(specParams, isSearch: false);
            var count = await _priceAdjustmentRepository.CountAsync(specCount);
            return new Pagination<PriceAdjustmentDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: priceAdjustments.Adapt<IReadOnlyList<PriceAdjustmentDto>>()
                );
        }

        public async Task<PriceAdjustmentDto> GetPriceAdjustmentByIdAsync(Guid id)
        {
            var priceAdjustment = await _priceAdjustmentRepository.GetByIdAsync(id);
            if (priceAdjustment == null)
                throw new NotFoundException(CommonMessage.NotFoundPriceAdjustment);
            return priceAdjustment.Adapt<PriceAdjustmentDto>();
        }
        public Task<PriceAdjustmentDto> UpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto dto)
        {
            throw new NotImplementedException();
        }

    }
}
