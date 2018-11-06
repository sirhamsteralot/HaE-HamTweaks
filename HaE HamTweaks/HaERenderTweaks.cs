using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Sandbox;
using VRage.Library.Utils;
using VRage.Plugins;
using VRageRender.ExternalApp;

namespace HaE_HamTweaks
{
    public partial class HaERenderTweaks
    {
        public HaERenderTweaks()
        {
            OnInit();
        }

        public void OnInit()
        {
            RegisterCommands();
            SetMaxFPS(HaEHamTweaks.config.maxFPS);
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
        #endregion
    }
}
