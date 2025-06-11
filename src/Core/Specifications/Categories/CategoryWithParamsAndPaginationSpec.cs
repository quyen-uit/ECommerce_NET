using Core.Specifications.Categories;

public class CategoryWithParamsAndPaginationSpec : CategoryWithParamsSpec
{
    public CategoryWithParamsAndPaginationSpec(CategorySpecParams specParams)
        : base(specParams)
    {
        AddPagination(specParams.PageSize, specParams.PageNumber);
    }
}
