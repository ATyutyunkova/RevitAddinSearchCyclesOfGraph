using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;
using RevitAddinSearchCyclesOfGraph.Models;

namespace RevitAddinSearchCyclesOfGraph.Service.Networkx;

public static class TestNetworkx
{

    public static void InitSearch1()
    {
        var step = 5000;

        var currentCountOfVertex = 0;
        var timeExecList = new List<TimeExecModel>();

        for (var i = 0; i < 10; i++)
        {
            currentCountOfVertex += step;
            var timeExec = FindCycles(currentCountOfVertex);
            timeExecList.Add(timeExec);
        }
        
        using (StreamWriter writer =
               new StreamWriter("C:\\Users\\tyutyunkova\\Desktop\\IDK\\tempGraphFolder\\cSharpLogs.txt"))
        {
            foreach (var timeExec in timeExecList)
            {
                writer.WriteLine("Количество векторов:" + timeExec.VertexCount);
                writer.WriteLine("Создание графов (мс): " + timeExec.CreationGraphsTime);
                writer.WriteLine("Поиск циклов (мс): " + timeExec.FindCyclesTime);
                writer.WriteLine("----------------------------------------------------");
            }
        }

        MessageBox.Show("END");
    }
    
    private static TimeExecModel FindCycles(int countV)
    {
        long timeExecOfCreatingGraph = 0;
        long timeExecOfFindCycles = 0;
        long timeExecOfDeserializeObject = 0;

        var watchExecOfCreatingGraph = Stopwatch.StartNew();
        var graphDto = CreateGraphDto(countV+1);
        watchExecOfCreatingGraph.Stop();
        timeExecOfCreatingGraph = watchExecOfCreatingGraph.ElapsedMilliseconds;

        var watchExecOfFindCycles = Stopwatch.StartNew();
        var result = PythonRunner.FindCyclesOfGraph(graphDto);
        watchExecOfFindCycles.Stop();
        timeExecOfFindCycles = watchExecOfFindCycles.ElapsedMilliseconds;

        var watchExecOfDeserializeObject = Stopwatch.StartNew();
        var deserializedResult = JsonConvert.DeserializeObject<FindedCycles>(result);
        watchExecOfDeserializeObject.Stop();
        timeExecOfDeserializeObject = watchExecOfDeserializeObject.ElapsedMilliseconds;


        MessageBox.Show("Кол-во циклов: " + deserializedResult.Cycles.Length + "\r\n" +
                        "Создание графа: " + timeExecOfCreatingGraph + "\r\n" + "Нахождение циклов: " +
                        timeExecOfFindCycles + "\r\n" + "Десериализация: " + timeExecOfDeserializeObject);
        
        return new TimeExecModel
        {
            CreationGraphsTime = timeExecOfCreatingGraph, FindCyclesTime = timeExecOfFindCycles,
            VertexCount = countV
        };
    }
    
    // 15milliseconds  - 1 миллион вершин
    public static string CreateGraphDto(int vCount)
    {
        var graph = new List<(int, int)>();
        for (var currentV = 0; currentV < vCount; currentV++)
        {
            if (currentV == 0) continue;
            var lastV = currentV - 1;
            graph.Add((lastV, currentV));
            graph.Add((currentV, 0));
        }

        var serializedGraph = JsonConvert.SerializeObject(graph);
        return serializedGraph;
    }
}