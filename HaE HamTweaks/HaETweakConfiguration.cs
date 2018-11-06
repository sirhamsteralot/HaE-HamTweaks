using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace HaE_HamTweaks
{
    [Serializable]
    public class HaETweakConfiguration
    {
        [XmlIgnore]
        public string fileName => "HaEHamTweaks.cfg";

        public float maxFPS = 240;

        public HaETweakConfiguration()
        {

        }
    }
}
