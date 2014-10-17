using Aliencube.TextEncodingConverter.Configs.Interfaces;
using System.Configuration;

namespace Aliencube.TextEncodingConverter.Configs
{
    /// <summary>
    /// This represents the <c>ConfigurationSection</c> element entity for <c>TextEncodingConverterSettings</c>.
    /// </summary>
    public class TextEncodingConverterSettings : ConfigurationSection, ITextEncodingConverterSettings
    {
        /// <summary>
        /// Gets or sets the encoding element.
        /// </summary>
        [ConfigurationProperty("encoding", IsRequired = true)]
        public EncodingElement Encoding
        {
            get { return (EncodingElement)this["encoding"]; }
            set { this["encoding"] = value; }
        }

        /// <summary>
        /// Gets or sets the converter element.
        /// </summary>
        [ConfigurationProperty("converter", IsRequired = true)]
        public ConverterElement Converter
        {
            get { return (ConverterElement)this["converter"]; }
            set { this["converter"] = value; }
        }

        /// <summary>
        /// Creates the <c>TextEncodingConverterSettings</c> instance.
        /// </summary>
        /// <returns>Returns the <c>TextEncodingConverterSettings</c> instance.</returns>
        public static TextEncodingConverterSettings CreateInstance()
        {
            var settings = ConfigurationManager.GetSection("textEncodingConverterSettings") as TextEncodingConverterSettings;
            return settings;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}