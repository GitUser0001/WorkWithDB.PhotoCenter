using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkWithDB.UI.ViewModel;
using WorkWithDB.UI.Views.Customers;
using WorkWithDB.UI.Views.Settings;
using WorkWithDB.UI.Views.StructuralUnits;
using WorkWithDB.UI.Views.StructuralUnits.CurrentUnitSetter;

namespace WorkWithDB.UI.Helpers
{
    public static class WindowManager
    {
        private static MainWindowVM _mainWindowViewModel;



        private static Dictionary<string, UserControl> _viewsCashedDictionary = new Dictionary<string, UserControl>()
        {
            {"CustomerRegister", new CustomerRegister()},
            {"PersonalCardRegister", new PersonalCardRegister()},
            {"Settings", new SettingsEditor()}
        };


        private static Dictionary<string, Func<UserControl>> _viewsDictionary = new Dictionary<string, Func<UserControl>>() 
        {
            {"StructuralUnitList", () => new StructuralUnitList()},
            {"StructureUnitEditor", () => new StructureUnitEditor()},
            {"CustomerList", () => new CustomerList()}
        };

        public static void SetMainViewModel(MainWindowVM viewModel)
        {
            _mainWindowViewModel = viewModel;
        }

        public static void ChangeInfoView(string viewName)
        {
            _mainWindowViewModel.InfoVeiew = ChangeView(viewName);
        }

        public static void ChangeMainView(string viewName)
        {
            _mainWindowViewModel.MainView = ChangeView(viewName);            
        }

        private static UIElement ChangeView(string viewName)
        {
            if (IsLegalOperation(viewName))
            {
                if (_viewsDictionary.Keys.Contains(viewName))
                {
                    return _viewsDictionary[viewName].Invoke();
                }
                else
                {
                    return _viewsCashedDictionary[viewName];
                }                
            }
            return null;
        }

        private static bool IsLegalOperation(string viewName)
        {
            if (_mainWindowViewModel == null)
            {
                throw new InvalidOperationException("main window view model = null");
            }

            if (!_viewsDictionary.Keys.Contains(viewName) && !_viewsCashedDictionary.Keys.Contains(viewName))
            {
                throw new InvalidOperationException("no such view");
            }
            return true;
        }
    }
}
