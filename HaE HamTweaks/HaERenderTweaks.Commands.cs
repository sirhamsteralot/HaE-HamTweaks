using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEPluginCore;
using HaEPluginCore.Console;
using VRageRender;

namespace HaE_HamTweaks
{
    public partial class HaERenderTweaks
    {
        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("ChangeMaxFPS", "Changes max FPS, Usage: ChangeMaxFPS {fps}", ChangeMaxFPS));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetDirtBloomRatio", "Sets Lens Dirt/Bloom ratio, Usage: SetLensDirt {ratio}", SetDirtBloomRatio));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetBloomMultiplier", "Sets Bloom multiplier, Usage: SetBloomMultiplier {multiplier}", SetBloomMultiplier));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetChromaticFactor", "Sets Chromatic factor, Usage: SetChromaticFactor {factor}", SetChromaticFactor));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetBlockEdges", "Sets Block Edges, Usage: SetBlockEdges {true/false}", SetBlockEdges));
        }

        public string SetBlockEdges(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            bool blockEdges;
            if (!bool.TryParse(args[0], out blockEdges))
                return $"Could not parse ${args[0]} into bool!";

            HaEHamTweaks.config.enableBlockEdges = blockEdges;
            SetBlockEdges(blockEdges);
            HaEHamTweaks.Save();
            return $"Block Edges changed to: {blockEdges}\nThis requires a world reload to apply on existing blocks!";
        }

        public string ChangeMaxFPS(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            float newFpsVal;
            if (!float.TryParse(args[0], out newFpsVal))
                return $"Could not parse ${args[0]} into float!";

            HaEHamTweaks.config.maxFPS = newFpsVal;
            SetMaxFPS(newFpsVal);
            HaEHamTweaks.Save();
            return $"MaxFPS changed to: {newFpsVal}";
        }

        public string SetDirtBloomRatio(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            float newDirtVal;
            if (!float.TryParse(args[0], out newDirtVal))
                return $"Could not parse ${args[0]} into float!";

            HaEHamTweaks.config.lensDirtBloomRatio = newDirtVal;

            SetLensDirtRatio(newDirtVal);

            HaEHamTweaks.Save();
            return $"LensDirtRatio set to: {newDirtVal}";
        }

        public string SetBloomMultiplier(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            float bloomMultiplier;
            if (!float.TryParse(args[0], out bloomMultiplier))
                return $"Could not parse ${args[0]} into float!";

            HaEHamTweaks.config.bloomMultiplier = bloomMultiplier;

            SetBloomMult(bloomMultiplier);

            HaEHamTweaks.Save();
            return $"BloomMultiplier set to: {bloomMultiplier}";
        }

        public string SetChromaticFactor(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            float chromaticFactor;
            if (!float.TryParse(args[0], out chromaticFactor))
                return $"Could not parse ${args[0]} into float!";

            HaEHamTweaks.config.chromaticFactor = chromaticFactor;

            SetChromaticFactor(chromaticFactor);

            HaEHamTweaks.Save();
            return $"Chromatic factor set to: {chromaticFactor}";
        }
    }
}
