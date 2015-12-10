using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Xps;
using System.Printing;

namespace WpfPrintSample
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 表示用データを管理します。
		/// </summary>
		private ReportSample FReport = new ReportSample();

		/// <summary>
		/// 表示用データを取得します。
		/// </summary>
		public ICollection<Person> List
		{
			get
			{
				return this.FReport.List;
			}
		}

		/// <summary>
		/// PrintDialogから印刷ボタンのクリックイベントを処理します。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void buttonClick(object sender, RoutedEventArgs e)
		{
			this.PrintDocument();
		}

		/// <summary>
		/// XpsDocumentWriterから印刷ボタンのクリックイベントを処理します。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void xpsPrintButtonClick(object sender, RoutedEventArgs e)
		{
			this.PrintXpsDocument();
		}

		/// <summary>
		/// PrintDialogを使用した印刷を行います。
		/// </summary>
		private void PrintDocument()
		{
			var dlg = new PrintDialog();
			if (dlg.ShowDialog() == true)
			{
				var page = new FixedPage();
				page.Children.Add(new SampleReport());

				var cont = new PageContent();
				cont.Child = page;

				var doc = new FixedDocument();
				doc.Pages.Add(cont);

				dlg.PrintDocument(doc.DocumentPaginator, "Print1");
			}
		}

		/// <summary>
		/// XpsDocumentWriterを使用した印刷を行います。
		/// </summary>
		private void PrintXpsDocument()
		{
			var page = new FixedPage();
			page.Children.Add(new SampleReport());

			var cont = new PageContent();
			cont.Child = page;

			var doc = new FixedDocument();
			doc.Pages.Add(cont);

			PrintDocumentImageableArea area = null;
			XpsDocumentWriter xps = PrintQueue.CreateXpsDocumentWriter(ref area);

			xps.Write(doc.DocumentPaginator);
		}

		/// <summary>
		/// 印刷ダイアログを出さずに既定のプリンタで画面のグリッドを印刷します。
		/// </summary>
		private void PrintXpsDocument2()
		{
			var lps = new LocalPrintServer();
			var q = lps.DefaultPrintQueue;
			XpsDocumentWriter xps = PrintQueue.CreateXpsDocumentWriter(q);

			VisualsToXpsDocument vs = (VisualsToXpsDocument)xps.CreateVisualsCollator();
			vs.Write(this.grid1);
			vs.EndBatchWrite();
		}

		private void xpsPrintButtonClick2(object sender, RoutedEventArgs e)
		{
			this.PrintXpsDocument2();
		}
	}
}
