using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Plugins;
using VRageRender;
using VRageRender.ExternalApp;
using HaEPluginCore;

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

            MySession.OnLoading += MySession_OnLoading; ;
        }

        private void MySession_OnLoading()
        {
            SetLensDirtRatio(HaEHamTweaks.config.lensDirtBloomRatio);
        }

        public void OnUpdate()
        {

        }

        public void OnDispose()
        {

        }

        #region methods
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
        #endregion
    }
}
