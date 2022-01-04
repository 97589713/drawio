using MyTools.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

namespace WebViewTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			this.Loaded += MainWindow_Loaded;
        }

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await webView.EnsureCoreWebView2Async(await CommonConstant.GetWebView2EnvironmentAsync());
			webView.CoreWebView2.AddHostObjectToScript("csharp", new CallbackObjectForJs());
			//webView.Source = new Uri(@"file://D:\github\drawio\src\main\webapp\index.html?local=1&splash=0&math=0");
			webView.Source = new Uri(@"file://D:\github\drawio\src\main\webapp\index.html?local=1&splash=0&dev=1&math=0");
			//webView.Source = new Uri(@"file://D:\github\drawio\src\main\webapp\index.html?math=0");

			//webView.Source = new Uri(@"file:///D:/git/vctool/client/extjs670/resources/drawio-8.3.5/src/main/webapp/index.html?dev=1");
		}
	}

	[ComVisible(true)]
	public class CallbackObjectForJs
	{
		private static readonly Dictionary<string, FileStream> FileStreamDic = new Dictionary<string, FileStream>();

		public string InsertFile()
		{
			var ofd = new Microsoft.Win32.OpenFileDialog
			{
				DefaultExt = ".*"
			};

			var fileName = "";
			if (ofd.ShowDialog() == true)
			{
				//fileName = ofd.FileName;
				fileName = System.IO.Path.GetFileName(ofd.FileName);
				File.Copy(ofd.FileName, System.IO.Path.Combine(@"D:\github\ckeditor5-localfile\sample", fileName), true);
			}
			else
			{

			}

			return fileName;
		}

		public string DropFile(string name, long start, long totalSize, string content)
		{
			var bytes = Convert.FromBase64String(content.Replace("data:application/octet-stream;base64,", "")); //Encoding.GetEncoding("ISO-8859-1").GetBytes(content);
			var key = name + totalSize;
			string filePath;
			if (FileStreamDic.ContainsKey(key))
			{
				var fs = FileStreamDic[key];
				fs.Write(bytes, 0, bytes.Length);
				fs.Flush();
				filePath = fs.Name;
			}
			else
			{
				//filePath = System.IO.Path.Combine(@"D:\github\ckeditor5-localfile\sample", name);
				filePath = System.IO.Path.Combine(@"D:\video", name);
				var fs = new FileStream(filePath, FileMode.Create);
				fs.Write(bytes, 0, bytes.Length);
				fs.Flush();
				FileStreamDic.Add(key, fs);
			}

			if (start + bytes.Length >= totalSize)
			{
				FileStreamDic[key].Close();
				FileStreamDic.Remove(key);
			}

			return name;
		}
	}
}
