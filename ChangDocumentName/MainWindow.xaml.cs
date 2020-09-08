using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChangDocumentName.Property;
using Path = System.IO.Path;

namespace ChangDocumentName
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string TargetFilePath = @"C:\ProgramData\Autodesk\Revit\Addins\";
        ObservableCollection<string> names1 = new ObservableCollection<string>();
        ObservableCollection<string> names2 = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            GetFiles gf = new GetFiles();
            this.ComBox1.ItemsSource = gf.GetAllFolders(TargetFilePath).Values;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        ObservableCollection<GetFiles.FileDirectory> files1 = new ObservableCollection<GetFiles.FileDirectory>();
        ObservableCollection<GetFiles.FileDirectory> files2 = new ObservableCollection<GetFiles.FileDirectory>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.ComBox1.SelectedValue != null)
            {
                string path = TargetFilePath + this.ComBox1.SelectedValue;
               

                new GetFiles().GetAllDictionary(path, ref files1, ref files2);
                

                foreach (var fileDirectory in files1)
                {
                    names1.Add(fileDirectory.Name);
                }

                foreach (var fileDirectory in files2)
                {
                    names2.Add(fileDirectory.Name);
                }

                this.ListBox1.ItemsSource = files1;
                this.ListBox2.ItemsSource = files2;

            }
        }

        private void MakeUnStartUp_Click(object sender, RoutedEventArgs e)
        {
            var items = this.ListBox1.SelectedItems;
            List<GetFiles.FileDirectory> dirs = new List<GetFiles.FileDirectory>();
            foreach (var item in items)
            {
                dirs.Add(item as GetFiles.FileDirectory);
            }

            foreach (var dir in dirs)
            {
                //names2.Add(dir);
                //names1.Remove(dir);
                files2.Add(dir);
                files1.Remove(dir);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder("本次修改文件为：\r\n");
            foreach (var fileDirectory in files1)
            {
                var filePath = fileDirectory.Path;
                var ext = Path.GetExtension(filePath);
                if (ext.Equals(".addin-"))
                {
                    var changeExtension = Path.ChangeExtension(filePath, ".addin");
                    File.Move(filePath,changeExtension);
                    builder.AppendFormat("{0}", fileDirectory.Name+"\t\r\n");
                }
            }

            foreach (var fileDirectory in files2)
            {
                var filePath = fileDirectory.Path;
                var ext = Path.GetExtension(filePath);
                if (ext.Equals(".addin"))
                {
                    var changeExtension = Path.ChangeExtension(filePath, ".addin-");
                    File.Move(filePath, changeExtension);
                    builder.AppendFormat("{0}", fileDirectory.Name + "\t\r\n");
                }
            }

            MessageBox.Show(builder.ToString());
            Thread.Sleep(500);
            this.Close();
        }

        private void MakeStartUp_Click(object sender, RoutedEventArgs e)
        {
            var items = this.ListBox2.SelectedItems;
            List<GetFiles.FileDirectory> dirs = new List<GetFiles.FileDirectory>();
            foreach (var item in items)
            {
                dirs.Add(item as GetFiles.FileDirectory);
            }

            foreach (var dir in dirs)
            {
                //names2.Add(dir);
                //names1.Remove(dir);
                files1.Add(dir);
                files2.Remove(dir);
            }
        }
    }
}
