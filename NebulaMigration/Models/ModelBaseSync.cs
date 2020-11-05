using System;

namespace NebulaMigration.Models
{
    public class ModelBaseSync
    {
        public ModelBaseSync()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ExternalId { get; set; }
    }
}