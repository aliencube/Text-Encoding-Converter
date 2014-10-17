using Aliencube.TextEncodingConverter.DataContainers;
using System;
using System.Collections.Generic;

namespace Aliencube.TextEncodingConverter.Services.Interfaces
{
    /// <summary>
    /// This provides interface to the <c>ConverterService</c> class.
    /// </summary>
    public interface IConverterService : IDisposable
    {
        /// <summary>
        /// Gets the list of encoding information instances.
        /// </summary>
        IEnumerable<EncodingInfoDataContainer> Encodings { get; }

        /// <summary>
        /// Gets the input parameters.
        /// </summary>
        ParameterInfoDataContainer Input { get; }

        /// <summary>
        /// Gets the output parameters.
        /// </summary>
        ParameterInfoDataContainer Output { get; }

        /// <summary>
        /// Checks whether the extension of the input file is valid or not.
        /// </summary>
        /// <param name="inputFile">Input file path.</param>
        /// <returns>Returns <c>True</c>, if the extension of the input file is valid; otherwise returns <c>False</c>.</returns>
        bool IsValidFile(string inputFile);

        /// <summary>
        /// Gets the fully qualified directory or file path.
        /// </summary>
        /// <param name="path">Directory or file path.</param>
        /// <returns>Returns the fully qualified directory or path.</returns>
        string GetQualifiedPath(string path);

        /// <summary>
        /// Performs the conversion from one encoding to another.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <param name="displayUsage">Value that specifies whether to display the usage instruction or not.</param>
        /// <returns>Returns <c>True</c>, if conversion is successful; otherwise returns <c>False</c>.</returns>
        bool Convert(IEnumerable<string> args, bool displayUsage = true);

        /// <summary>
        /// Perform the conversion from one encoding to another.
        /// </summary>
        /// <param name="displayUsage">Value that specifies whether to display the usage instruction or not.</param>
        /// <returns>Returns <c>True</c>, if conversion is successful; otherwise returns <c>False</c>.</returns>
        bool Convert(bool displayUsage = true);

        /// <summary>
        /// Performs the file backup.
        /// </summary>
        /// <param name="files">List of files.</param>
        /// <returns>Returns <c>True</c>, if the backup is successful; otherwise returns <c>False</c>.</returns>
        bool Backup(IEnumerable<string> files);
    }
}