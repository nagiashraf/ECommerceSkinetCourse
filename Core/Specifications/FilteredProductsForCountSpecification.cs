using Core.Entities;

namespace Core.Specifications
{
    public class FilteredProductsForCountSpecification : BaseSpecifcation<Product>
    {
        public FilteredProductsForCountSpecification(ProductSpecParams productParams)
            : base(p =>
                (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId) &&
                (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
                (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search))
            )
        {
        }
    }
}