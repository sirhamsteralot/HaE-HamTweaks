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
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("ChangeMaxFPS", "Changes max FPS", ChangeMaxFPS));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetLensDirt", "Sets Lens Dirt, Usage: SetLensDirt {true/false}", SetLensDirt));
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

        public string SetLensDirt(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            bool newDirtVal;
            if (!bool.TryParse(args[0], out newDirtVal))
                return $"Could not parse ${args[0]} into bool!";

            HaEHamTweaks.config.disableLensDirt = !newDirtVal;

            SetLensDirtTexture(!newDirtVal);

            HaEHamTweaks.Save();
            return $"DisableLensDirt changed to: {!newDirtVal}";
        }
    }
}
