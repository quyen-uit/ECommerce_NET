using Core.Dtos.Images;
using Core.Enums;

namespace Core.Interfaces.Services
{
    public interface IImageService
    {
        Task ProcessImagesAsync(CreateListImageDto imageDtos);
        Task<List<ImageDto>> GetAllImageByRefIdAsync(long refId, ImageType type);

    }
}
