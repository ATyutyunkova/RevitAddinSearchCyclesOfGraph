using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ExportToPilotBim.Services;

namespace ExportToPilotBim.Commands
{

    public class InitCommand : IExternalApplication
    {
        private readonly string _tabName = "ROFL";
        private readonly string _ribbonPanelName = "Export to Pilot-Bim";

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(_tabName);
            var ribbonPanel = application.CreateRibbonPanel(_tabName, _ribbonPanelName);
            var thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var button = new PushButtonData(_ribbonPanelName, _ribbonPanelName, thisAssemblyPath,
                "ExportToPilotBim.Commands.MainCommand");
            ribbonPanel.AddItem(button);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }
    }
}