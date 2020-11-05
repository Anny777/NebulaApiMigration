using ProjectOrderFood.Enums;

namespace NebulaApi.Models
{
    public class Category : ModelBaseSync
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public WorkshopType WorkshopType { get; set; }
    }
}