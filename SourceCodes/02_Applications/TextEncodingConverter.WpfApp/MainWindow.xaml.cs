using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aliencube.TextEncodingConverter.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConverterService _converter;
        private readonly MainWindowViewModel _vm;

        /// <summary>
        /// Initialises a new instance of the <c>MailWindow</c> class.
        /// </summary>
        /// <param name="conveter"></param>
        /// <param name="vm"></param>
        public MainWindow(IConverterService conveter, MainWindowViewModel vm)
        {
            if (conveter == null)
            {
                throw new ArgumentNullException("conveter");
            }
            this._converter = conveter;

            if (vm == null)
            {
                throw new ArgumentNullException("vm");
            }
            this._vm = vm;

            InitializeComponent();

            this.DataContext = this._vm;
        }

        /// <summary>
        /// Performs when the Browse button is clicked.
        /// </summary>
        /// <param name="sender">The Browse button.</param>
        /// <param name="e">Event instance.</param>
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog()
                                {
                                    AllowNonFileSystemItems = true,
                                    Multiselect = true
                                })
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Text documents", "*.smi;*.srt;*.txt"));
                dialog.Filters.Add(new CommonFileDialogFilter("All documents", "*.*"));

                var result = dialog.ShowDialog();
                if (result != CommonFileDialogResult.Ok)
                {
                    return;
                }

                var filenames = dialog.FileNames;
                this.Filenames.ItemsSource = filenames;
            }
        }

        /// <summary>
        /// Performs when the Convert button is clicked.
        /// </summary>
        /// <param name="sender">The Convert button.</param>
        /// <param name="e">Event instance.</param>
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (this.AllowBackup.IsChecked.GetValueOrDefault())
            {
                this._converter.Backup(this.Filenames.Items.Cast<string>());
            }

            var ie = ((string)this.InputEncoding.SelectedValue).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var oe = ((string)this.OutputEncoding.SelectedValue).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var o = "Converted";

            Parallel.ForEach(this.Filenames.Items.Cast<string>(), i => this.ProcessConvert(ie, oe, i, o));
        }

        /// <summary>
        /// Processes the conversion.
        /// </summary>
        /// <param name="ie">Input encoding.</param>
        /// <param name="oe">Output encoding.</param>
        /// <param name="i">Input file.</param>
        /// <param name="o">Output directory.</param>
        private void ProcessConvert(string ie, string oe, string i, string o)
        {
            var args = new List<string>()
                       {
                           "/f",
                           String.Format("/ie:{0}", ie),
                           String.Format("/oe:{0}", oe),
                           String.Format("/i:\"{0}\"", i),
                           String.Format("/o:{0}", o)
                       };

            var result = this._converter.Convert(args, false);
            this.ConvertedNames.Text += String.Format("{0} => {1}", i, (result ? "Converted" : "Failed"));
        }
    }
}