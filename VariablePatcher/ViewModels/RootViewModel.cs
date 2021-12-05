using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jot;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Stylet;
using VariablePatcher.Models;

namespace VariablePatcher.ViewModels
{
    public class RootViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        public RootViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            Tracker tracker = new();
            tracker.Configure<RootViewModel>()
                .Id(p => p.WindowName, includeType: false)
                .Property(p => p.Height, 500, "Window Height")
                .Property(p => p.Width, 900, "Window Width")

                .Property(p => p.VariableItems, "Saved Variable Paths")
                .Property(p => p.VariableText, string.Empty, "Potential Variable Path")

                .Property(p => p.AdminMenuItems, "Saved Admin Menu Paths")
                .Property(p => p.AdminMenuText, string.Empty, "Potential Admin Menu Path")

                .Property(p => p.IsSameLocationChecked, false, "IsSameLocationChecked CheckBox")
                .Property(p => p.SameLocationVariableText, string.Empty, "Same Location Variable Path")
                .Property(p => p.SameLocationAdminMenuText, string.Empty, "Same Location AdminMenu Path")

                .Property(p => p.IsAutoChecked, false, "IsAutoChecked CheckBox")

                .Property(p => p.IsDarkTheme, false, "Toggle theme")
                .Property(p => p.VariableSplitKey, "->", "Variable Split Key that the app should use when splitting variables in .txt file")

                .PersistOn(nameof(PropertyChanged));
            tracker.Track(this);

            if (IsAutoChecked)
                _ = Patch();
        }

        public string WindowName => "RootWindow";
        public string Title => "Variable Patcher | 0.1 by Coke";

        private double _height;
        public double Height
        {
            get => _height;
            set => SetAndNotify(ref _height, value);
        }

        private double _width;
        public double Width
        {
            get => _width;
            set => SetAndNotify(ref _width, value);
        }

        //Left ListView
        public BindableCollection<FileModel> VariableItems { get; set; } = new();

        public void VariableMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in VariableItems)
                item.IsSelected = false;
        }

        public void VariableSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;

            var workingWidth = listView.ActualWidth - 18;
            var col1 = 0.30;
            var col2 = 0.35;
            var col3 = 0.30;

            if (workingWidth < 0)
                return;

            var gridView = listView.View as GridView;
            gridView.Columns[0].Width = workingWidth * col1;
            gridView.Columns[1].Width = workingWidth * col2;
            gridView.Columns[2].Width = workingWidth * col3;
        }

        public void DeleteVariablePath()
        {
            var items = VariableItems.Where(i => i.IsSelected);
            VariableItems.RemoveRange(items.ToList());

            if (VariableItems.Count > 0)
                if (VariableItems.All(i => i.IsPrioritised == false))
                    VariableItems[0].IsPrioritised = true;
        }

        public void VariableFilePathHeaderClick()
        {
            if (VariableItems.Count == 0)
                return;

            VariableItems = new BindableCollection<FileModel>(VariableItems.Reverse());
            NotifyOfPropertyChange(() => VariableItems);
        }

        private string _variableText;
        public string VariableText
        {
            get => _variableText;
            set
            {
                SetAndNotify(ref _variableText, value);

                NotifyOfPropertyChange(nameof(CanAddVariablePath));
            }
        }

        public bool CanAddVariablePath => !string.IsNullOrWhiteSpace(VariableText);

        public void AddVariablePath()
        {
            if (VariableItems.All(i => i.FilePath != VariableText))
            {
                VariableItems.Add(new FileModel()
                {
                    FilePath = VariableText,
                    FileAddedDateString = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    IsPrioritised = VariableItems.Count == 0
                });

                VariableText = string.Empty;
            }
            else
                _windowManager.ShowMessageBox($"\"{VariableText}\" is already in the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //Right ListView
        public BindableCollection<FileModel> AdminMenuItems { get; set; } = new();

        public void AdminMenuMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in AdminMenuItems)
                item.IsSelected = false;
        }

        public void AdminMenuSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;

            var workingWidth = listView.ActualWidth - 18;
            var col1 = 0.30;
            var col2 = 0.35;
            var col3 = 0.30;

            if (workingWidth < 0)
                return;

            var gridView = listView.View as GridView;
            gridView.Columns[0].Width = workingWidth * col1;
            gridView.Columns[1].Width = workingWidth * col2;
            gridView.Columns[2].Width = workingWidth * col3;
        }

        public void DeleteAdminMenuPath()
        {
            var items = AdminMenuItems.Where(i => i.IsSelected);
            AdminMenuItems.RemoveRange(items.ToList());

            if (AdminMenuItems.Count > 0)
                if (AdminMenuItems.All(i => i.IsPrioritised == false))
                    AdminMenuItems[0].IsPrioritised = true;
        }

        public void AdminMenuFilePathHeaderClick()
        {
            if (AdminMenuItems.Count == 0)
                return;

            AdminMenuItems = new BindableCollection<FileModel>(AdminMenuItems.Reverse());
            NotifyOfPropertyChange(() => AdminMenuItems);
        }

        private string _adminMenuText;
        public string AdminMenuText
        {
            get => _adminMenuText;
            set
            {
                SetAndNotify(ref _adminMenuText, value);

                NotifyOfPropertyChange(nameof(CanAddAdminMenuPath));
            }
        }

        public bool CanAddAdminMenuPath => !string.IsNullOrWhiteSpace(AdminMenuText);

        public void AddAdminMenuPath()
        {
            if (AdminMenuItems.All(i => i.FilePath != AdminMenuText))
            {
                AdminMenuItems.Add(new FileModel()
                {
                    FilePath = AdminMenuText,
                    FileAddedDateString = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    IsPrioritised = AdminMenuItems.Count == 0
                });

                AdminMenuText = string.Empty;
            }
            else
                _windowManager.ShowMessageBox($"\"{AdminMenuText}\" is already in the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //Bottom
        private bool _isSameLocationChecked;
        public bool IsSameLocationChecked
        {
            get => _isSameLocationChecked;
            set
            {
                SetAndNotify(ref _isSameLocationChecked, value);

                if (IsSameLocationChecked)
                    IsSameLocationVariableEnabled = IsSameLocationAdminMenuEnabled = true;
                else
                    IsSameLocationVariableEnabled = IsSameLocationAdminMenuEnabled = false;
            } 
        }

        public enum FileType
        {
            VariableFilePath = 0,
            AdminMenuFilePath = 1,
            VariableFileName = 2,
            AdminMenuFileName = 3
        }

        public void ChooseFile(string fileType)
        {
            var file = Enum.Parse<FileType>(fileType);

            OpenFileDialog openFileDialog = new OpenFileDialog();

            switch (file)
            {
                case FileType.VariableFilePath:
                    openFileDialog.Filter = "Text file (*.txt)|*.txt";
                    if (openFileDialog.ShowDialog() ?? false)
                        VariableText = openFileDialog.FileName;
                    break;
                case FileType.AdminMenuFilePath:
                    openFileDialog.Filter = "SQF file (*.sqf)|*.sqf";
                    if (openFileDialog.ShowDialog() ?? false)
                        AdminMenuText = openFileDialog.FileName;
                    break;
                case FileType.VariableFileName:
                    openFileDialog.Filter = "Text file (*.txt)|*.txt";
                    if (openFileDialog.ShowDialog() ?? false)
                        SameLocationVariableText = openFileDialog.SafeFileName;
                    break;
                case FileType.AdminMenuFileName:
                    openFileDialog.Filter = "SQF file (*.sqf)|*.sqf";
                    if (openFileDialog.ShowDialog() ?? false)
                        SameLocationAdminMenuText = openFileDialog.SafeFileName;
                    break;
            }
        }

        private string _sameLocationVariableText;
        public string SameLocationVariableText
        {
            get => _sameLocationVariableText;
            set => SetAndNotify(ref _sameLocationVariableText, value);
        }

        private bool _isSameLocationVariableEnabled;
        public bool IsSameLocationVariableEnabled
        {
            get => _isSameLocationVariableEnabled;
            set => SetAndNotify(ref _isSameLocationVariableEnabled, value);
        }

        private string _sameLocationAdminMenuText;
        public string SameLocationAdminMenuText
        {
            get => _sameLocationAdminMenuText;
            set => SetAndNotify(ref _sameLocationAdminMenuText, value);
        }

        private bool _isSameLocationAdminMenuEnabled;
        public bool IsSameLocationAdminMenuEnabled
        {
            get => _isSameLocationAdminMenuEnabled;
            set => SetAndNotify(ref _isSameLocationAdminMenuEnabled, value);
        }

        public async Task Patch()
        {
            string variableFileLocation = string.Empty;
            string adminMenuFileLocation = string.Empty;

            if (IsSameLocationChecked)
            {
                
            }
            else
            {
                
            }
        }

        private bool _isAutoChecked;
        public bool IsAutoChecked
        {
            get => _isAutoChecked;
            set => SetAndNotify(ref _isAutoChecked, value);
        }

        //Options
        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                SetAndNotify(ref _isDarkTheme, value);

                if (IsDarkTheme)
                    IsDarkThemeClicked();
            }
        }

        private readonly PaletteHelper _paletteHelper = new();
        public void IsDarkThemeClicked()
        {
            ITheme theme = _paletteHelper.GetTheme();

            IBaseTheme baseTheme = IsDarkTheme ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();

            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private string _variableSplitKey;
        public string VariableSplitKey
        {
            get => _variableSplitKey;
            set => SetAndNotify(ref _variableSplitKey, value);
        }
    }
}
