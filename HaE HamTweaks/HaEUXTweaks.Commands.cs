using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using Sandbox;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using HaEPluginCore;
using HaEPluginCore.Console;

namespace HaE_HamTweaks
{
    public partial class HaEUXTweaks
    {

        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OverridePlacementLimits", "Overrides clipboard placement limits, Usage: OverridePlacementLimits {true/false}", OverridePlacementLimits));
        }


        #region commands
        public string OverridePlacementLimits(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            bool placementTest;
            if (!bool.TryParse(args[0], out placementTest))
                return $"Could not parse ${args[0]} into bool!";

            MyFakes.DISABLE_CLIPBOARD_PLACEMENT_TEST = placementTest;
            return $"Set placement test: {placementTest}";
        }
        #endregion
    }
}
