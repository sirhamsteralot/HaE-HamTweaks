using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using Sandbox;
using Sandbox.Game;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;

namespace HaE_HamTweaks
{
    public partial class HaEUXTweaks
    {
        private MyGridClipboard Clipboard => MyClipboardComponent.Static.Clipboard;

        public HaEUXTweaks()
        {
            OnInit();
        }

        public void OnInit()
        {
            RegisterCommands();
        }

        public void OnUpdate()
        {

        }

        public void OnDispose()
        {

        }


        #region methods
        public void SetSnapModeOverride(SnapMode mode)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
