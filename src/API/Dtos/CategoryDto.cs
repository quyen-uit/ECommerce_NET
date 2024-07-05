using Core.Common;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

    }
} 