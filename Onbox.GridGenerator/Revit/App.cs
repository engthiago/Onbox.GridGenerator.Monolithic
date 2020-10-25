using Onbox.Abstractions.V8;
using Onbox.Core.V8;
using Onbox.Revit.V8;
using Onbox.Revit.V8.Applications;
using Onbox.Mvc.Revit.V8;
using Onbox.Revit.V8.UI;
using Autodesk.Revit.UI;
using Onbox.GridGenerator.Revit.Commands.Availability;
using Onbox.GridGenerator.Revit.Commands;
using Onbox.GridGenerator.ContainerExtensions;

namespace Onbox.GridGenerator.Revit
{
    [ContainerProvider("8fe7986c-9cea-49ad-be3c-a2ca57a1bb2a")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons
            var br = ribbonManager.GetLineBreak();

            // Adds a Ribbon Panel to the Addins tab
            var addinPanelManager = ribbonManager.CreatePanel("Grid Generator");
            addinPanelManager.AddPushButton<GridGeneratorCommand, AvailableOnProject>($"Grid{br}Generator", "onbox_logo");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container
            container.AddOnboxCore();
            container.AddRevitMvc();

            container.AddUnitConverters();

            container.AddGridGenerator();
            container.AddViews();

            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}
