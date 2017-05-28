using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LinkCopyYa.XmlModel
{
    public class XmlLoader
    {
        public static CopyFiles Load(string filename)
        {
            using(var inputStream = new FileStream(filename, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(CopyFiles));
                return serializer.Deserialize(inputStream) as CopyFiles;
            }
        }
    }
}
