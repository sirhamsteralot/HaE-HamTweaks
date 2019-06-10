using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.SessionComponents;

namespace HaEHamTweaks.Profiling
{
    public class PBTracker
    {
        public string PBID { get { return $"{PB.CubeGrid.DisplayName}-{PB.CustomName}"; } }
        public bool IsEnabled => PB.Enabled;
        public double AverageMS => averageMs;
        public string Owner => MySession.Static.Players.TryGetIdentity(PB.OwnerId).DisplayName;

        public MyProgrammableBlock PB;
        public double averageMs;
        public DateTime lastExecutionTime;

        public PBTracker(MyProgrammableBlock PB, double ms)
        {
            this.PB = PB;
            UpdatePerformance(ms);
        }

        public void UpdatePerformance(double ms)
        {
            lastExecutionTime = DateTime.Now;

            averageMs = (1 - PBProfiling.tickSignificance) * averageMs + PBProfiling.tickSignificance * ms;
        }
    }
}
