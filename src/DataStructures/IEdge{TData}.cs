namespace DataStructures
{
    /// <summary>
    /// Extends the <see cref="IEdge"/> by a <see cref="IData{TData}.Value"/> property.
    /// </summary>
    /// <typeparam name="TData">The type of the <seealso cref="IData{TData}.Value"/> property</typeparam>
    public interface IEdge<TData> : IEdge, IData<TData>
    {

    }
}
