namespace DataStructures
{
    public interface IEdge
    {
        IVertex U { get; set; }
        IVertex V { get; set; }
        int Weighted { get; set; }
    }
}
