namespace Aliencube.TextEncodingConverter.Services
{
    /// <summary>
    /// This specifies the conversion type.
    /// </summary>
    public enum ConversionType
    {
        /// <summary>
        /// Identifies no conversion type is determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Identifies all files in a directory is to be converted.
        /// </summary>
        Directory = 1,

        /// <summary>
        /// Identifies individual file is to be converted.
        /// </summary>
        File = 2
    }
}