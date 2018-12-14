using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace HaEHamTweaks.Patching
{
    public class RendertweakPatches
    {
        private static Type myBillboardRenderer = typeof(MyAtmosphereRenderer).Assembly.GetType("VRageRender.MyBillboardRenderer");
        private static Type myLightsRendering = typeof(MyAtmosphereRenderer).Assembly.GetType("VRage.Render11.LightingStage.MyLightsRendering");

        private static Type MyPointlightConstants = typeof(MyAtmosphereRenderer).Assembly.GetType("VRage.Render11.LightingStage.MyPointlightConstants");

        private const int pointlightCount = 1024;
        private const int spotlightCount = 256;

        public static void ApplyPatch()
        {
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.RenderTweaks");
            BillboardRendererPatch(harmony);
            /// MyLightsRenderingPatch(harmony); Im gonna leave this one up to eq
        }

        public static void MyLightsRenderingPatch(HarmonyInstance harmony)
        {
            HaEConsole.WriteLine($"Patching MyLightsRendering...");
            MethodInfo[] methods = myLightsRendering.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.GetMethodBody() == null)
                    continue;

                harmony.Patch(method, null, null, new HarmonyMethod(typeof(Patch).GetMethod("LightsRenderingTranspiler", BindingFlags.Static | BindingFlags.Public)));
                HaEConsole.WriteLine($"Patched method: {method.Name}");
            }

            object pointlightscullbuffer = Array.CreateInstance(MyPointlightConstants, pointlightCount);

            myLightsRendering.GetField("m_pointlightsCullBuffer", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, pointlightscullbuffer);
            myLightsRendering.GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }

        public static void BillboardRendererPatch(HarmonyInstance harmony)
        {
            HaEConsole.WriteLine($"Patching BillboardRenderer...");
            MethodInfo[] methods = myBillboardRenderer.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.GetMethodBody() == null)
                    continue;

                harmony.Patch(method, null, null, new HarmonyMethod(typeof(Patch).GetMethod("BillboardTranspiler", BindingFlags.Static | BindingFlags.Public)));
                HaEConsole.WriteLine($"Patched method: {method.Name}");
            }
        }

        public class Patch
        {
            public static IEnumerable<CodeInstruction> BillboardTranspiler(IEnumerable<CodeInstruction> ip)
            {
                foreach (var i in ip)
                {
                    if (i.opcode == OpCodes.Ldc_I4 && i.operand is int intVal && (intVal % 32768) == 0)
                    {
                        var nw = new CodeInstruction(i);
                        nw.opcode = OpCodes.Ldc_I4;
                        nw.operand = (int)(intVal / 32768) * 131072;
                        yield return nw;
                    }
                    else
                        yield return i;
                }
            }

            public static IEnumerable<CodeInstruction> LightsRenderingTranspiler(IEnumerable<CodeInstruction> ip)
            {
                foreach (var i in ip)
                {
                    if (i.opcode == OpCodes.Ldc_I4 && i.operand is int intVal && intVal == 32)
                    {
                        var nw = new CodeInstruction(i);
                        nw.opcode = OpCodes.Ldc_I4;
                        nw.operand = spotlightCount;
                        yield return nw;
                    }
                    else if (i.opcode == OpCodes.Ldc_I4 && i.operand is int intVals && intVals == 256)
                    {
                        var nw = new CodeInstruction(i);
                        nw.opcode = OpCodes.Ldc_I4;
                        nw.operand = pointlightCount;
                        yield return nw;
                    }
                    else
                        yield return i;
                }
            }
        }
    }
}
