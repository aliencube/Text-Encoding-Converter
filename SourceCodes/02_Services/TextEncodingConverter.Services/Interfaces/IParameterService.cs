using System;
using System.Text.RegularExpressions;
using Aliencube.TextEncodingConverter.DataContainers;

namespace Aliencube.TextEncodingConverter.Services.Interfaces
{
    public interface IParameterService : IDisposable
    {
        /// <summary>
        /// Gets the regular expression instance to filter codepage.
        /// </summary>
        Regex CodePageRegex { get; }

        /// <summary>
        /// Validates the parameter inputs.
        /// </summary>
        /// <returns>Returns <c>True</c>, if all parameters are valid; otherwise returns <c>False</c>.</returns>
        bool Validate();

        /// <summary>
        /// Gets the input parameter.
        /// </summary>
        /// <returns>Returns the input parameter.</returns>
        ParameterInfoDataContainer GetInput();

        /// <summary>
        /// Gets the output parameter.
        /// </summary>
        /// <returns>Returns the output parameter.</returns>
        ParameterInfoDataContainer GetOutput();

        /// <summary>
        /// Gets the conversion type.
        /// </summary>
        /// <returns>Returns the conversion type.</returns>
        ConversionType GetConversioinType();

        /// <summary>
        /// Gets the input encoding information.
        /// </summary>
        /// <returns>Returns the input encoding information.</returns>
        EncodingInfoDataContainer GetInputEncoding();

        /// <summary>
        /// Gets the output encoding information.
        /// </summary>
        /// <returns>Returns the output encoding information.</returns>
        EncodingInfoDataContainer GetOutputEncoding();
    }
}