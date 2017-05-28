using LinkCopyYa.XmlModel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace LinkCopyYa
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = new MainViewModel();
            this.DataContext = vm;
            mainDataGrid.DataContextChanged += MainDataGrid_DataContextChanged;
        }

        private void MainDataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void baseDirBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // use WindowsAPICodePack
            var dlg = new CommonOpenFileDialog("Browse base directory.");
            dlg.IsFolderPicker = true;
            var ret = dlg.ShowDialog();
            if (ret == CommonFileDialogResult.Ok)
            {
                this.baseDirTextBox.Text = dlg.FileName;
            }
        }

        private void configFileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Open config file.";
            dialog.Filter = "XML Format(*.xml)|*.xml";
            if (dialog.ShowDialog() == true)
            {
                this.configFileTextBox.Text = dialog.FileName;
            }
        }

        private void loadXmlButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(baseDirTextBox.Text)) return;
            if (string.IsNullOrWhiteSpace(configFileTextBox.Text)) return;
            vm.LoadButtonClicked(configFileTextBox.Text, baseDirTextBox.Text);
        }

        // on checkbox clicked, uncheck other boxes in group.
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var groupNo = cb.TemplatedParent;
            var checkedRecordModel = cb.DataContext as RecordModel;
            var inGroupOtherCheckBoxes = vm.RecordModels
                .Where(m => m.FileGroupNo == checkedRecordModel.FileGroupNo)
                .Where(m => m != checkedRecordModel);
            foreach (var item in inGroupOtherCheckBoxes)
            {
                item.IsSelected = false;
            }
        }

        private void manualCopyButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure to overwrite files in other directories with checked items?",
                "Confirm",
                MessageBoxButton.OKCancel);
            if(result == MessageBoxResult.OK)
            {
                vm.ManualCopy();
            }
        }

        private void autoCopyButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure to overwrite files in other directories with the latest update date?",
                "Confirm",
                MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                vm.AutoCopy();
            }
        }

    }
}
