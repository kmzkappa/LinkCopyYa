using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCopyYa
{
    [System.Xml.Serialization.XmlRoot("copy_files")]
    public class CopyFiles
    {
        [System.Xml.Serialization.XmlElement("file")]
        public ObservableCollection<DirectoryGroup> FileGroup { get; set; }
    }

    public class DirectoryGroup
    {
        [System.Xml.Serialization.XmlElement("dir")]
        public ObservableCollection<DirectoryItem> Directories { get; set; }


        [System.Xml.Serialization.XmlAttribute("name")]
        public string FileName { get; set; }
    }

    public class DirectoryItem
    {
        [System.Xml.Serialization.XmlAttribute("default")]
        public bool IsDefault { get; set; }


        [System.Xml.Serialization.XmlText]
        public string DirectoryName { get; set; }
    }
}
