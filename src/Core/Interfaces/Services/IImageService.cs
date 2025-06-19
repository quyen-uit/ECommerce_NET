

using Core.Common;
using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Specifications.Products;

namespace Core.Interfaces.Services
{
    public interface IImageService
    {
        Task ProcessImagesAsync(List<CreateOrUpdateImageDto> imageDtos);
        Task<List<ImageDto>> GetAllImageByRefIdAsync(long refId, ImageType type);

    }
}
