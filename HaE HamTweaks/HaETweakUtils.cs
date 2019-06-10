using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEHamTweaks
{
    public class HaETweakUtils
    {
        public static bool TryGetPlayerByName(ref MyPlayer player, string name)
        {
            if (MyMultiplayer.Static == null || MySession.Static == null)
                return false;

            var playerId = MySession.Static.Players.GetAllPlayers().FirstOrDefault(x => MySession.Static.Players.TryGetIdentityNameFromSteamId(x.SteamId) == name);
            if (MySession.Static.Players.TryGetPlayerById(playerId, out player))
                return true;

            return false;
        }
    }
}
