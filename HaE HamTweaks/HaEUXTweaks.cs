﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VRage;
using VRage.Input;
using VRage.FileSystem;
using VRage.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using Sandbox;
using Sandbox.ModAPI;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using HaEPluginCore.Console;

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

        public List<IMyCubeGrid> GetGridGroupWithName(string name)
        {
            HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities, x => x.DisplayName == name);

            IMyCubeGrid cubegrid = null;
            foreach (var entity in entities)
                cubegrid = entity as IMyCubeGrid;

            if (cubegrid == null)
                return null;

            return MyAPIGateway.GridGroups.GetGroup(cubegrid, GridLinkTypeEnum.Logical);
        }

        public int SetPBScripts(string scriptName, string PBTag, IMyCubeGrid grid)
        {
            int scriptsChangedCount = 0;

            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            List<IMySlimBlock> temp = new List<IMySlimBlock>();
            grid.GetBlocks(blocks);

            for (int i = 0; i < blocks.Count; i++)
            {
                var myProgrammable = blocks[i].FatBlock as IMyProgrammableBlock;

                if (myProgrammable == null || !myProgrammable.CustomName.Contains(PBTag))
                    continue;


                string program = "";
                try
                {
                    program = File.ReadAllText(Path.Combine(new string[]
                    {
                    MyFileSystem.UserDataPath,
                    "IngameScripts",
                    "local",
                    scriptName,
                    "script.cs"
                    }));
                } catch (FileNotFoundException e)
                {
                    try
                    {
                        program = File.ReadAllText(Path.Combine(new string[]
                        {
                            MyFileSystem.UserDataPath,
                            "IngameScripts",
                            "local",
                            scriptName,
                            "Script.cs"
                        }));
                    } catch (FileNotFoundException f)
                    {
                        continue;
                    }
                } catch (DirectoryNotFoundException e)
                {
                    continue;
                }

                if (program == "")
                    continue;

                myProgrammable.ProgramData = program;
                scriptsChangedCount++;
            }

            return scriptsChangedCount;
        }
        #endregion
    }
}
