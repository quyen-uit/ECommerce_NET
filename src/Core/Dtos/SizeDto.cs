using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos
{
    public class SizeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }

    public class CreateSizeDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }

    public class UpdateSizeDto : CreateSizeDto
    {
        public long Id { get; set; }
    }
}
