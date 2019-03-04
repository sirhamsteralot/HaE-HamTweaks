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
using Sandbox;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.Engine.Utils;

namespace HaEHamTweaks.Patching
{
    public class UXTweakPatches
    {
        public static void ApplyPatch()
        {
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.UXTweaks");

            IsSettingsExperimentalPatch(harmony);
        }

        public static void IsSettingsExperimentalPatch(HarmonyInstance harmony)
        {
            HaEConsole.WriteLine($"Patching UXPatches.MySession...");

            harmony.Patch(typeof(MySession).GetMethod("IsSettingsExperimental", BindingFlags.Public | BindingFlags.Instance),
                new HarmonyMethod(typeof(Patch).GetMethod("PrefixIsSettingsExperimental", BindingFlags.Public | BindingFlags.Static)));

            HaEConsole.WriteLine($"Patched IsSettingsExperimental");
        }

        public class Patch
        {
            public static bool PrefixIsSettingsExperimental(ref bool __result)
            {
                __result = true;
                return false;
            }
        }
    }
}
