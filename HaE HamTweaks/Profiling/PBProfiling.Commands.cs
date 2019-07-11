using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEPluginCore.Console;
using HaEHamTweaks.Patching;

namespace HaEHamTweaks.Profiling
{
    public partial class PBProfiling
    {
        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetPBProfiling", "Enables/Disables the PB profiling, Usage: SetPBProfiling {bool}", SetPBProfiling));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("GetPBRuntime", "Returns a list of profiled pbs, Usage: GetPBRuntime {search}", GetPBRuntime));

        }

        #region commands
        public string GetPBRuntime(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            StringBuilder sb = new StringBuilder();
            List<PBTracker> trackers = new List<PBTracker>();

            PBData.GetTrackerNameContains(args[0], trackers);

            sb.AppendLine($"Results for {args[0]}:");

            foreach (var tracker in trackers)
            {
                sb.Append("PB: ").Append(tracker.PBID).Append(" Average: ").Append(tracker.AverageMS.ToString()).AppendLine(" ms.");
            }

            return sb.ToString();
        }

        public string SetPBProfiling(List<string> args)
        {
            if (args.Count < 1)
                return "Not Enough args!";

            bool x;
            if (!bool.TryParse(args[0], out x))
                return $"Could not parse ${args[0]} into bool!";

            profilingEnabled = x;
            ProfilerPatches.EnablePatch = x;

            return $"Set Profiling: {args[0]}";
        }
        #endregion
    }
}
