using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        private readonly IConverterService _converter;

        /// <summary>
        /// Initialises a new instance of the <c>MainWindowViewModel</c> class.
        /// </summary>
        /// <param name="converter"><c>ConverterService</c> instance.</param>
        public MainWindowViewModel(IConverterService converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            this._converter = converter;
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Events

        #region Properties

        private ObservableCollection<string> _encodings;

        /// <summary>
        /// Gets or sets the list of encodings.
        /// </summary>
        public ObservableCollection<string> Encodings
        {
            get
            {
                if (this._encodings == null || !this._encodings.Any())
                {
                    var encodings = this._converter
                                        .Encodings
                                        .Select(p => String.Format("{0} - {1} - {2}", p.Name, p.DisplayName, p.CodePage));

                    this._encodings = new ObservableCollection<string>(encodings);
                }
                return this._encodings;
            }
            set
            {
                this._encodings = value;
                OnPropertyChanged();
            }
        }

        private string _inputEncoding;

        /// <summary>
        /// Gets or sets the input encoding.
        /// </summary>
        public string InputEncoding
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._inputEncoding))
                {
                    this._inputEncoding = this.Encodings.Single(p => p.StartsWith("ks_c_5601-1987"));
                }
                return this._inputEncoding;
            }
            set
            {
                this._inputEncoding = value;
                OnPropertyChanged();
            }
        }

        private string _outputEncoding;

        /// <summary>
        /// Gets or sets the output encoding.
        /// </summary>
        public string OutputEncoding
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._outputEncoding))
                {
                    this._outputEncoding = this.Encodings.Single(p => p.StartsWith("utf-8"));
                }
                return this._outputEncoding;
            }
            set
            {
                this._outputEncoding = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}