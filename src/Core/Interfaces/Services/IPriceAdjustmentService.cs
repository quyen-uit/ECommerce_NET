using Core.Common;
using Core.Dtos.PriceAdjustments;
using Core.Specifications.PriceAdjustments;

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
