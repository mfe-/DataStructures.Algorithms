
namespace Get.the.Solution.DataStructure
{
    public interface ITreeNode<T> : INode<T>
    {
        ITreeNode<T> Parent { get; set; }
        new ITreeNode<T> Left { get; set; }
        new ITreeNode<T> Right { get; set; }
    }
}
