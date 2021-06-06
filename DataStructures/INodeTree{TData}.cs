
namespace DataStructures
{

    /// <summary>
    /// Extends the <seealso cref="INodeParent{TNode}"/> by a <see cref="IData{TData}.Value"/> property.
    /// </summary>
    /// <typeparam name="TData">The type of the data which should be used for <see cref="IData{TData}.Value"/> </typeparam>
    public interface INodeTree<TData> : IData<TData>, INodeParent<INodeTree<TData>>
    {

    }
}
