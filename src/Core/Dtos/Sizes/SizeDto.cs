using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos.Sizes
{
    public class SizeDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public int SortOrder { get; set; }
        public SizeType SizeType { get; set; }
    }

}
