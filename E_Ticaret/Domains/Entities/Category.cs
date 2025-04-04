using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public ICollection<CategoryRelation> ParentRelations { get; set; } = new List<CategoryRelation>();

        public ICollection<CategoryRelation> ChildRelations { get; set; } = new List<CategoryRelation>();
    }
}
