using LinkCopyYa.XmlModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LinkCopyYa
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<RecordModel> _recordModels = new ObservableCollection<RecordModel>();
        public ObservableCollection<RecordModel> RecordModels
        {
            get
            {
                return _recordModels;
            }
            set
            {
                if (_recordModels == value) return;
                _recordModels = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecordModels)));
            }
        }


        public string BaseDirectory { get; set; }
        private int maxGroupNo;


        public MainViewModel()
        {

        }

        private IEnumerable ConvertXmlConfigToFileModels(CopyFiles copyFiles)
        {
            var models = new ObservableCollection<RecordModel>();
            int groupNo = 0;
            foreach (var file in copyFiles.FileGroup)
            {
                foreach (var dir in file.Directories)
                {
                    models.Add(new RecordModel
                    {
                        FileGroupNo = groupNo,
                        FileInformation = new System.IO.FileInfo(Path.Combine(BaseDirectory, dir.DirectoryName, file.FileName)),
                        IsSelected = dir.IsDefault
                    });
                }
                maxGroupNo = groupNo;
                groupNo++;
            }
            return models;
        }

        public void LoadButtonClicked(string configFile, string baseDir)
        {
            RecordModels.Clear();
            BaseDirectory = baseDir;
            CopyFiles copyFiles = null;
            try
            {
                copyFiles = XmlLoader.Load(configFile);
            }
            catch
            {
                MessageBox.Show("Config file load failed.", "Error");
                return;
            }

            if (!Directory.Exists(baseDir))
            {
                MessageBox.Show("Base directory not found.", "Error");
                return;
            }

            try
            {
                foreach (var model in ConvertXmlConfigToFileModels(copyFiles) as ObservableCollection<RecordModel>)
                {
                    RecordModels.Add(model);
                }
            }
            catch
            {
                MessageBox.Show("File status get failed.", "Error");
                return;
            }
        }

        public void ManualCopy()
        {
            var groupList = new List<List<RecordModel>>();
            for (int i = 0; i <= maxGroupNo; i++)
            {
                var fileGroup = new List<RecordModel>();
                fileGroup.AddRange(RecordModels.Where(r => r.FileGroupNo == i));
                groupList.Add(fileGroup);
            }

            var now = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

            using (StreamWriter writer = new StreamWriter(Path.Combine(BaseDirectory, "copylog_" + now + ".txt")))
            {
                foreach (var filesInGroup in groupList)
                {
                    if (filesInGroup.Count <= 1) continue;
                    if (filesInGroup.Any(f => f.IsSelected) == false) continue;

                    var selectedItem = filesInGroup.FirstOrDefault(f => f.IsSelected);
                    var notSelectedItems = filesInGroup.Where(f => f.IsSelected == false).ToList();



                    writer.WriteLine("Copy source: " + selectedItem.FileInformation.FullName);
                    foreach (var copyDest in notSelectedItems)
                    {
                        writer.Write("       dest: " + copyDest.FileInformation.FullName);
                        selectedItem.FileInformation.CopyTo(copyDest.FileInformation.FullName, true);
                        writer.WriteLine(" [OK]");
                    }

                    writer.WriteLine();

                }
            }

            MessageBox.Show("Copy completed.");
        }

        public void AutoCopy()
        {
        }

    }
}
