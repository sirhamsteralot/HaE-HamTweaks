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
using Sandbox.Game.Entities;
using Sandbox.Engine.Utils;
using VRage.Game.Entity;
using Sandbox.Game.Multiplayer;
using HaEHamTweaks;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System.Net;
using VRage.GameServices;

namespace HaEHamTweaks.Patching
{
    public class UXTweakPatches
    {
        #region Properties
        public static bool AllowEveryonePaintOverride { get; set; } = false;
        public static Dictionary<IPEndPoint, string> passwords { get; set; }
        #endregion

        private static MyGameServerItem _lastServerConnect;

        public static void ApplyPatch()
        {
            var harmony = HarmonyInstance.Create("com.HaE.HamTweaks.UXTweaks");

            HaEConsole.WriteLine($"Patching UXPatches...");

            IsSettingsExperimentalPatch(harmony);
            //ColorGridOrBlockRequestValidationPatch(harmony);

            HaEConsole.WriteLine($"UXPatches: Patched 2 methods");
        }

        public static void IsSettingsExperimentalPatch(HarmonyInstance harmony)
        {
            harmony.Patch(typeof(MySession).GetMethod("IsSettingsExperimental", BindingFlags.Public | BindingFlags.Instance),
                new HarmonyMethod(typeof(Patch).GetMethod("PrefixIsSettingsExperimental", BindingFlags.Public | BindingFlags.Static)));

            HaEConsole.WriteLine($"Patched MySession.IsSettingsExperimental");
        }

        public static void ColorGridOrBlockRequestValidationPatch(HarmonyInstance harmony)
        {
            harmony.Patch(typeof(MyCubeGrid).GetMethod("ColorGridOrBlockRequestValidation", BindingFlags.NonPublic | BindingFlags.Instance),
                new HarmonyMethod(typeof(Patch).GetMethod("PrefixColorGridOrBlockRequestValidation", BindingFlags.Public | BindingFlags.Static)));

            HaEConsole.WriteLine($"Patched MyCubeGrid.ColorGridOrBlockRequestValidation");
        }


        public class Patch
        {
            public static bool PrefixIsSettingsExperimental(ref bool __result)
            {
                __result = true;
                return false;
            }

            public static bool PrefixColorGridOrBlockRequestValidation(long player, MyCubeGrid __instance, ref bool __result)
            {
                if (AllowEveryonePaintOverride)
                {
                    __result = true;
                    return false;
                }

                if (player == 0L)
                {
                    __result = true;
                    return false;
                }
                if (!Sync.IsServer)
                {
                    __result = true;
                    return false;
                }
                if (__instance.BigOwners.Count == 0)
                {
                    __result = true;
                    return false;
                }
                foreach (long current in __instance.BigOwners)
                {

                    MyRelationsBetweenPlayers relation = MyPlayer.GetRelationsBetweenPlayers(current, player);
                    if (relation == MyRelationsBetweenPlayers.Self || 
                        relation == MyRelationsBetweenPlayers.Allies)
                    {
                        __result = true;
                        return false;
                    }
                }

                __result = false;
                return false;
            }

            public static void SuffixServerResponded(MyGameServerItem serverItem)
            {

            }

            //public static void SuffixCreateScreen(MyGuiScreenServerPassword __instance)
            //{
            //    MyGuiControlTextbox passwordTextBox = (MyGuiControlTextbox)typeof(MyGuiScreenServerPassword)
            //        .GetField("m_passwordTextbox", BindingFlags.NonPublic | BindingFlags.Instance)
            //        .GetValue(__instance);

            //    string pass = null;
            //    if (passwords.TryGetValue( _lastServerConnect.NetAdr, out pass))
            //    {
            //        passwordTextBox.Text = pass;
            //    }
            //}
        }
    }
}
