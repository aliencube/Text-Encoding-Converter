using Aliencube.TextEncodingConverter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
    }
}