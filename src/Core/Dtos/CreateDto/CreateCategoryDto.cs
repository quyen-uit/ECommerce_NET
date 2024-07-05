using Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }
        public bool IsActive { get; set; }

    }
} 