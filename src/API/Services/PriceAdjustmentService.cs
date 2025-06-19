using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
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

        public async Task<PriceAdjustmentDto> AddPriceAdjustmentAsync(CreatePriceAdjustmentDto priceAdjustmentDto)
        {
            var priceAdjustment = priceAdjustmentDto.Adapt<PriceAdjustment>();
            _priceAdjustmentRepository.Add(priceAdjustment);
            await _priceAdjustmentRepository.Complete();
            return priceAdjustment.Adapt<PriceAdjustmentDto>();
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

        public async Task<PriceAdjustmentDto> UpdatePriceAdjustmentAsync(UpdatePriceAdjustmentDto priceAdjustmentDto)
        {
            var existingAdjustment = await _priceAdjustmentRepository.GetByIdAsync(priceAdjustmentDto.Id);
            if (existingAdjustment == null)
                throw new NotFoundException(CommonMessage.NotFoundPriceAdjustment);

            // update Items
            var adjustment = priceAdjustmentDto.Adapt<PriceAdjustment>();
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
                    existingItem.ProductId = dtoItem.ProductId;
                    existingItem.SalePrice = dtoItem.SalePrice;
                }
                else
                {
                    adjustment.PriceAdjustmentItems.Add(new PriceAdjustmentItem
                    {
                        ProductId = dtoItem.ProductId,
                        SalePrice = dtoItem.SalePrice
                    });
                }
            }
            
            await _priceAdjustmentRepository.Complete();

            return adjustment.Adapt<PriceAdjustmentDto>();
        }

        public Task<PriceAdjustmentDto> UpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto dto)
        {
            throw new NotImplementedException();
        }

    }
}
