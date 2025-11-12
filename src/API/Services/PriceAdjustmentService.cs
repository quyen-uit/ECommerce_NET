using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.PriceAdjustments;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.PriceAdjustments;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class PriceAdjustmentService : IPriceAdjustmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriceAdjustmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PriceAdjustmentDto> AddOrUpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto priceAdjustmentDto)
        {
            var priceAdjustmentRepo = _unitOfWork.Repository<PriceAdjustment>();
            PriceAdjustment? existingAdjustment = null;
            if (priceAdjustmentDto.Id.HasValue && priceAdjustmentDto.Id != Guid.Empty)
            {
                existingAdjustment = await priceAdjustmentRepo.GetByIdAsync(priceAdjustmentDto.Id.Value);
            }

            PriceAdjustment resultEntity;
            if (existingAdjustment == null)
            {
                var adjustment = priceAdjustmentDto.Adapt<PriceAdjustment>();
                await priceAdjustmentRepo.AddAsync(adjustment);
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
                await priceAdjustmentRepo.UpdateAsync(existingAdjustment);
                resultEntity = existingAdjustment;
            }

            await _unitOfWork.SaveChangesAsync();
            return resultEntity.Adapt<PriceAdjustmentDto>();
        }

        public async Task DeletePriceAdjustmentAsync(Guid id)
        {
            var priceAdjustmentRepo = _unitOfWork.Repository<PriceAdjustment>();
            var existing = await priceAdjustmentRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundPriceAdjustment);
            await priceAdjustmentRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<PriceAdjustmentDto>> GetAllPriceAdjustmentAsync(PriceAdjustmentSpecParams specParams)
        {
            var priceAdjustmentRepo = _unitOfWork.Repository<PriceAdjustment>();
            var spec = new PriceAdjustmentSpecification(specParams);
            var priceAdjustments = await priceAdjustmentRepo.ListAsync(spec);
            var specCount = new PriceAdjustmentSpecification(specParams, isSearch: false);
            var count = await priceAdjustmentRepo.CountAsync(specCount);
            return new Pagination<PriceAdjustmentDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: priceAdjustments.Adapt<IReadOnlyList<PriceAdjustmentDto>>()
                );
        }

        public async Task<PriceAdjustmentDto> GetPriceAdjustmentByIdAsync(Guid id)
        {
            var priceAdjustmentRepo = _unitOfWork.Repository<PriceAdjustment>();
            var priceAdjustment = await priceAdjustmentRepo.GetByIdAsync(id);
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
