using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.PriceAdjustments;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.PriceAdjustments;
using Core.Specifications.Products;
using Core.Specifications.Sizes;
using Mapster;

namespace API.Services
{
    public class PriceAdjustmentService : IPriceAdjustmentService
    {
        private readonly IGenericRepository<PriceAdjustment> _priceAdjustmentRepository;

        public PriceAdjustmentService(IGenericRepository<PriceAdjustment> PriceAdjustmentRepository)
        {
            _priceAdjustmentRepository = PriceAdjustmentRepository;
        }

        public async Task<PriceAdjustmentDto> AddOrUpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto priceAdjustmentDto)
        {
            var existingAdjustment = await _priceAdjustmentRepository.GetByIdAsync(priceAdjustmentDto.Id);
            var adjustment = priceAdjustmentDto.Adapt<PriceAdjustment>();

            if (existingAdjustment == null)
            {
                _priceAdjustmentRepository.Add(adjustment);
            }
            else
            {
                // update Items
                var existingItems = adjustment.PriceAdjustmentItems = existingAdjustment.PriceAdjustmentItems.ToList();

                // Remove items not in the new DTO
                var dtoItemIds = priceAdjustmentDto.PriceAdjustmentItems.Select(i => i.Id).ToHashSet();
                var itemsToRemove = existingItems.Where(i => !dtoItemIds.Contains(i.Id)).ToList();
                foreach (var item in itemsToRemove)
                {
                    adjustment.PriceAdjustmentItems.Remove(item);
                }

                // Update or add items
                foreach (var dtoItem in priceAdjustmentDto.PriceAdjustmentItems)
                {
                    var existingItem = existingItems.FirstOrDefault(i => i.Id == dtoItem.Id);
                    if (existingItem != null)
                    {
                        existingItem = dtoItem.Adapt<PriceAdjustmentItem>();
                    }
                    else
                    {
                        adjustment.PriceAdjustmentItems.Add(dtoItem.Adapt<PriceAdjustmentItem>());
                    }
                }
                _priceAdjustmentRepository.Update(adjustment);
            }

            await _priceAdjustmentRepository.Complete();

            return adjustment.Adapt<PriceAdjustmentDto>();
        }

        public async Task DeletePriceAdjustmentAsync(long id)
        {
            var existing = await _priceAdjustmentRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundPriceAdjustment);
            _priceAdjustmentRepository.Delete(id);
            await _priceAdjustmentRepository.Complete();
        }

        public async Task<Pagination<PriceAdjustmentDto>> GetAllPriceAdjustmentAsync(PriceAdjustmentSpecParams specParams)
        {
            var spec = new PriceAdjustmentSpecification(specParams);
            var priceAdjustments = await _priceAdjustmentRepository.GetAllWithSpecAsync(spec);
            var count = await _priceAdjustmentRepository.CountAsync(spec);
            return new Pagination<PriceAdjustmentDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: priceAdjustments.Adapt<IReadOnlyList<PriceAdjustmentDto>>()
                );
        }

        public async Task<PriceAdjustmentDto> GetPriceAdjustmentByIdAsync(long id)
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
