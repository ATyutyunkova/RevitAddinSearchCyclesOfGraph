using System.Windows.Forms;
using RevitAddinSearchCyclesOfGraph.Models;
using RevitAddinSearchCyclesOfGraph.Utils;
using StronglyConnectedComponents.Core;

namespace RevitAddinSearchCyclesOfGraph.Service;

public static class GraphService
{
    private static RevitStatusBar _statusBar = null;

    public static void InitSearch()
    {
        var step = 1000;
        var currentCountOfVertex = 0;
        var timeExecList = new List<TimeExecModel>();

        for (var i = 0; i < 10; i++)
        {
            currentCountOfVertex += step;
            var timeExec = FindCyclesOfGraph(currentCountOfVertex);
            timeExecList.Add(timeExec);
        }

        using (StreamWriter writer =
               new StreamWriter("C:\\Users\\tyutyunkova\\Desktop\\IDK\\TEMP WORK GRAPH FOLDER\\timeexectest.txt"))
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

    private static TimeExecModel FindCyclesOfGraph(int vCount)
    {
        _statusBar = RevitStatusBar.Create();
        long timeExecOfCreatingGraph = 0;
        long timeExecOfFindCycles = 0;

        var listOfGraph = new List<VertexList<int>>();
        var prevGraph = new VertexList<int>();
        var watchExecOfCreatingGraph = System.Diagnostics.Stopwatch.StartNew();
        for (var i = 1; i < vCount; i++)
        {
            var tempGraph = new VertexList<int>();
            var vertex = new Vertex<int>(i);
            if (i > 1)
            {
                tempGraph = new VertexList<int>(prevGraph);
            }

            var firstVertex = new Vertex<int>(0);
            if (prevGraph.Count > 0)
            {
                firstVertex = new Vertex<int>(prevGraph.FirstOrDefault(x => x.Value == 0));
            }
            else
            {
                tempGraph.Add(firstVertex);
            }

            vertex.Dependencies.Add(firstVertex);
            firstVertex.Dependencies.Add(vertex);

            if (i > 1)
            {
                var oldPrevVertex = tempGraph.FirstOrDefault(x => x.Value == i - 1);
                var prevVertex = new Vertex<int>(oldPrevVertex);
                vertex.Dependencies.Add(prevVertex);
                prevVertex.Dependencies.Add(vertex);
                var index = tempGraph.IndexOf(oldPrevVertex);
                tempGraph[index] = prevVertex;
            }

            tempGraph.Add(vertex);
            prevGraph = new VertexList<int>(tempGraph);
            listOfGraph.Add(tempGraph);
            _statusBar.Set("Создано графов: " + i + " из " + vCount);
        }

        _statusBar.Set("Done creating graph");

        watchExecOfCreatingGraph.Stop();
        timeExecOfCreatingGraph = watchExecOfCreatingGraph.ElapsedMilliseconds;

        var watchExecOfFindCycles = System.Diagnostics.Stopwatch.StartNew();
        var countOfCycle = 0;
        for (var i = 0; i < listOfGraph.Count; i++)
        {
            var graph1 = listOfGraph[i];
            var detector = new StronglyConnectedComponentFinder<int>();
            var components = detector.DetectCycle(graph1);
            if (components.Cycles().Count() == 1)
            {
                countOfCycle++;
            }

            _statusBar.Set("Обработано графов: " + i + "из " + listOfGraph.Count);
        }

        watchExecOfFindCycles.Stop();
        timeExecOfFindCycles = watchExecOfFindCycles.ElapsedMilliseconds;

        _statusBar.Set("Done");
        return new TimeExecModel
            {CreationGraphsTime = timeExecOfCreatingGraph, FindCyclesTime = timeExecOfFindCycles, VertexCount = vCount};
    }
}