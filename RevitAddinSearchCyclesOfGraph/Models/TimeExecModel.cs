namespace RevitAddinSearchCyclesOfGraph.Models;

public class TimeExecModel
{
    public int VertexCount { get; set; }
    public long CreationGraphsTime { get; set; }
    public long FindCyclesTime { get; set; }
}