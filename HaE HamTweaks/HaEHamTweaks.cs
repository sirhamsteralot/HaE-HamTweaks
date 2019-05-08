using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using VRage.Plugins;
using HaEPluginCore;
using HaEHamTweaks.Managers;
using HaEHamTweaks.Patching;

namespace HaE_HamTweaks
{
    public class HaEHamTweaks : IPlugin
    {
        public const int MinBasePluginVersion = 1060;

        public static HaETweakConfiguration config;
        public static HaEUITweaks uiTweaks;
        public static HaEUXTweaks uxTweaks;
        public static HaERenderTweaks renderTweaks;

        public static TexturePackManager textureManager;

        public void Init(object gameInstance)
        {
            if (HaEConstants.versionNumber < MinBasePluginVersion)
                throw new Exception("HaE Plugincore, BasePlugin out of date! please update!");

            config = new HaETweakConfiguration();
            DeSerialize();

            uiTweaks = new HaEUITweaks();
            uxTweaks = new HaEUXTweaks();
            renderTweaks = new HaERenderTweaks();

            textureManager = new TexturePackManager();

        }

        public void Update()
        {
            uiTweaks.OnUpdate();
            uxTweaks.OnUpdate();
            renderTweaks.OnUpdate();
        }

        public void Dispose()
        {
            uiTweaks.OnDispose();
            uxTweaks.OnDispose();
            renderTweaks.OnDispose();
            Save();
        }

        public static void Save()
        {
            if (!Directory.Exists($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}"))
                Directory.CreateDirectory($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}");

            using (var writer = new StreamWriter($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}\\{config.fileName}"))
            {
                var x = new XmlSerializer(typeof(HaETweakConfiguration));
                x.Serialize(writer, config);
                writer.Close();
            }
        }

        public static void DeSerialize()
        {
            if (Directory.Exists($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}"))
            {
                try
                {
                    using (var writer = new StreamReader($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}\\{config.fileName}"))
                    {
                        var x = new XmlSerializer(typeof(HaETweakConfiguration));
                        config = (HaETweakConfiguration)x.Deserialize(writer);
                        writer.Close();
                    }
                }
                catch (FileNotFoundException e)
                {
                    //nom
                }
            }
        }
    }
}
