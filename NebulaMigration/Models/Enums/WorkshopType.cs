using System.ComponentModel.DataAnnotations;

namespace NebulaMigration.Models.Enums
{
    public enum WorkshopType
    {
        [Display (Name = "Кухня")]
        Kitchen = 1,
        [Display (Name = "Бар")]
        Bar = 2
    }
}