using System;

namespace Aliencube.TextEncodingConverter.Configs.Interfaces
{
    public interface ITextEncodingConverterSettings : IDisposable
    {
        /// <summary>
        /// Gets or sets the encoding element.
        /// </summary>
        EncodingElement Encoding { get; set; }

        /// <summary>
        /// Gets or sets the converter element.
        /// </summary>
        ConverterElement Converter { get; set; }
    }
}