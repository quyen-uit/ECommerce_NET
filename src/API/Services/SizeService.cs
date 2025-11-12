using Core.Exceptions;
using Core.Common;
using Core.Constants;
using Core.Dtos.Sizes;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Sizes;
using Core.Interfaces.Reposiories;
using Core.Interfaces;
using Mapster;

namespace API.Services
{
    public class SizeService : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SizeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SizeDto> AddOrUpdateSizeAsync(CreateSizeDto dto)
        {
            if (dto == null)
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var sizeRepo = _unitOfWork.Repository<Size>();
            Size? entity = null;
            if (dto.Id.HasValue && dto.Id != Guid.Empty)
            {
                entity = await sizeRepo.GetByIdAsync(dto.Id.Value);
            }
            if (entity == null)
            {
                entity = dto.Adapt<Size>();
                await sizeRepo.AddAsync(entity);
            }
            else
            {
                dto.Adapt(entity);
                await sizeRepo.UpdateAsync(entity);
            }
            await _unitOfWork.SaveChangesAsync();
            return entity.Adapt<SizeDto>();
        }

        public async Task<IReadOnlyList<SizeDto>> AddRangeSizeAsync(
            IReadOnlyList<CreateSizeDto> dtos
        )
        {
            if (dtos == null || !dtos.Any())
            {
                throw new BadRequestException(CommonMessage.CreateFail);
            }
            var sizeRepo = _unitOfWork.Repository<Size>();
            var entities = dtos.Adapt<IReadOnlyList<Size>>();
            await sizeRepo.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return entities.Adapt<IReadOnlyList<SizeDto>>();
        }

        public async Task DeleteSizeAsync(Guid id)
        {
            var sizeRepo = _unitOfWork.Repository<Size>();
            var existing = await sizeRepo.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);

            await sizeRepo.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<SizeDto>> GetAllSizesAsync(SizeSpecParams specParams)
        {
            var sizeRepo = _unitOfWork.Repository<Size>();
            var spec = new SizeSpecification(specParams);
            var entities = await sizeRepo.ListAsync(spec);
            var specCount = new SizeSpecification(specParams, false);
            var count = await sizeRepo.CountAsync(specCount);
            return new Pagination<SizeDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: count,
                    data: entities.Adapt<IReadOnlyList<SizeDto>>()
                );
        }

        public async Task<SizeDto> GetSizeByIdAsync(Guid id)
        {
            var sizeRepo = _unitOfWork.Repository<Size>();
            var entity = await sizeRepo.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundSize);
            return entity.Adapt<SizeDto>();
        }

        public async Task DeleteSizesAsync(List<Guid> ids)
        {
            var sizeRepo = _unitOfWork.Repository<Size>();
            var toDelete = new List<Size>();
            foreach (var id in ids)
            {
                var entity = await sizeRepo.GetByIdAsync(id);
                if (entity != null)
                {
                    toDelete.Add(entity);
                }
            }
            if (toDelete.Count > 0)
            {
                await sizeRepo.DeleteRangeAsync(toDelete);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
