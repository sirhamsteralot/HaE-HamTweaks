using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace HaEHamTweaks
{
    [Serializable]
    public class HaETweakConfiguration
    {
        [XmlIgnore]
        public string fileName => "HaEHamTweaks.cfg";

        public float maxFPS = 240;
        public float lensDirtBloomRatio = 0;
        public float bloomMultiplier = 0.0025f;
        public float chromaticFactor = 0.025f;
        public bool enableBlockEdges = true;
        public bool mainMenuBanners = true;
        public bool lightingPatch = false;

        public HaETweakConfiguration()
        {

        }
    }
}
