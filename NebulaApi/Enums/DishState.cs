using System.ComponentModel.DataAnnotations;

namespace ProjectOrderFood.Enums
{

    public enum DishState
    {
        Deleted = 1,
        InWork = 2,
        Ready = 3,
        Taken = 4,
        CancellationRequested = 5
    }
}