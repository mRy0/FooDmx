using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FooDmx.Outputs.ArtNET
{
    public class ArtNETOutput : IOutput
    {
        public ArtNETOutput() { }

        public string Name => "ArtNET!";

        public bool IsActive => false;

        public byte[] Addresses => throw new NotImplementedException();

        public event Action<byte[]> Updated;

        public UserControl GetOptionsPage()
        {
            return new ArtNETSettingsPage();
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void SetAddresses(byte[] addresses)
        {

        }
    }
}
