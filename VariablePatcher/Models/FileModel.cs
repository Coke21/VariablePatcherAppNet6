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

        private bool _isPrioritised;
        public bool IsPrioritised
        {
            get => _isPrioritised;
            set
            {
                _isPrioritised = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
