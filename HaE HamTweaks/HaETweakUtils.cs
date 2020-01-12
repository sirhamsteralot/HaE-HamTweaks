using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Reflection;
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

        public static string GetAnyValueS(string SourceTypeName, string FieldName, object sourceObject = null)
        {
            try
            {
                Type type = null;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(SourceTypeName, false);
                    if (type != null)
                        break;
                }

                if (type == null)
                    return "Could not resolve type";

                FieldInfo field = type.GetField(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                PropertyInfo property = type.GetProperty(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (field == null && property == null)
                    return "Could not resolve field/property";

                if (field != null)
                    return field.GetValue(sourceObject)?.ToString() ?? "Null";
                else
                    return property.GetValue(sourceObject)?.ToString() ?? "Null";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public static object GetAnyValue(string SourceTypeName, string FieldName, object sourceObject = null)
        {
            Type type = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(SourceTypeName, false);
                if (type != null)
                    break;
            }

            if (type == null)
                return null;

            FieldInfo field = type.GetField(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            PropertyInfo property = type.GetProperty(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (field == null && property == null)
                return null;

            if (field != null)
                return field.GetValue(sourceObject)?.ToString();
            else
                return property.GetValue(sourceObject)?.ToString();
        }
    }
}
