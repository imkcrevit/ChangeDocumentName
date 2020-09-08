using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ChangDocumentName.Annotations;

namespace ChangDocumentName.Property
{
    class GetFiles
    {
        public Dictionary<string, string> GetAllFolders(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            Dictionary<string,string> dic = new Dictionary<string, string>();
            if (info.Exists)
            {
                var infos = new List<string>(Directory.GetDirectories(path, "*",System.IO.SearchOption.TopDirectoryOnly));
                foreach (var  dir in infos)
                {
                    dic.Add(dir,dir.Split('\\').Last());
                }
                return dic;

            }

            return null;
        }

        public void GetAllDictionary(string path,ref ObservableCollection<FileDirectory> files1,ref ObservableCollection<FileDirectory> files2)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            ObservableCollection<FileDirectory> fileDirectories1 = new ObservableCollection<FileDirectory>();
            ObservableCollection<FileDirectory> fileDirectories2 = new ObservableCollection<FileDirectory>();
            if (info.Exists)
            {
                var infos = info.GetFiles("*", SearchOption.AllDirectories);
                foreach (var fileInfo in infos)
                {
                    var pathName = fileInfo.FullName;
                    var ext = fileInfo.Extension;
                    if (path.Contains(fileInfo.DirectoryName ?? string.Empty))
                    {
                        if (ext.Equals(".addin"))
                        {
                            var name = fileInfo.Name;
                            var trimEnd = name.TrimEnd(".addin".ToCharArray());
                            fileDirectories1.Add(new FileDirectory(){Name = trimEnd,Path = pathName});
                        }
                        else if (ext.Equals(".addin-"))
                        {
                            var name = fileInfo.Name;
                            string trimEnd = name.TrimEnd(".addin-".ToCharArray());
                            fileDirectories2.Add(new FileDirectory(){Name = trimEnd,Path = pathName});
                        }
                    }
                    
                }

                
            }

            files1 = fileDirectories1;
            files2 = fileDirectories2;

        }
        public class FileDirectory:INotifyPropertyChanged
        {
            /// <summary>
            /// 文件名称
            /// </summary>
            private string _name;
            public string Name
            {
                get => _name;
                set { _name = value;OnPropertyChanged(nameof(Name)); }
            }
            /// <summary>
            /// 文件路径
            /// </summary>
            private string _path;

            public string Path
            {
                get => _path;
                set { _path = value;OnPropertyChanged(nameof(Path)); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
