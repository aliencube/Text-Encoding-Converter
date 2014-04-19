using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliencube.TextEncodingConverter.ViewModels;

namespace Aliencube.TextEncodingConverter.Services
{
    public class ConverterService
    {
        private IList<EncodingInfoViewModel> _encodings;
 
        public IList<EncodingInfoViewModel> Encodings
        {
            get
            {
                if (this._encodings == null || !this._encodings.Any())
                {
                    this._encodings = Encoding.GetEncodings()
                                              .Select(p => new EncodingInfoViewModel()
                                                           {
                                                               CodePage = p.CodePage,
                                                               Name = p.Name,
                                                               DisplayName = p.DisplayName
                                                           })
                                              .ToList();
                }
                return this._encodings;
            }
        }

        public EncodingInfoViewModel InputEncoding { get; set; }

        public EncodingInfoViewModel OutputEncoding { get; set; }

        public string InputDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public string InputFile { get; set; }

        public string OutputFile { get; set; }

        public void Convert()
        {
        }
    }
}
