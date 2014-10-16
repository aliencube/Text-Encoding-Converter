using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            var args = new List<string>() { "/f" };
        }
    }
}