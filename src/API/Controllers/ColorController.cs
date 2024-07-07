using Core.Dtos;
using Core.Dtos.CreateDto;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ColorController : ApiControllerBase
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;

        public ColorController(IColorService colorService, IMapper mapper)
        {
            _colorService = colorService;
            _mapper = mapper;
        }

        // GET: api/Color
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<ColorDto>>> GetColors()
        {
            var colors = await _colorService.GetAllColorsAsync();
            var colorDtos = _mapper.Map<IReadOnlyList<ColorDto>>(colors);

            return Ok(colorDtos);
        }


        // POST: api/Color
        [HttpPost]
        public async Task<ActionResult<ColorDto>> PostColor(CreateColorDto colorDto)
        {
            var result = await _colorService.AddColorAsync(colorDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<ColorDto>(result));
        }

        // POST: api/Color/many
        [HttpPost("many")]
        public async Task<ActionResult<ColorDto>> PostColors(IReadOnlyList<CreateColorDto> colorDtos)
        {
            var result = await _colorService.AddRangeColorAsync(colorDtos);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<IReadOnlyList<ColorDto>>(result));
        }

        // PUT: api/Color/5
        [HttpPut]
        public async Task<IActionResult> PutColor(ColorDto colorDto)
        {
            var result = await _colorService.UpdateColorAsync(colorDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<ColorDto>(result));
        }

        // DELETE: api/Color/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _colorService.GetColorByIdAsync(id);
            if (color == null)
            {
                return NotFound(new ApiException(404));
            }

            var result = await _colorService.DeleteColorAsync(id);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Deleting fail"));
            }
            return Ok("Delete succesfully");
        }
    }
}
