﻿namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;

    /// <summary>
    /// Category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// ExternalId.
        /// </summary>
        public int ExternalId { get; set; }
        
        /// <summary>
        /// IsActive.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Code.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Workshop type.
        /// </summary>
        public WorkshopType WorkshopType { get; set; }
    }
}