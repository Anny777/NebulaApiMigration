using System;

namespace NebulaApi.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set;}
    }
}