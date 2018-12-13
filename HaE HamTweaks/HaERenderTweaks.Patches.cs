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

namespace HaE_HamTweaks
{
    public partial class HaERenderTweaks
    {
        private static Type myBillboardRenderer = typeof(MyAtmosphereRenderer).Assembly.GetType("VRageRender.MyBillboardRenderer");

        public static void ApplyPatch()
        {
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.RenderTweaks");

            MethodInfo[] methods = myBillboardRenderer.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                harmony.Patch(method, null, null, new HarmonyMethod(typeof(Patch).GetMethod("BillboardTranspiler", BindingFlags.Static | BindingFlags.NonPublic)));
                HaEConsole.WriteLine($"Patched method: {method.Name}");
            }
        }
        
        private class Patch
        {
            private static IEnumerable<CodeInstruction> BillboardTranspiler(IEnumerable<CodeInstruction> ip)
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
        }

    }
}
