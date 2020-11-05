using System.ComponentModel.DataAnnotations;

namespace ProjectOrderFood.Enums
{
    public enum WorkshopType
    {
        [Display (Name = "Кухня")]
        Kitchen = 1,
        [Display (Name = "Бар")]
        Bar = 2
    }
}