namespace NebulaMigration.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum WorkshopType
    {
        [Display(Name = "Кухня")]
        Kitchen = 1,

        [Display(Name = "Бар")]
        Bar = 2
    }
}