using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using VRageRender;
using HaEPluginCore.Console;
using VRageMath;
using Sandbox.Game.Entities.Blocks;
using HaEHamTweaks;

namespace HaEHamTweaks.Patching
{
    public class ProfilerPatches
    {
        public static Action<MyProgrammableBlock, double> PBRan;
        public static bool EnablePatch = false;

        public static void ApplyPatch()
        {
            HaEConsole.WriteLine($"Patching ProfilerPatches...");
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.Profiler");
            PBProfilingPatch(harmony);
            HaEConsole.WriteLine($"ProfilerPatches: Patched 1 method");
        }

        public static void PBProfilingPatch(HarmonyInstance harmony)
        {
            harmony.Patch(typeof(MyProgrammableBlock).GetMethod("ExecuteCode", BindingFlags.Public | BindingFlags.Instance),
                null,
                new HarmonyMethod(typeof(ProfilerPatches).GetMethod("SuffixProfilePB", BindingFlags.NonPublic | BindingFlags.Static)));

            HaEConsole.WriteLine($"Patched MyProgrammableBlock.ExecuteCode");
        }

        static FieldInfo runtimeField = typeof(MyProgrammableBlock).GetField("m_runtime", BindingFlags.Instance | BindingFlags.NonPublic);
        static PropertyInfo lastruntimeMS = typeof(MyProgrammableBlock)
            .GetNestedType("RuntimeInfo", BindingFlags.NonPublic)
            .GetProperty("LastRunTimeMs", BindingFlags.Public | BindingFlags.Instance);
        private static void SuffixProfilePB(MyProgrammableBlock __instance)
        {
            if (!EnablePatch)
                return;

            double dtInMS = (double)lastruntimeMS.GetValue(runtimeField.GetValue(__instance));

            PBRan?.Invoke(__instance, dtInMS);
        }
    }
}
