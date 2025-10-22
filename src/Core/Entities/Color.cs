using Core.Common.Entities;

namespace Core.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

    }

}
