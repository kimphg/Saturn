using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRS_Demo
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
            fLoginForm loginForm = new fLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                //Application.Run(new RecordingForm());
                Application.Run(new NewSessionForm());
            }
            
        }
    }
}
