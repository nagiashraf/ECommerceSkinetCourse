using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecifcation<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(p =>
                (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId) &&
                (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
                (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search))
            )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDsc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}