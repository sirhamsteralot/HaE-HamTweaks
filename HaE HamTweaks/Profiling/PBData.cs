using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEHamTweaks.Patching;
using System.Threading;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;

namespace HaEHamTweaks.Profiling
{
    public static class PBData
    {
        public static Dictionary<long, PBTracker> pbPair = new Dictionary<long, PBTracker>();

        public static void Init()
        {
            ProfilerPatches.PBRan += AddOrUpdatePair;
        }

        public static void AddOrUpdatePair(MyProgrammableBlock entity, double runtime)
        {
            if (pbPair.ContainsKey(entity.EntityId))
            {
                pbPair[entity.EntityId].UpdatePerformance(runtime);
            }
            else
            {
                lock (pbPair)
                {
                    pbPair[entity.EntityId] = new PBTracker(entity, runtime);
                }
            }
        }

        public static void GetTrackerNameContains(string Findstring, List<PBTracker> results)
        {
            foreach(var pb in pbPair.Values)
            {
                if (pb.PB.CustomName.ToString().Contains(Findstring))
                    results.Add(pb);
            }
        }
    }
}
