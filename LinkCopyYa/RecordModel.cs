using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCopyYa
{
    public class RecordModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _fileGroupNo = -1;
        public int FileGroupNo
        {
            get
            {
                return _fileGroupNo;
            }
            set
            {
                if (_fileGroupNo == value) return;
                _fileGroupNo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileGroupNo)));
            }
        }

        public bool IsEvenGroup
        {
            get
            {
                return FileGroupNo % 2 != 0;
            }
        }

        public bool IsFileExists
        {
            get
            {
                return FileInformation.Exists;
            }
        }

        public string LastWriteTime
        {
            get
            {
                if (FileInformation.Exists)
                {
                    return FileInformation.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private FileInfo _fileInformation;
        public FileInfo FileInformation
        {
            get
            {
                return _fileInformation;
            }
            set
            {
                if (_fileInformation == value) return;
                _fileInformation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileInformation)));
            }
        }

        public string DirectoryName { get; set; }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }
}
