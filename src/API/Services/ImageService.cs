using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Colors;
using Core.Specifications.Images;
using Core.Specifications.Products;
using Core.Specifications.Sizes;
using Mapster;

namespace API.Services
{
    public class ImageService : IImageService
    {
        private readonly IGenericRepository<Image> _imageRepository;

        public ImageService(IGenericRepository<Image> imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<List<ImageDto>> GetAllImageByRefIdAsync(long refId, ImageType type)
        {
            var spec = new ImageByRefIdSpecification(refId, type);
            var images = await _imageRepository.GetAllWithSpecAsync(spec);
            return images.Adapt<List<ImageDto>>();
        }


        public async Task ProcessImagesAsync(List<CreateOrUpdateImageDto> imageDtos)
        {

            foreach (var dto in imageDtos)
            {
                if (dto.IsDelete)
                {
                    if (dto.Id.HasValue)
                        _imageRepository.Delete(dto.Id.Value);
                }
                else if (dto.Id.HasValue)
                {
                    var existingImage = await _imageRepository.GetByIdAsync(dto.Id.Value);
                    if (existingImage != null)
                    {
                        existingImage = dto.Adapt<Image>();
                        _imageRepository.Update(existingImage);
                    }
                }
                else
                {
                    var newImage = dto.Adapt<Image>();
                    _imageRepository.Add(newImage);
                }
            }
            await _imageRepository.Complete();
        }
    }
}
