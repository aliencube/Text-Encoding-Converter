using Aliencube.TextEncodingConverter.Configs.Interfaces;
using Aliencube.TextEncodingConverter.DataContainers;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliencube.TextEncodingConverter.Services
{
    /// <summary>
    /// This represents the parameter service entity.
    /// </summary>
    public class ParameterService : IParameterService
    {
        private readonly Regex _df;
        private readonly Regex _ie;
        private readonly Regex _oe;
        private readonly Regex _i;
        private readonly Regex _o;
        private readonly Regex _codePageRegex;

        /// <summary>
        /// Initialises a new instance of the ParameterService class.
        /// </summary>
        /// <param name="settings"><c>TextEncodingConverterSettings</c> instance.</param>
        public ParameterService(ITextEncodingConverterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this._df = new Regex("^/[df]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this._ie = new Regex("^/ie\\:", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this._oe = new Regex("^/oe\\:", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this._i = new Regex("^/i\\:", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this._o = new Regex("^/o\\:", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            this._codePageRegex = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Initialises a new instance of the ParameterService class.
        /// </summary>
        /// <param name="args">Arguments taken from the application.</param>
        /// <param name="settings"><c>TextEncodingConverterSettings</c> instance.</param>
        public ParameterService(IEnumerable<string> args, ITextEncodingConverterSettings settings)
            : this(settings)
        {
            this.Args = args ?? new List<string>();
        }

        /// <summary>
        /// Gets or sets the list of arguments.
        /// </summary>
        public IEnumerable<string> Args { get; set; }

        private IEnumerable<EncodingInfoDataContainer> _encodings;

        /// <summary>
        /// Gets the list of encoding information instances.
        /// </summary>
        public IEnumerable<EncodingInfoDataContainer> Encodings
        {
            get
            {
                if (this._encodings == null || !this._encodings.Any())
                {
                    this._encodings = Encoding.GetEncodings()
                                              .Select(p => new EncodingInfoDataContainer()
                                              {
                                                  CodePage = p.CodePage,
                                                  Name = p.Name,
                                                  DisplayName = p.DisplayName
                                              })
                                              .OrderBy(p => p.Name);
                }
                return this._encodings;
            }
        }

        /// <summary>
        /// Validates the parameter inputs.
        /// </summary>
        /// <returns>Returns <c>True</c>, if all parameters are valid; otherwise returns <c>False</c>.</returns>
        public bool Validate()
        {
            if (this.Args == null || !this.Args.Any())
            {
                return false;
            }

            var df = this.Args.Any(p => this._df.IsMatch(p));
            if (!df)
            {
                return false;
            }

            var ie = this.Args.Any(p => this._ie.IsMatch(p));
            if (!ie)
            {
                return false;
            }

            var oe = this.Args.Any(p => this._oe.IsMatch(p));
            if (!oe)
            {
                return false;
            }

            var i = this.Args.Any(p => this._i.IsMatch(p));
            if (!i)
            {
                return false;
            }
            var o = this.Args.Any(p => this._o.IsMatch(p));
            if (!o)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the input parameter.
        /// </summary>
        /// <returns>Returns the input parameter.</returns>
        public ParameterInfoDataContainer GetInput()
        {
            var param = new ParameterInfoDataContainer();

            var source = this.Args.FirstOrDefault(p => p.ToLower().StartsWith("/i:"));
            if (String.IsNullOrWhiteSpace(source))
            {
                return param;
            }

            var conversionType = this.GetConversioinType();
            switch (conversionType)
            {
                case ConversionType.Directory:
                    param.Directories = source.Replace("/i:", "")
                                              .Replace("\"", "")
                                              .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                              .ToList();
                    break;

                case ConversionType.File:
                    param.Files = source.Replace("/i:", "")
                                        .Replace("\"", "")
                                        .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                        .ToList();
                    break;

                default:
                    throw new InvalidOperationException("ConversionType must be either Directory or File");
            }

            param.EncodingInfo = this.GetInputEncoding();
            return param;
        }

        /// <summary>
        /// Gets the output parameter.
        /// </summary>
        /// <returns>Returns the output parameter.</returns>
        public ParameterInfoDataContainer GetOutput()
        {
            var param = new ParameterInfoDataContainer();

            var source = this.Args.FirstOrDefault(p => p.ToLower().StartsWith("/o:"));
            if (String.IsNullOrWhiteSpace(source))
            {
                return param;
            }

            param.Directories = new List<string>()
                                        {
                                            source.Replace("/o:", "").Replace("\"", "")
                                        };

            param.EncodingInfo = this.GetOutputEncoding();
            return param;
        }

        /// <summary>
        /// Gets the conversion type.
        /// </summary>
        /// <returns>Returns the conversion type.</returns>
        public ConversionType GetConversioinType()
        {
            var conversionType = ConversionType.Unknown;

            if (this.Args.Any(p => p.ToLower() == "/d"))
            {
                conversionType = ConversionType.Directory;
            }
            else if (this.Args.Any(p => p.ToLower() == "/f"))
            {
                conversionType = ConversionType.File;
            }

            return conversionType;
        }

        /// <summary>
        /// Gets the input encoding information.
        /// </summary>
        /// <returns>Returns the input encoding information.</returns>
        public EncodingInfoDataContainer GetInputEncoding()
        {
            var encoding = this.Args.FirstOrDefault(p => p.ToLower().StartsWith("/ie:"));
            if (String.IsNullOrWhiteSpace(encoding))
            {
                return this.Encodings.Single(p => p.CodePage == 949);
            }

            encoding = encoding.Replace("/ie:", "");

            return this._codePageRegex.IsMatch(encoding)
                       ? this.Encodings.Single(p => p.CodePage == Int32.Parse(encoding))
                       : this.Encodings.Single(p => String.Equals(p.Name, encoding, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the output encoding information.
        /// </summary>
        /// <returns>Returns the output encoding information.</returns>
        public EncodingInfoDataContainer GetOutputEncoding()
        {
            var encoding = this.Args.FirstOrDefault(p => p.ToLower().StartsWith("/oe:"));
            if (String.IsNullOrWhiteSpace(encoding))
            {
                return this.Encodings.Single(p => p.CodePage == 65001);
            }

            encoding = encoding.Replace("/oe:", "");

            return this._codePageRegex.IsMatch(encoding)
                       ? this.Encodings.Single(p => p.CodePage == Int32.Parse(encoding))
                       : this.Encodings.Single(p => String.Equals(p.Name, encoding, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}