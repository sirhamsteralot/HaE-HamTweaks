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

        public static void ApplyPatch()
        {
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.RenderTweaks");

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
        }
    }
}
