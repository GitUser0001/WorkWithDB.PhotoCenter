﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkWithDB.UI.ViewModel;
using WorkWithDB.UI.Views.Customers;

namespace WorkWithDB.UI.Helpers
{
    public static class WindowManager
    {
        private static MainWindowVM _mainWindowViewModel;

        private static Dictionary<string, UIElement> _viewsDictionary = new Dictionary<string, UIElement>() 
        {
            {"CustomerRegister", CustomerRegister},
            {"PersonalCardRegister", PersonalCardRegister}
        };

        private static CustomerRegister CustomerRegister
        {
            get
            {
                return new CustomerRegister();
            }
        }

        private static PersonalCardRegister PersonalCardRegister
        {
            get
            {
                return new PersonalCardRegister();
            }
        }

        public static void SetMainViewModel(MainWindowVM viewModel)
        {
            _mainWindowViewModel = viewModel;
        }

        public static void ChangeView(string viewName)
        {
            if (_mainWindowViewModel == null)
            {
                throw new InvalidOperationException("main window view model = null");
            }

            if (!_viewsDictionary.Keys.Contains(viewName))
            {
                throw new InvalidOperationException("no such view");
            }

            _mainWindowViewModel.MainView = _viewsDictionary[viewName];
        }
    }
}