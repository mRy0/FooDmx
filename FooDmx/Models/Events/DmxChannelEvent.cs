using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooDmx.Models.Events
{
    public class DmxChannelEvent
    {
        public ushort ChannelId { get; set; }
        public byte Value { get; set; }
    }
}
