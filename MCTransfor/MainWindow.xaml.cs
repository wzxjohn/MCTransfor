using Ionic.Zip;
using System;
using System.Collections.Generic;
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

namespace MCTransfor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetLanguageDictionary();
            newPath.Text = System.Environment.GetEnvironmentVariable("appdata") + "\\.minecraft";
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("en_us.xaml",
                                  UriKind.Relative);
                    break;
                case "zh-CN":
                    dict.Source = new Uri("zh_cn.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("en_us.xaml",
                                      UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void oldButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "minecraft.jar"; // Default file name
            dlg.DefaultExt = ".jar"; // Default file extension 
            dlg.Filter = "Java runnable (.jar)|*.jar"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog(); //show dialog

            if (result == true)
            {
                // Open document 
                oldPath.Text = dlg.FileName.Substring(0, dlg.FileName.Length - 18);
            }
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "minecraft.jar"; // Default file name
            dlg.DefaultExt = ".jar"; // Default file extension 
            dlg.Filter = "Java runnable (.jar)|*.jar"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog(); //show dialog

            if (result == true)
            {
                // Open document 
                newPath.Text = dlg.FileName.Substring(0, dlg.FileName.Length - 18);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldPath.Text == string.Empty)
            {
                MessageBox.Show(this.Resources["doNotSelectOldPath"].ToString(), this.Resources["error"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newPath.Text == string.Empty)
            {
                MessageBox.Show(this.Resources["doNotSelectNewPath"].ToString(), this.Resources["error"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Directory.Exists(oldPath.Text))
            {
                MessageBox.Show(this.Resources["oldPathNotExist"].ToString(), this.Resources["error"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Directory.Exists(newPath.Text))
            {
                if (MessageBox.Show(this.Resources["existsMessage"].ToString(), this.Resources["existsTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssff_backup");
                    //ZipFile实例化一个压缩文件保存路径的一个对象zip
                    using (ZipFile zip = new ZipFile(FileName + ".zip", Encoding.Default))
                    {
                        //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)
                        zip.AddDirectory(newPath.Text);
                        //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept
                        //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");
                        MessageBox.Show(this.Resources["backupStartMessageLine1"].ToString() + "\r\n" + this.Resources["backupStartMessageLine2"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        zip.Save();
                        MessageBox.Show(this.Resources["backupFinishMessage"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    if (MessageBox.Show(this.Resources["deleteStartMessageLine1"].ToString() + "\r\n" + this.Resources["deleteStartMessageLine2"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        if (Directory.Exists(newPath.Text))
                        {
                            Directory.Delete(newPath.Text, true);
                            MessageBox.Show(this.Resources["deleteFinishMessage"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            if (MessageBox.Show(this.Resources["moveStartMessageLine1"].ToString() + "\r\n" + this.Resources["moveStartMessageLine2"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (Directory.Exists(oldPath.Text))
                {
                    Directory.Copy(oldPath.Text, newPath.Text);
                    MessageBox.Show(this.Resources["moveFinishMessage"].ToString(), this.Resources["backupTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this.Resources["oldPathNotExist"].ToString(), this.Resources["error"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
