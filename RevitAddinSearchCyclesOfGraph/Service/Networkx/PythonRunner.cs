using System.IO;
using System.Text;

namespace RevitAddinSearchCyclesOfGraph.Service.Networkx;

public static class PythonRunner
{
    public static string FindCyclesOfGraph(string graphDto)
    {
        var path = WriteGraphToFile(graphDto);

        var pythonPath =
            @"C:\Users\tyutyunkova\source\repos rider\RevitAddinSearchCyclesOfGraph\RevitAddinSearchCyclesOfGraph\Resources\pyEnv\python.exe";
        var pathScript = string.Format(@"C:\Users\tyutyunkova\Desktop\testFromConsoleWithArgs.py {0}",
            path);

        using (var cmd = new CommandLineProcess(pythonPath, pathScript))
        {
            var sb = new StringBuilder();
            // Call Python:
            var exitCode = cmd.Run(out string processOutput, out string processError);
            // Get result:
            sb.AppendLine(processOutput);
            sb.AppendLine(processError);
            return sb.ToString();
        }
    }

    // Запись в txt
    private static string WriteGraphToFile(string graphDto)
    {
        var path = @"C:\Users\tyutyunkova\Desktop\IDK\tempGraphFolder\graph.txt";
        using (StreamWriter writer =
               new StreamWriter(path))
        {
            writer.WriteLine(graphDto);
        }

        return path;
    }
}