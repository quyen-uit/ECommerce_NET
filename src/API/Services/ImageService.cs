using AutoMapper;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Dtos.Images;
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


        public async Task ProcessImagesAsync(CreateListImageDto listImageDto)
        {
            if (listImageDto.CreateImageDtos.Count > 0)
            {
                // update Items
                var spec = new ImageByRefIdSpecification(listImageDto.ReferenceId, listImageDto.CreateImageDtos.First().Type);
                var existingItems = await _imageRepository.GetAllWithSpecAsync(spec);

                // Remove items not in the new DTO
                var dtoItemIds = listImageDto.CreateImageDtos.Select(i => i.Id).ToHashSet();
                var itemsToRemove = existingItems.Where(i => !dtoItemIds.Contains(i.Id)).ToList();
                foreach (var item in itemsToRemove)
                {
                    _imageRepository.Delete(item.Id);
                }

                // Update or add items
                foreach (var dtoItem in listImageDto.CreateImageDtos)
                {
                    var existingItem = existingItems.FirstOrDefault(i => i.Id == dtoItem.Id);
                    if (existingItem != null)
                    {
                        existingItem = dtoItem.Adapt<Image>();
                        _imageRepository.Update(existingItem);
                    }
                    else
                    {
                        _imageRepository.Add(dtoItem.Adapt<Image>());
                    }
                }
                
                await _imageRepository.Complete();
            }
        }
    }
}
