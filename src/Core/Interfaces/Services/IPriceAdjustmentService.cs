using Core.Common;
using Core.Dtos;
using Core.Dtos.PriceAdjustments;
using Core.Entities;
using Core.Specifications.Colors;
using Core.Specifications.PriceAdjustments;
using Core.Specifications.Sizes;

namespace Core.Interfaces.Services
{
    public interface IPriceAdjustmentService
    {
        Task<Pagination<PriceAdjustmentDto>> GetAllPriceAdjustmentAsync(PriceAdjustmentSpecParams specParams);
        Task<PriceAdjustmentDto> GetPriceAdjustmentByIdAsync(long id);
        Task<PriceAdjustmentDto> AddOrUpdatePriceAdjustmentAsync(CreatePriceAdjustmentDto dto);
        Task DeletePriceAdjustmentAsync(long id);
    }
}
