using System;
using System.Windows.Forms;

namespace SwishApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormClasificacion());
        }
    }
}