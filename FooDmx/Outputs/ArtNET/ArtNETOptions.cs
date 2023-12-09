using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooDmx.Outputs.ArtNET
{
    public class ArtNETOptions
    {
        public string OutputInterface { get; set; }

        public bool UseBroadcast { get; set; }

        public string MulticastAddress { get; set; }

        public int HZ { get; set; }

    }
}
