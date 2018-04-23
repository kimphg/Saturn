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
                if (CommonParam.UserName == "Admin")                
                    Application.Run(new SystemDataForm());                
                else                
                    Application.Run(new NewSessionForm());
                
            }
            
        }
    }
}
