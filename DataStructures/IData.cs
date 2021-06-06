﻿namespace DataStructures
{
    /// <summary>
    /// Get or sets the Data
    /// </summary>
    /// <typeparam name="TData">The Type of the Data which should be used in the DataStructure</typeparam>
    public interface IData<TData>
    {
        /// <summary>
        /// Get or sets the Data
        /// </summary>
        TData Value { get; set; }
    }
}
