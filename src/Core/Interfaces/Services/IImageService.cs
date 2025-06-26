

using Core.Common;
using Core.Dtos;
using Core.Dtos.Images;
using Core.Entities;
using Core.Enums;
using Core.Specifications.Products;

namespace Core.Interfaces.Services
{
    public interface IImageService
    {
        Task ProcessImagesAsync(CreateListImageDto imageDtos);
        Task<List<ImageDto>> GetAllImageByRefIdAsync(long refId, ImageType type);

    }
}
