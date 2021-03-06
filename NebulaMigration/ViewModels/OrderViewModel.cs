﻿using NebulaMigration.Models;
using System;
using System.Collections.Generic;

namespace NebulaMigration.ViewModels
{
    /// <summary>
    /// Order view model.
    /// </summary>
    public class OrderViewModel
    {
        /// <summary>
         /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public int TableNumber { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Количество готовых блюд для вывода бэйджа над столом.
        /// </summary>
        public int ReadyDishesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is export requested.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is export requested; otherwise, <c>false</c>.
        /// </value>
        public bool IsExportRequested { get; set; }
    }
}