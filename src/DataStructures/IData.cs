namespace DataStructures
{
    /// <summary>
    /// Provides a property to store data
    /// </summary>
    /// <typeparam name="TData">The Type of the data which should be stored</typeparam>
    public interface IData<TData>
    {
        /// <summary>
        /// Get or sets the Data
        /// </summary>
        TData Value { get; set; }
    }
}
