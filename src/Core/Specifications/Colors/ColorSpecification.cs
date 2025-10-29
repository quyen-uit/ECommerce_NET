using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Colors
{
    public class ColorSpecification : Specification<Color>
    {
        public ColorSpecification() { }

        public ColorSpecification(ColorSpecParams specParams, bool isSearch = true)
        {
            Query.Where(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.HexCode) || specParams.Filter.HexCode.ToLower().Contains(x.HexCode.ToLower()))
            );
            if (isSearch)
            {
                Query.Skip(specParams.PageSize * (specParams.PageNumber - 1))
                     .Take(specParams.PageSize);
                switch (specParams.Sort)
                {
                    case "name_asc":
                        Query.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        Query.OrderByDescending(x => x.Name);
                        break;
                    default:
                        Query.OrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}
