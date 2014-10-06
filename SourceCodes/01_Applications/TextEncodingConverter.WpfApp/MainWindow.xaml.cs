using Aliencube.TextEncodingConverter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Aliencube.TextEncodingConverter.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ParameterService _parameterService;
        private readonly ConverterService _converterService;

        public MainWindow()
        {
            InitializeComponent();

            this._parameterService = new ParameterService();
            this._converterService = new ConverterService(this._parameterService);

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var items = this._converterService
                            .Encodings
                            .Select(p => new ComboBoxItem()
                                         {
                                             Content = String.Format("{0}:::{1}:::{2}", p.CodePage, p.Name, p.DisplayName)
                                         })
                            .ToList();
            var inputs = new List<ComboBoxItem>() { new ComboBoxItem() { Content = "Input Encoding" } };
            inputs.AddRange(items);
            var outputs = new List<ComboBoxItem>() { new ComboBoxItem() { Content = "Output Encoding" } };
            outputs.AddRange(items);

            this.InputEncoding.ItemsSource = inputs;
            this.InputEncoding.SelectedIndex = 0;
            this.OutputEncoding.ItemsSource = outputs;
            this.OutputEncoding.SelectedIndex = 0;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog()
                                {
                                    AllowNonFileSystemItems = true,
                                    Multiselect = true
                                })
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Text documents", "*.txt;*.srt;*.smi"));
                dialog.Filters.Add(new CommonFileDialogFilter("All documents", "*.*"));

                var result = dialog.ShowDialog();
                if (result != CommonFileDialogResult.Ok)
                {
                    return;
                }

                var filenames = dialog.FileNames;
                this.Filenames.Text = String.Join("\r\n", filenames);
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            var args = new List<string>() { "/f" };
        }
    }
}