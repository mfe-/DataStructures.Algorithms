namespace DataStructures
{
    /// <summary>
    /// Extends the <see cref="IVertex"/> by a <see cref="IData{TData}.Value"/> property.
    /// </summary>
    /// <typeparam name="TData">The type of the <seealso cref="IData{TData}.Value"/> property</typeparam>
    public interface IVertex<TData> : IVertex, IData<TData>
    {
    }
}
