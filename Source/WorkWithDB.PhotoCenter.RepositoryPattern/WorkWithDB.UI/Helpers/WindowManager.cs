using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private static Dictionary<string, UIElement> _viewsDictionary = new Dictionary<string, UIElement>() 
        {
            {"CustomerRegister", CustomerRegister},
            {"PersonalCardRegister", PersonalCardRegister},
            {"StructuralUnitList", StructuralUnitList},
            {"StructureUnitEditor", StructureUnitEditor},
            {"Settings", Settings }
        };

        private static SettingsEditor Settings
        {
            get
            {
                return new SettingsEditor();
            }
        }

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

        private static StructuralUnitList StructuralUnitList
        {
            get
            {
                return new StructuralUnitList();
            }
        }

        private static StructureUnitEditor StructureUnitEditor
        {
            get
            {
                return new StructureUnitEditor();
            }
        }

        public static void SetMainViewModel(MainWindowVM viewModel)
        {
            _mainWindowViewModel = viewModel;
        }

        public static void ChangeInfoView(string viewName)
        {
            if (IsLegalOperation(viewName))
            {
                _mainWindowViewModel.InfoVeiew = _viewsDictionary[viewName];
            }            
        }

        public static void ChangeMainView(string viewName)
        {
            if(IsLegalOperation(viewName))
            {
                _mainWindowViewModel.MainView = _viewsDictionary[viewName];  
            }                 
        }

        private static bool IsLegalOperation(string viewName)
        {
            if (_mainWindowViewModel == null)
            {
                throw new InvalidOperationException("main window view model = null");
            }

            if (!_viewsDictionary.Keys.Contains(viewName))
            {
                throw new InvalidOperationException("no such view");
            }
            return true;
        }
    }
}
