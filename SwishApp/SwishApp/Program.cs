using System;
using System.Windows.Forms;
using SwishApp.Vista;

namespace SwishApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormIndex());
        }
    }
}