﻿using ThreatsManager.Interfaces;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Status of the Work Item.
    /// </summary>
    public enum WorkItemStatus
    {
        /// <summary>
        /// The Task status is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The Task has been created.
        /// </summary>
        Created,
        /// <summary>
        /// The Task has been planned.
        /// </summary>
        Planned,
        /// <summary>
        /// The Task is in progress.
        /// </summary>
        [EnumLabel("In Progress")]
        InProgress,
        /// <summary>
        /// The Task has been completed.
        /// </summary>
        Done,
        /// <summary>
        /// The Task has been removed.
        /// </summary>
        Removed
    }
}