using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEPluginCore;
using HaEPluginCore.Console;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;

namespace HaEHamTweaks.Managers
{
    public class TexturePackCommands
    {
        public static void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("SetTexturePack", "Swaps in texture pack, Usage: SetTexturePack {packName}", SetTexturePack));
        }

        private static string SetTexturePack(List<string> texturePack)
        {
            if (texturePack.Count < 1)
                return "Not enough arguments!";

            TexturePackManager.Instance.LoadTexturesFrom(texturePack[0]);

            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO,
                new StringBuilder("Please restart SE for texture changes to take effect!"),
                new StringBuilder("HaE HamTweaks:"),
                null, null, null, null, new Action<MyGuiScreenMessageBox.ResultEnum>(ExitCallback)));

            return "Pack Swapped!";
        }
        private static void ExitCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
        {
            if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
            {
                MyScreenManager.CloseAllScreensNowExcept(null);
                MySandboxGame.ExitThreadSafe();
            }
        }
    }
}
