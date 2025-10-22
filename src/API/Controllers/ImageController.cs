using Core.Constants;
using Core.Dtos.Images;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ImageController : ApiControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("process")]
        public async Task<ActionResult> CreateImage(
            [FromBody] CreateListImageDto dtos
        )
        {
            await _imageService.ProcessImagesAsync(dtos);
            return Ok(CommonMessage.CreateSuccess);
        }
    }
}
