using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VariablePatcher.Models
{
    //Cannot use Stylet's Screen since it's not supported in Jot
    public class FileModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        private string _fileAddedDateString;
        public string FileAddedDateString
        {
            get => _fileAddedDateString;
            set
            {
                _fileAddedDateString = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Stylet not supported in Jot
        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get => _isSelected;
        //    set => SetAndNotify(ref _isSelected, value);
        //}

        //private string _filePath;
        //public string FilePath
        //{
        //    get => _filePath;
        //    set => SetAndNotify(ref _filePath, value);
        //}

        //private string _fileAddedDateString;
        //public string FileAddedDateString
        //{
        //    get => _fileAddedDateString;
        //    set => SetAndNotify(ref _fileAddedDateString, value);
        //}
    }
}
