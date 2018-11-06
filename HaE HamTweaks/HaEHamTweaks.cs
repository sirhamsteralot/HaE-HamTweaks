using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using VRage.Plugins;

namespace HaE_HamTweaks
{
    public class HaEHamTweaks : IPlugin
    {
        public static string StoragePath => Path.GetDirectoryName(typeof(HaEHamTweaks).Assembly.Location);

        public static HaETweakConfiguration config;
        public static HaEUITweaks uiTweaks;
        public static HaEUXTweaks uxTweaks;
        public static HaERenderTweaks renderTweaks;

        public void Init(object gameInstance)
        {
            config = new HaETweakConfiguration("HaEHamTweaks.cfg");
            DeSerialize();

            uiTweaks = new HaEUITweaks();
            uxTweaks = new HaEUXTweaks();
            renderTweaks = new HaERenderTweaks();
        }

        public void Update()
        {
            uiTweaks.OnUpdate();
            uxTweaks.OnUpdate();
            renderTweaks.OnUpdate();
        }

        public void Dispose()
        {
            Save();
        }

        public void Save()
        {
            using (var writer = new StreamWriter($"{StoragePath}\\{config.fileName}"))
            {
                var x = new XmlSerializer(typeof(HaETweakConfiguration));
                x.Serialize(writer, config);
                writer.Close();
            }
        }

        public void DeSerialize()
        {
            try
            {
                using (var writer = new StreamReader($"{StoragePath}\\{config.fileName}"))
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
