using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadcastsSchedule
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BSForm = new BroadcastsSchedule();
            Application.Run(BSForm);
        }
        public static BroadcastsSchedule BSForm = null;
    }
}
