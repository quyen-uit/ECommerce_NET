using Core.Dtos.Images;
using Core.Exceptions;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Specifications.Images;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ImageDto>> GetAllImageByRefIdAsync(Guid refId, ImageType type)
        {
            var imageRepo = _unitOfWork.Repository<Image>();
            var spec = new ImageByRefIdSpecification(refId, type);
            var images = await imageRepo.ListAsync(spec);
            return images.Adapt<List<ImageDto>>();
        }


        public async Task ProcessImagesAsync(CreateListImageDto listImageDto)
        {
            if (listImageDto.CreateImageDtos.Count > 0)
            {
                var imageRepo = _unitOfWork.Repository<Image>();
                // update Items
                var spec = new ImageByRefIdSpecification(listImageDto.ReferenceId, listImageDto.CreateImageDtos.First().Type);
                var existingItems = await imageRepo.ListAsync(spec);

                // Remove items not in the new DTO
                var dtoItemIds = listImageDto.CreateImageDtos
                    .Where(i => i.Id.HasValue && i.Id != Guid.Empty)
                    .Select(i => i.Id!.Value)
                    .ToHashSet();
                var itemsToRemove = dtoItemIds.Count == 0
                    ? new List<Image>()
                    : existingItems.Where(i => !dtoItemIds.Contains(i.Id)).ToList();

                await imageRepo.DeleteRangeAsync(itemsToRemove);

                // Update or add items
                foreach (var dtoItem in listImageDto.CreateImageDtos)
                {
                    var existingItem = dtoItem.Id.HasValue ? existingItems.FirstOrDefault(i => i.Id == dtoItem.Id.Value) : null;
                    if (existingItem != null)
                    {
                        existingItem = dtoItem.Adapt<Image>();
                        await imageRepo.UpdateAsync(existingItem);
                    }
                    else
                    {
                        await imageRepo.AddAsync(dtoItem.Adapt<Image>());
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
