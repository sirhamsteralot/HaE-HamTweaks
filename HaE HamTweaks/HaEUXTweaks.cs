using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Input;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using Sandbox;
using Sandbox.ModAPI;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Engine;
using Sandbox.Engine.Utils;

namespace HaE_HamTweaks
{
    public partial class HaEUXTweaks
    {
        private MyGridClipboard Clipboard => MyClipboardComponent.Static.Clipboard;

        public HaEUXTweaks()
        {
            OnInit();
            RegisterCommands();
        }

        public void OnInit()
        {
            MySession.OnLoading += MySession_OnLoading;
        }

        public void OnUpdate()
        {

        }

        public void OnDispose()
        {

        }

        private void MySession_OnLoading()
        {
            EnableDevKeys();
        }

        #region methods
        public void SetSnapModeOverride(SnapMode mode)
        {
            throw new NotImplementedException();
        }

        
        public void EnableDevKeys()
        {
            MyDirectXInput input = (MyDirectXInput)MyAPIGateway.Input;
            if (input == null)
                throw new Exception("Input null!");

            PropertyInfo property = input.GetType().GetProperty("ENABLE_DEVELOPER_KEYS", BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
                throw new Exception("Property null!");

            property.GetSetMethod(true).Invoke(input, new object[] { true });
        }
        #endregion
    }
}
