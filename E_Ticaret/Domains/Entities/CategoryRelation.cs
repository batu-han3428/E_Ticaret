namespace E_Ticaret.Domains.Entities
{
    public class CategoryRelation
    {
        public long ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }

        public long ChildCategoryId { get; set; }

        public Category ChildCategory { get; set; }
    }
}
