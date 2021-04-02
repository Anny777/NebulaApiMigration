namespace NebulaMigration.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Состояние готовности блюда.
    /// </summary>
    public enum DishState
    {
        [Display(Name = "Удалено.")]
        Deleted = 1,

        [Display(Name = "В работе.")]
        InWork = 2,

        [Display(Name = "Готово.")]
        Ready = 3,
        
        [Display(Name = "Забрано.")]
        Taken = 4,
        
        [Display(Name = "Запрошено удаление.")]
        CancellationRequested = 5
    }
}