using System;
using System.Text.RegularExpressions;
using Aliencube.TextEncodingConverter.ViewModels;

namespace Aliencube.TextEncodingConverter.Services.Interfaces
{
    public interface IParameterService : IDisposable
    {
        /// <summary>
        /// Gets the regular expression instance to filter codepage.
        /// </summary>
        Regex CodePageRegex { get; }

        /// <summary>
        /// Gets the input parameter.
        /// </summary>
        /// <returns>Returns the input parameter.</returns>
        ParameterInfoViewModel GetInput();

        /// <summary>
        /// Gets the output parameter.
        /// </summary>
        /// <returns>Returns the output parameter.</returns>
        ParameterInfoViewModel GetOutput();

        /// <summary>
        /// Gets the conversion type.
        /// </summary>
        /// <returns>Returns the conversion type.</returns>
        ConversionType GetConversioinType();

        /// <summary>
        /// Gets the input encoding information.
        /// </summary>
        /// <returns>Returns the input encoding information.</returns>
        EncodingInfoViewModel GetInputEncoding();

        /// <summary>
        /// Gets the output encoding information.
        /// </summary>
        /// <returns>Returns the output encoding information.</returns>
        EncodingInfoViewModel GetOutputEncoding();
    }
}