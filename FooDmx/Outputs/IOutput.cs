using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FooDmx.Outputs
{
    public interface IOutput
    {
        string Name { get; }

        bool IsActive { get; }

        byte[] Addresses { get; }

        void SetAddresses(byte[] addresses);

        event Action<byte[]> Updated;


        Task RunAsync(CancellationToken cancellationToken);

        System.Windows.Controls.UserControl GetOptionsPage();
    }
}
