namespace NebulaMigration.Models
{
    using System.ComponentModel.DataAnnotations;

    public class GoodsGroup
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Code { get; set; }
    }
}
