﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Gui;
using VRage.Library.Utils;
using VRage.Plugins;
using VRageRender;
using VRageRender.ExternalApp;
using HaEPluginCore;

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

            MySandboxGame.Static.OnGameLoaded += Static_OnGameLoaded;
        }

        private void Static_OnGameLoaded(object sender, EventArgs e)
        {
            SetLensDirtTexture(HaEHamTweaks.config.disableLensDirt);
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

        public void SetLensDirtTexture(bool val)
        {
            if (val)
                SetLensDirtTexture(HaEConstants.pluginFolder + "\\" + HaEConstants.AssetFolder + "\\NoDirt.DDS");
            else
                SetLensDirtTexture(MyPostprocessSettings.Default.DirtTexture);
        }

        public void SetLensDirtTexture(string texturePath)
        {
            var settings = MyDefinitionManager.Static.EnvironmentDefinition.PostProcessSettings;

            settings.DirtTexture = texturePath;
            MyDefinitionManager.Static.EnvironmentDefinition.PostProcessSettings = settings;
            MyPostprocessSettingsWrapper.ReloadSettingsFrom(settings);
        }
        #endregion
    }
}
