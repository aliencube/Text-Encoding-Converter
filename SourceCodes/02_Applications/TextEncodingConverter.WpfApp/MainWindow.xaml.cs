using Aliencube.TextEncodingConverter.Configs.Interfaces;
using Aliencube.TextEncodingConverter.DataContainers;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Aliencube.TextEncodingConverter.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITextEncodingConverterSettings _settings;
        private readonly IConverterService _converter;
        private readonly MainWindowViewModel _vm;

        private static object _lock = new object();

        /// <summary>
        /// Initialises a new instance of the <c>MailWindow</c> class.
        /// </summary>
        /// <param name="settings"><c>TextEncodingConverterSettings</c> instance.</param>
        /// <param name="conveter"><c>ConverterService</c> instance.</param>
        /// <param name="vm"><c>MainWindowViewModel</c> instance.</param>
        public MainWindow(ITextEncodingConverterSettings settings, IConverterService conveter, MainWindowViewModel vm)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            this._settings = settings;

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
                var extensions = String.Join(";", this._settings
                                                      .Converter
                                                      .Extensions
                                                      .Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(p => "*." + p));
                dialog.Filters.Add(new CommonFileDialogFilter("Text documents", extensions));
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
            if (this.ConvertedNames.HasItems)
            {
                this.ConvertedNames.Items.Clear();
            }

            if (this.AllowBackup.IsChecked.GetValueOrDefault())
            {
                this._converter.Backup(this.Filenames.Items.Cast<string>());
            }

            var ie = ((EncodingInfoDataContainer)this.InputEncoding.SelectedValue).Name;
            var oe = ((EncodingInfoDataContainer)this.OutputEncoding.SelectedValue).Name;
            var o = this._settings.Converter.OutputPath;

            var results = new List<string>();
            foreach (var i in this.Filenames.Items.Cast<string>())
            {
                results.Add(this.ProcessConvert(ie, oe, i, o));
            }

            this.ConvertedNames.ItemsSource = results;
        }

        /// <summary>
        /// Processes the conversion.
        /// </summary>
        /// <param name="ie">Input encoding.</param>
        /// <param name="oe">Output encoding.</param>
        /// <param name="i">Input file.</param>
        /// <param name="o">Output directory.</param>
        private string ProcessConvert(string ie, string oe, string i, string o)
        {
            var args = new List<string>()
                       {
                           "/f",
                           String.Format("/ie:{0}", ie),
                           String.Format("/oe:{0}", oe),
                           String.Format("/i:\"{0}\"", i),
                           String.Format("/o:{0}", o)
                       };

            var converted = this._converter.Convert(args, false);
            var result = String.Format("=> {0}", (converted ? "Converted" : "Failed"));
            return result;
        }
    }
}