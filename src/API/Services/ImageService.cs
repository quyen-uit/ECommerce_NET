using Core.Dtos.Images;
using Core.Entities;
using Core.Enums;
using Ardalis.Specification;
using Core.Interfaces.Services;
using Core.Specifications.Images;
using Mapster;

namespace API.Services
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryBase<Image> _imageRepository;

        public ImageService(IRepositoryBase<Image> imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<List<ImageDto>> GetAllImageByRefIdAsync(Guid refId, ImageType type)
        {
            var spec = new ImageByRefIdSpecification(refId, type);
            var images = await _imageRepository.ListAsync(spec);
            return images.Adapt<List<ImageDto>>();
        }


        public async Task ProcessImagesAsync(CreateListImageDto listImageDto)
        {
            if (listImageDto.CreateImageDtos.Count > 0)
            {
                // update Items
                var spec = new ImageByRefIdSpecification(listImageDto.ReferenceId, listImageDto.CreateImageDtos.First().Type);
                var existingItems = await _imageRepository.ListAsync(spec);

                // Remove items not in the new DTO
                var dtoItemIds = listImageDto.CreateImageDtos
                    .Where(i => i.Id.HasValue && i.Id != Guid.Empty)
                    .Select(i => i.Id!.Value)
                    .ToHashSet();
                var itemsToRemove = dtoItemIds.Count == 0
                    ? new List<Image>()
                    : existingItems.Where(i => !dtoItemIds.Contains(i.Id)).ToList();

                await _imageRepository.DeleteRangeAsync(itemsToRemove); // test

                // Update or add items
                foreach (var dtoItem in listImageDto.CreateImageDtos)
                {
                    var existingItem = dtoItem.Id.HasValue ? existingItems.FirstOrDefault(i => i.Id == dtoItem.Id.Value) : null;
                    if (existingItem != null)
                    {
                        existingItem = dtoItem.Adapt<Image>();
                        await _imageRepository.UpdateAsync(existingItem);
                    }
                    else
                    {
                        await _imageRepository.AddAsync(dtoItem.Adapt<Image>());
                    }
                }

            }
        }
    }
}
