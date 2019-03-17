﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;
using Sandbox;
using Sandbox.ModAPI;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using HaEPluginCore;
using HaEPluginCore.Console;
using HaEHamTweaks.Patching;
using VRage.Input;

namespace HaE_HamTweaks
{
    public partial class HaEUXTweaks
    {

        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OverridePlacementLimits", "Overrides clipboard placement limits, Usage: OverridePlacementLimits {true/false}", OverridePlacementLimits));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetProgrammableBlockScripts", "Sets all PB scripts on a grid to a certain script, Usage: SetProgrammableBlockScripts {gridName} {pbNameTag} {scriptName}", SetProgrammableBlockScripts));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetProjectorProjections", "Sets all Projector projections on a grid to a certain blueprint, Usage: SetProjectorProjections {gridName} {projectorTag} {blueprintName}", SetProjectorProjections));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetSensitivity", "Sets mouse sensitivity, Usage: SetSensitivity {sensitivity float}", SetSensitivity));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetPaintOverride", "Sets paint allow override, Usage: SetSensitivity {bool}", SetPaintOverride));
        }


        #region commands
        public string SetPaintOverride(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            bool paintOverride;
            if (!bool.TryParse(args[0], out paintOverride))
                return $"Could not parse ${args[0]} into bool!";

            UXTweakPatches.AllowEveryonePaintOverride = paintOverride;

            return $"Set allow paint override: {paintOverride}";
        }

        public string SetSensitivity(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            float newSensitivity;
            if (!float.TryParse(args[0], out newSensitivity))
                return $"Could not parse ${args[0]} into float!";

            MyInput.Static.SetMouseSensitivity(newSensitivity);
            MySandboxGame.Config.Save();

            return $"Set mouse sensitivity: {newSensitivity}";
        }

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

        public string SetProjectorProjections(List<string> args)
        {
            if (args.Count < 3)
                return "Not Enough args!";

            string maingridName = args[0];
            string projectortag = args[1];
            string blueprintName = args[2];

            var grids = GetGridGroupWithName(maingridName);

            if (grids == null || grids.Count == 0)
                return $"Could not find cubegrid with name {maingridName}";

            int changed = 0;

            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            List<IMyProjector> projectors = new List<IMyProjector>();
            for (int i = 0; i < grids.Count; i++)
            {
                blocks.Clear();
                grids[i].GetBlocks(blocks);

                for (int j = 0; j < blocks.Count; j++)
                {
                    var projector = blocks[j].FatBlock as IMyProjector;
                    if (projector != null)
                    {
                        projectors.Add(projector);
                        continue;
                    }
                }
            }

            foreach (var projector in projectors)
            {
                if (projector.ProjectedGrid != null)
                    grids.Add(projector.ProjectedGrid);
            }

            foreach (var grid in grids)
            {
                changed += SetProjectorProjections(blueprintName, projectortag, grid);
            }

            return $"Success!, on {grids.Count} grids: {changed} projections changed.";
        }

        public string SetProgrammableBlockScripts(List<string> args)
        {
            if (args.Count < 3)
                return "Not Enough args!";

            string maingridName = args[0];
            string pbNameTag = args[1];
            string scriptName = args[2];

            var grids = GetGridGroupWithName(maingridName);

            if (grids == null || grids.Count == 0)
                return $"Could not find cubegrid with name {maingridName}";

            int changed = 0;

            List<MyTuple<IMyCubeGrid, IMyProjector>> projectorGridCombos = new List<MyTuple<IMyCubeGrid, IMyProjector>>();
            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            for(int i = 0; i < grids.Count; i++)
            {
                blocks.Clear();
                grids[i].GetBlocks(blocks);

                for (int j = 0; j < blocks.Count; j++)
                {
                    var projector = blocks[j].FatBlock as IMyProjector;
                    if (projector != null)
                    {
                        if (projector.ProjectedGrid != null)
                        {
                            projectorGridCombos.Add(new MyTuple<IMyCubeGrid, IMyProjector>(projector.ProjectedGrid, projector));
                        }

                        continue;
                    }
                }
            }

            foreach (var tuple in projectorGridCombos)
            {
                grids.Add(tuple.Item1);
            }

            foreach(var grid in grids)
            {
                changed += SetPBScripts(scriptName, pbNameTag, grid);
            }

            UpdateProjectorProjections(projectorGridCombos);

            return $"Success!, on {grids.Count} grids: {changed} scripts changed."; 
        }
        #endregion
    }
}
