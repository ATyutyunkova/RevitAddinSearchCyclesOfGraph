using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using RevitAddinSearchCyclesOfGraph.Models;
using RevitAddinSearchCyclesOfGraph.Service;

namespace RevitAddinSearchCyclesOfGraph.Commands;

[Transaction(TransactionMode.Manual)]
public class MainCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            long timeExecOfCreatingGraph = 0;
            long timeExecOfFindCycles = 0;
            long timeExecOfDeserializeObject = 0;

            var watchExecOfCreatingGraph = Stopwatch.StartNew();
            var graphDto = CGraphService.CreateGraphDto(1000001);
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
            return Result.Succeeded;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
            throw;
        }
    }
}