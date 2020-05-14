
namespace DataStructures
{
    public interface INodeTree<TData> : IData<TData>, INodeParent<INodeTree<TData>> 
    {

    }
}
