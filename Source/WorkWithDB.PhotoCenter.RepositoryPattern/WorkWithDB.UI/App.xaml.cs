﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.PostgreSQL;

namespace WorkWithDB.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());
        }
    }
}
