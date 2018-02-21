using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitWatchdog.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
