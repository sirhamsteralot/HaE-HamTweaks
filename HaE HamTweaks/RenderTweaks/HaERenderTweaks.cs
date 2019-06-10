using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Plugins;
using VRage.Filesystem;
using VRage.FileSystem;
using VRageRender;
using VRageRender.ExternalApp;
using HaEPluginCore;
using HaEPluginCore.Console;
using HaEHamTweaks.Patching;

namespace HaE_HamTweaks
{
    public partial class HaERenderTweaks
    {
        public MyPostprocessSettings preTamperSettings; 

        public HaERenderTweaks()
        {
            OnInit();
        }

        public void OnInit()
        {
            RegisterCommands();
            preTamperSettings = MyPostprocessSettingsWrapper.Settings;

            SetMaxFPS(HaEHamTweaks.config.maxFPS);

            MySession.OnLoading += MySession_OnLoading;

            ApplyLightingPatch();
        }

        private void MySession_OnLoading()
        {
            SetLensDirtRatio(HaEHamTweaks.config.lensDirtBloomRatio);
            SetBloomMult(HaEHamTweaks.config.bloomMultiplier);
            SetChromaticFactor(HaEHamTweaks.config.chromaticFactor);
            SetBlockEdges(HaEHamTweaks.config.enableBlockEdges);
        }

        public void OnUpdate()
        {

        }

        public void OnDispose()
        {

        }

        #region methods
        public void ApplyLightingPatch()
        {
            if (!HaEHamTweaks.config.lightingPatch)
            {
                HaEConsole.WriteLine("lighting patch disabled.");
                return;
            }
            

            RendertweakPatches.ApplyPatch();

            string filePath = MyFileSystem.ContentPath + "\\Shaders\\Lighting\\LightDefs.hlsli";
            StreamReader reader = new StreamReader(filePath);
            string input = reader.ReadToEnd();
            reader.Close();

            if (input.Contains($"#define MAX_TILE_LIGHTS 256"))
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    {
                        string output = input.Replace("#define MAX_TILE_LIGHTS 256", $"#define MAX_TILE_LIGHTS {RendertweakPatches.pointlightCount}");
                        writer.Write(output);
                    }
                    writer.Close();
                }
                
                try
                {
                    Directory.Delete(MyFileSystem.UserDataPath + "\\ShaderCache2", true);
                } catch(DirectoryNotFoundException e)
                {
                    HaEConsole.WriteLine("Could not find shadercache!");
                }
                

                MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO,
                    new StringBuilder("Please restart SE for lighting change to take effect!"),
                    new StringBuilder("HaE HamTweaks:"),
                    null, null, null, null, new Action<MyGuiScreenMessageBox.ResultEnum>(ExitCallback)));
            }
        }
        public void ExitCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
        {
            if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
            {
                MyScreenManager.CloseAllScreensNowExcept(null);
                MySandboxGame.ExitThreadSafe();
            }
        }

        public void SetMaxFPS(float maxFrameRate)
        {
            MyRenderThread renderThread = MySandboxGame.Static.GameRenderComponent.RenderThread;
            FieldInfo field = renderThread.GetType().GetField("m_waiter", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo field2 = renderThread.GetType().GetField("m_timer", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(renderThread, new WaitForTargetFrameRate((MyGameTimer)field2.GetValue(renderThread), maxFrameRate));
        }

        public void SetLensDirtRatio(float ratio)
        {
            MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = ratio;
        }

        public void SetBloomMult(float mult)
        {
            MyPostprocessSettingsWrapper.Settings.Data.BloomMult = mult;
        }

        public void SetChromaticFactor(float factor)
        {
            MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor = factor;
        }

        public void SetBlockEdges(bool edges)
        {
            MyFakes.ENABLE_EDGES = edges;
        }

        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
