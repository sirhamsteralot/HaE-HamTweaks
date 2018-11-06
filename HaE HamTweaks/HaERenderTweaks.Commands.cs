using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEPluginCore;
using HaEPluginCore.Console;

namespace HaE_HamTweaks
{
    public partial class HaERenderTweaks
    {
        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("ChangeMaxFPS", "Changes max FPS", ChangeMaxFPS));
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
    }
}
