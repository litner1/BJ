﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BJ
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

            do{
                Application.Run(new Form1());
            } while (Form1.newgame);
        }
    }
}
