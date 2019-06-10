using System;
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
using Sandbox.Game.Multiplayer;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Engine;
using Sandbox.Engine.Utils;
using ParallelTasks;
using HaEPluginCore.Console;
using Sandbox.Game.Screens.Helpers;
using HaEHamTweaks.Patching;

namespace HaEHamTweaks
{
    public partial class HaEUXTweaks
    {
        private MyGridClipboard Clipboard => MyClipboardComponent.Static.Clipboard;
        List<IMyGps> gpsListCache;

        public HaEUXTweaks()
        {
            OnInit();
            RegisterCommands();
        }

        public void OnInit()
        {
            MySession.OnLoading += MySession_OnLoading;
            UXTweakPatches.ApplyPatch();
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
        public int SetGPSOverride(string tag, bool setting, TagFilter filter)
        {
            int amountChanged = 0;

            if (gpsListCache == null)
                gpsListCache = new List<IMyGps>();
            var gpsCollection = MySession.Static.Gpss;
            var myId = MySession.Static.LocalPlayerId;
            gpsCollection.GetGpsList(myId, gpsListCache);

            foreach(var gps in gpsListCache)
            {
                switch(filter)
                {
                    case TagFilter.name:
                        if (gps.Name.Contains(tag))
                        {
                            gps.ShowOnHud = setting;
                            gpsCollection.SendModifyGps(myId, (MyGps)gps);
                        }
                        break;
                    case TagFilter.description:
                        if (gps.Description.Contains(tag))
                        {
                            gps.ShowOnHud = setting;
                            gpsCollection.SendModifyGps(myId, (MyGps)gps);
                        }
                        break;
                }
            }

            gpsCollection.updateForHud();

            gpsListCache.Clear();
            return amountChanged;
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

        public void UpdateProjectorProjections(List<MyTuple<IMyCubeGrid, IMyProjector>> projectedGrids)
        {
            foreach (var tuple in projectedGrids)
            {
                if (tuple.Item1 == null)
                    continue;

                SetProjectedGrid((MyObjectBuilder_CubeGrid)tuple.Item1.GetObjectBuilder(), (MyProjectorBase)tuple.Item2);
            }
        }


        private MethodInfo sendNewBlueprint = typeof(MyProjectorBase).GetMethod("SendNewBlueprint", BindingFlags.Instance | BindingFlags.NonPublic);
        private FieldInfo originalBuilder = typeof(MyProjectorBase).GetField("m_originalGridBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
        public void SetProjectedGrid(MyObjectBuilder_CubeGrid grid, MyProjectorBase projector)
        {
            //if (grid == null)
            //    return;

            //MyEntities.RemapObjectBuilder(grid);
            //originalBuilder.SetValue(projector, grid);
            //sendNewBlueprint.Invoke(projector, new object[] { grid });

            ((IMyProjector)projector).SetProjectedGrid(grid);
        }

        public int SetProjectorProjections(string blueprintName, string ProjectorTag, IMyCubeGrid grid)
        {
            int projectionsChangedCount = 0;

            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            List<IMySlimBlock> temp = new List<IMySlimBlock>();
            grid.GetBlocks(blocks);

            for (int i = 0; i < blocks.Count; i++)
            {
                var projector = blocks[i].FatBlock as IMyProjector;

                if (projector == null || !projector.CustomName.Contains(ProjectorTag))
                    continue;

                if (projector.LoadBlueprint(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, blueprintName, "bp.sbc")))
                    projectionsChangedCount++;
                else
                    HaEConsole.WriteLine($"ERR: {projector.CustomName} COULDNT UPDATE PROJECTION!, check blueprint name!");
            }

            return projectionsChangedCount;
        }
        #endregion
        #region enums
        public enum TagFilter
        {
            name,
            description
        }
        #endregion
    }
}
