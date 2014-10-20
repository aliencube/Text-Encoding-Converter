using Aliencube.TextEncodingConverter.Configs.Interfaces;
using Aliencube.TextEncodingConverter.DataContainers;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly ITextEncodingConverterSettings _settings;
        private readonly IParameterService _parameterService;

        /// <summary>
        /// Initialises a new instance of the ConverterService class.
        /// </summary>
        /// <param name="settings"><c>TextEncodingConverterSettings</c> instance.</param>
        /// <param name="parameterService"><c>ParameterService</c> instance.</param>
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
        public ConverterService(ITextEncodingConverterSettings settings, IParameterService parameterService)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            this._settings = settings;

            if (parameterService == null)
            {
                throw new ArgumentNullException("parameterService");
            }
            this._parameterService = parameterService;
        }

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

        private ParameterInfoDataContainer _input;

        /// <summary>
        /// Gets the input parameters.
        /// </summary>
        public ParameterInfoDataContainer Input
        {
            get
            {
                if (this._input == null)
                {
                    this._input = this._parameterService.GetInput();
                }

                return this._input;
            }
        }

        private ParameterInfoDataContainer _output;

        /// <summary>
        /// Gets the output parameters.
        /// </summary>
        public ParameterInfoDataContainer Output
        {
            get
            {
                if (this._output == null)
                {
                    this._output = this._parameterService.GetOutput();
                }

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
            {
                return false;
            }

            var extension = inputFile.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last().ToLower();
            var extensions = this._settings.Converter.Extensions.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
            {
                return path;
            }

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (String.IsNullOrWhiteSpace(directory))
            {
                return path;
            }

            path = String.Format(path.StartsWith("\\") ? "{0}{1}" : "{0}\\{1}",
                                 directory.TrimEnd('\\'),
                                 path.TrimEnd('\\'));

            return path;
        }

        /// <summary>
        /// Performs the conversion from one encoding to another.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <param name="displayUsage">Value that specifies whether to display the usage instruction or not.</param>
        /// <returns>Returns <c>True</c>, if conversion is successful; otherwise returns <c>False</c>.</returns>
        public bool Convert(IEnumerable<string> args, bool displayUsage = true)
        {
            this._parameterService.Args = args ?? new List<string>();
            return this.Convert(displayUsage);
        }

        /// <summary>
        /// Performs the conversion from one encoding to another.
        /// </summary>
        /// <param name="displayUsage">Value that specifies whether to display the usage instruction or not.</param>
        /// <returns>Returns <c>True</c>, if conversion is successful; otherwise returns <c>False</c>.</returns>
        public bool Convert(bool displayUsage = true)
        {
            var result = false;
            if (!this._parameterService.Validate())
            {
                if (displayUsage)
                {
                    this.DisplayUsage();
                }

                return result;
            }

            try
            {
                if (this.Input.Directories != null && this.Input.Directories.Any())
                {
                    Parallel.ForEach(this.Input.Directories, p => this.ConvertFilesInDirectory(p, this.Output.Directories.First()));
                }

                if (this.Input.Files != null && this.Input.Files.Any())
                {
                    Parallel.ForEach(this.Input.Files, p => this.ConvertFile(p, this.Output.Directories.First()));
                }

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Performs the file backup.
        /// </summary>
        /// <param name="files">List of files.</param>
        /// <returns>Returns <c>True</c>, if the backup is successful; otherwise returns <c>False</c>.</returns>
        public bool Backup(IEnumerable<string> files)
        {
            var backup = this._settings.Converter.BackupPath;
            if (!Directory.Exists(backup))
            {
                Directory.CreateDirectory(backup);
            }

            bool result;
            try
            {
                Parallel.ForEach(files, p =>
                                        {
                                            var fi = new FileInfo(p);
                                            var file = String.Format("{0}\\{1}", backup, fi.Name);
                                            if (File.Exists(file))
                                            {
                                                File.Delete(file);
                                            }
                                            File.Copy(p, file);
                                        });

                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Displays the usage screen.
        /// </summary>
        private void DisplayUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    Aliencube.TextEncodingConverter.exe /d|/f /ie:WWW /oe:XXX /i:YYY /o:ZZZ");
            Console.WriteLine();
            Console.WriteLine("    /d|/f      Indicates whether the input is directory(/d) or file(/f)");
            Console.WriteLine("    /ie:WWW    Specifies the input file encoding");
            Console.WriteLine("    /oe:XXX    Specifies the output file encoding");
            Console.WriteLine("    /i:YYY     Specifies the input directory/file path");
            Console.WriteLine("    /o:ZZZ     Specifies the output directpry path");
            Console.WriteLine();
            Console.WriteLine("    NOTE: Either YYY or ZZZ can be wrapped with quotation marks for some cases");
        }

        /// <summary>
        /// Converts files in a given directory.
        /// </summary>
        /// <param name="inputDirectory">Input directory.</param>
        /// <param name="outputDirectory">Output directory.</param>
        private void ConvertFilesInDirectory(string inputDirectory, string outputDirectory)
        {
            var directory = this.GetQualifiedPath(inputDirectory);
            var files = Directory.GetFiles(directory);
            if (files.Any())
            {
                Parallel.ForEach(files, p => this.ConvertFile(p, outputDirectory));
            }
        }

        /// <summary>
        /// Convers a file.
        /// </summary>
        /// <param name="inputFile">Input file path.</param>
        /// <param name="outputDirectory">Output directory.</param>
        private void ConvertFile(string inputFile, string outputDirectory)
        {
            if (!this.IsValidFile(inputFile))
            {
                return;
            }

            if (!File.Exists(inputFile))
            {
                return;
            }

            var inputPath = this.GetQualifiedPath(inputFile);
            var inputCodepage = this.Input.EncodingInfo.CodePage;
            var inputCodename = this.Input.EncodingInfo.Name;

            if (!inputCodepage.HasValue && String.IsNullOrWhiteSpace(inputCodename))
            {
                throw new ApplicationException("Either codepage or codename for input must be specified for conversion");
            }

            var inputEncoding = inputCodepage.HasValue
                                    ? Encoding.GetEncoding(inputCodepage.Value)
                                    : Encoding.GetEncoding(inputCodename);

            using (var reader = new StreamReader(inputPath, inputEncoding))
            {
                var directory = this.GetQualifiedPath(outputDirectory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var outputPath = String.Format("{0}\\{1}",
                                               directory,
                                               inputFile.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last());
                var outputCodepage = this.Output.EncodingInfo.CodePage;
                var outputCodename = this.Output.EncodingInfo.Name;

                if (!outputCodepage.HasValue && String.IsNullOrWhiteSpace(outputCodename))
                {
                    throw new ApplicationException("Either codepage or codename for output must be specified for conversion");
                }

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