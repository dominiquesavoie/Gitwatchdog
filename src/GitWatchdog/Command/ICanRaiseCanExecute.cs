using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitWatchdog.Command
{
    public interface ICanRaiseCanExecute
    {
        void RaiseCanExecuteChanged();
    }
}
