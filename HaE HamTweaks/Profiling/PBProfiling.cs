using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEHamTweaks.Patching;

namespace HaEHamTweaks.Profiling
{
    public partial class PBProfiling
    {
        public static double tickSignificance = 0.005;
        public static bool profilingEnabled = false;

        public PBProfiling()
        {
            ProfilerPatches.ApplyPatch();
            PBData.Init();
            RegisterCommands();
        }
    }
}
