using Aliencube.TextEncodingConverter.ViewModels;
using System;
using System.Collections.Generic;

namespace Aliencube.TextEncodingConverter.Services.Interfaces
{
    public interface IConverterService : IDisposable
    {
        /// <summary>
        /// Gets the list of encoding information instances.
        /// </summary>
        IList<EncodingInfoViewModel> Encodings { get; }

        /// <summary>
        /// Gets or sets the input parameters.
        /// </summary>
        ParameterInfoViewModel Input { get; }

        /// <summary>
        /// Gets or sets the output parameters.
        /// </summary>
        ParameterInfoViewModel Output { get; }

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
        /// Perform the conversion from one encoding to another.
        /// </summary>
        void Convert();

        /// <summary>
        /// Displays the usage screen.
        /// </summary>
        void DisplayUsage();

        /// <summary>
        /// Converts files in a given directory.
        /// </summary>
        /// <param name="inputDirectory">Input directory.</param>
        /// <param name="outputDirectory">Output directory.</param>
        void ConvertFilesInDirectory(string inputDirectory, string outputDirectory);

        /// <summary>
        /// Convers a file.
        /// </summary>
        /// <param name="inputFile">Input file path.</param>
        /// <param name="outputDirectory">Output directory.</param>
        void ConvertFile(string inputFile, string outputDirectory);
    }
}