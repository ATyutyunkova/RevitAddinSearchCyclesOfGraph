using Newtonsoft.Json;

namespace RevitAddinSearchCyclesOfGraph.Models;

public class FindedCycles
{
    [JsonProperty("cycles")]
    public int[][] Cycles { get; set; }
}