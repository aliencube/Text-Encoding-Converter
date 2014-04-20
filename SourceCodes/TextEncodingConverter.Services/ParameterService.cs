using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aliencube.TextEncodingConverter.Services
{
    /// <summary>
    /// This represents the parameter service entity.
    /// </summary>
    public class ParameterService : IParameterService
    {
        private readonly IList<string> _args;

        /// <summary>
        /// Initialises a new instance of the ParameterService class.
        /// </summary>
        /// <param name="args">Arguments taken from the application.</param>
        public ParameterService(IEnumerable<string> args)
        {
            this._args = args.ToList();
        }

        private Regex _codePageRegex;

        /// <summary>
        /// Gets the regular expression instance to filter codepage.
        /// </summary>
        public Regex CodePageRegex
        {
            get
            {
                if (this._codePageRegex == null)
                    this._codePageRegex = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return this._codePageRegex;
            }
        }

        /// <summary>
        /// Gets the input parameter.
        /// </summary>
        /// <returns>Returns the input parameter.</returns>
        public ParameterInfoViewModel GetInput()
        {
            var param = new ParameterInfoViewModel();

            var source = this._args.FirstOrDefault(p => p.ToLower().StartsWith("/i:"));
            if (String.IsNullOrWhiteSpace(source))
                return param;

            var conversionType = this.GetConversioinType();
            switch (conversionType)
            {
                case ConversionType.Directory:
                    param.Directories = new List<string>()
                                        {
                                            source.Replace("/i:", "").Replace("\"", "")
                                        };
                    break;

                case ConversionType.File:
                    param.Files = new List<string>()
                                  {
                                      source.Replace("/i:", "").Replace("\"", "")
                                  };
                    break;

                default:
                    throw new InvalidOperationException("ConversionType must be either Directory or File");
                    break;
            }

            param.EncodingInfo = this.GetInputEncoding();
            return param;
        }

        /// <summary>
        /// Gets the output parameter.
        /// </summary>
        /// <returns>Returns the output parameter.</returns>
        public ParameterInfoViewModel GetOutput()
        {
            var param = new ParameterInfoViewModel();

            var source = this._args.FirstOrDefault(p => p.ToLower().StartsWith("/o:"));
            if (String.IsNullOrWhiteSpace(source))
                return param;

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

            if (this._args.Any(p => p.ToLower() == "/d"))
                conversionType = ConversionType.Directory;
            else if (this._args.Any(p => p.ToLower() == "/f"))
                conversionType = ConversionType.File;

            return conversionType;
        }

        /// <summary>
        /// Gets the input encoding information.
        /// </summary>
        /// <returns>Returns the input encoding information.</returns>
        public EncodingInfoViewModel GetInputEncoding()
        {
            var encoding = this._args.FirstOrDefault(p => p.ToLower().StartsWith("/ie:"));
            if (String.IsNullOrWhiteSpace(encoding))
                return new EncodingInfoViewModel() { CodePage = 949, Name = "ks_c_5601-1987" };

            encoding = encoding.Replace("/ie:", "");

            var ei = new EncodingInfoViewModel();
            if (this._codePageRegex.IsMatch(encoding))
                ei.CodePage = Int32.Parse(encoding);
            else
                ei.Name = encoding;

            return ei;
        }

        /// <summary>
        /// Gets the output encoding information.
        /// </summary>
        /// <returns>Returns the output encoding information.</returns>
        public EncodingInfoViewModel GetOutputEncoding()
        {
            var encoding = this._args.FirstOrDefault(p => p.ToLower().StartsWith("/oe:"));
            if (String.IsNullOrWhiteSpace(encoding))
                return new EncodingInfoViewModel() { CodePage = 65001, Name = "utf-8" };

            encoding = encoding.Replace("/ie:", "");

            var ei = new EncodingInfoViewModel();
            if (this._codePageRegex.IsMatch(encoding))
                ei.CodePage = Int32.Parse(encoding);
            else
                ei.Name = encoding;

            return ei;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}