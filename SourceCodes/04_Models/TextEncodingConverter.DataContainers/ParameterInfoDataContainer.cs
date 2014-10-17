using System.Collections.Generic;

namespace Aliencube.TextEncodingConverter.DataContainers
{
    /// <summary>
    /// This represents the view model containing parameters information.
    /// </summary>
    public class ParameterInfoDataContainer
    {
        /// <summary>
        /// Gets or sets the encoding information.
        /// </summary>
        public EncodingInfoDataContainer EncodingInfo { get; set; }

        /// <summary>
        /// Gets or sets the list of directories.
        /// </summary>
        /// <remarks>
        /// Unless fully qualified directory path is specified, the directory path is considered as a subdirectory of the executable's path.
        /// </remarks>
        public IList<string> Directories { get; set; }

        /// <summary>
        /// Gets or sets the list of files.
        /// </summary>
        /// <remarks>
        /// Unless fully qualified file path is specified, the file path is considered the same as the executable's path.
        /// </remarks>
        public IList<string> Files { get; set; }
    }
}