using Aliencube.TextEncodingConverter.Services.Interfaces;
using Aliencube.TextEncodingConverter.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aliencube.TextEncodingConverter.Services
{
    /// <summary>
    /// This represents the converter service entity.
    /// </summary>
    public class ConverterService : IConverterService
    {
        private readonly IParameterService _parameterService;

        /// <summary>
        /// Initialises a new instance of the ConverterService class.
        /// </summary>
        /// <remarks>
        /// When initialised, default values of input and output are to be:
        /// <list type="bullet">
        ///     <item>
        ///         <term>Input CodePage:</term>
        ///         <description>949 (ks_c_5601-1987)</description>
        ///     </item>
        ///     <item>
        ///         <term>Output CodePage:</term>
        ///         <description>65001 (utf-8)</description>
        ///     </item>
        ///     <item>
        ///         <term>Output Directory:</term>
        ///         <description>./output</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ConverterService(IParameterService parameterService)
        {
            this._parameterService = parameterService;
        }

        private IList<EncodingInfoViewModel> _encodings;

        /// <summary>
        /// Gets the list of encoding information instances.
        /// </summary>
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

        private ParameterInfoViewModel _input;

        /// <summary>
        /// Gets or sets the input parameters.
        /// </summary>
        public ParameterInfoViewModel Input
        {
            get
            {
                if (this._input == null)
                    this._input = this._parameterService.GetInput();

                return this._input;
            }
        }

        private ParameterInfoViewModel _output;

        /// <summary>
        /// Gets or sets the output parameters.
        /// </summary>
        public ParameterInfoViewModel Output
        {
            get
            {
                if (this._output == null)
                    this._output = this._parameterService.GetOutput();

                return this._output;
            }
        }

        /// <summary>
        /// Checks whether the extension of the input file is valid or not.
        /// </summary>
        /// <param name="inputFile">Input file path.</param>
        /// <returns>Returns <c>True</c>, if the extension of the input file is valid; otherwise returns <c>False</c>.</returns>
        public bool IsValidFile(string inputFile)
        {
            if (String.IsNullOrWhiteSpace(inputFile))
                return false;

            var extension = inputFile.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last().ToLower();
            var extensions = ConfigurationManager.AppSettings["Extensions"].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return extensions.Select(p => p.ToLower()).Contains(extension);
        }

        /// <summary>
        /// Gets the fully qualified directory or file path.
        /// </summary>
        /// <param name="path">Directory or file path.</param>
        /// <returns>Returns the fully qualified directory or path.</returns>
        public string GetQualifiedPath(string path)
        {
            path = path.Replace("/", "\\");

            if (path.Contains(":\\"))
                return path;

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (String.IsNullOrWhiteSpace(directory))
                return path;

            path = String.Format(path.StartsWith("\\") ? "{0}{1}" : "{0}\\{1}",
                                 directory.TrimEnd('\\'),
                                 path.TrimEnd('\\'));

            return path;
        }

        /// <summary>
        /// Perform the conversion from one encoding to another.
        /// </summary>
        public void Convert()
        {
            if (this.Input.Directories != null && this.Input.Directories.Any())
                Parallel.ForEach(this.Input.Directories, p => this.ConvertFilesInDirectory(p, this.Output.Directories.First()));

            if (this.Input.Files != null && this.Input.Files.Any())
                Parallel.ForEach(this.Input.Files, p => this.ConvertFile(p, this.Output.Directories.First()));
        }

        /// <summary>
        /// Converts files in a given directory.
        /// </summary>
        /// <param name="inputDirectory">Input directory.</param>
        /// <param name="outputDirectory">Output directory.</param>
        public void ConvertFilesInDirectory(string inputDirectory, string outputDirectory)
        {
            var directory = this.GetQualifiedPath(inputDirectory);
            var files = Directory.GetFiles(directory);
            if (files.Any())
                Parallel.ForEach(files, p => this.ConvertFile(p, outputDirectory));
        }

        /// <summary>
        /// Convers a file.
        /// </summary>
        /// <param name="inputFile">Input file path.</param>
        /// <param name="outputDirectory">Output directory.</param>
        public void ConvertFile(string inputFile, string outputDirectory)
        {
            if (!this.IsValidFile(inputFile))
                return;

            if (!File.Exists(inputFile))
                return;

            var inputPath = this.GetQualifiedPath(inputFile);
            var inputCodepage = this.Input.EncodingInfo.CodePage;
            var inputCodename = this.Input.EncodingInfo.Name;

            if (!inputCodepage.HasValue && String.IsNullOrWhiteSpace(inputCodename))
                throw new ApplicationException("Either codepage or codename for input must be specified for conversion");

            var inputEncoding = inputCodepage.HasValue
                                    ? Encoding.GetEncoding(inputCodepage.Value)
                                    : Encoding.GetEncoding(inputCodename);

            using (var reader = new StreamReader(inputPath, inputEncoding))
            {
                var outputPath = String.Format("{0}\\{1}",
                                               this.GetQualifiedPath(outputDirectory),
                                               inputFile.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last());
                var outputCodepage = this.Output.EncodingInfo.CodePage;
                var outputCodename = this.Output.EncodingInfo.Name;

                if (!outputCodepage.HasValue && String.IsNullOrWhiteSpace(outputCodename))
                    throw new ApplicationException("Either codepage or codename for output must be specified for conversion");

                var outputEncoding = outputCodepage.HasValue
                                         ? Encoding.GetEncoding(outputCodepage.Value)
                                         : Encoding.GetEncoding(outputCodename);

                using (var writer = new StreamWriter(outputPath, false, outputEncoding))
                {
                    writer.Write(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}