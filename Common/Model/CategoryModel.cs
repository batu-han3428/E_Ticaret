namespace Common.Model
{
    public class CategoryModel
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public List<CategoryModel> ParentCategories { get; set; } = new List<CategoryModel>();

        public List<CategoryModel> ChildCategories { get; set; } = new List<CategoryModel>();
    }
}
