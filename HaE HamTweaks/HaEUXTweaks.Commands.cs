using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using Sandbox;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using HaEPluginCore;
using HaEPluginCore.Console;

namespace HaE_HamTweaks
{
    public partial class HaEUXTweaks
    {

        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OverridePlacementLimits", "Overrides clipboard placement limits, Usage: OverridePlacementLimits {true/false}", OverridePlacementLimits));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetProgrammableBlockScripts", "Sets all PB scripts on a grid to a certain script, Usage: SetProgrammableBlockScripts {gridName} {pbNameTag} {scriptName}", SetProgrammableBlockScripts));

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
            return $"Set override: {placementTest}";
        }

        public string SetProgrammableBlockScripts(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            string maingridName = args[0];
            string pbNameTag = args[1];
            string scriptName = args[2];

            var grids = GetGridGroupWithName(maingridName);

            if (grids == null || grids.Count == 0)
                return $"Could not find cubegrid with name {maingridName}";

            int changed = 0;

            foreach(var grid in grids)
            {
                changed += SetPBScripts(scriptName, pbNameTag, grid);
            }

            return $"Success!, on {grids.Count} grids: {changed} scripts changed."; 
        }
        #endregion
    }
}
