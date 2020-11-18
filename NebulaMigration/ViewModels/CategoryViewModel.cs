using System;
using NebulaMigration.Models.Enums;

namespace NebulaMigration.ViewModels
{
    /// <summary>
    /// Category view model.
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Work shop type.
        /// </summary>
        public WorkshopType WorkshopType { get; set; }
    }
}