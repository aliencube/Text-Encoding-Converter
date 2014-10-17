using System.Configuration;

namespace Aliencube.TextEncodingConverter.Configs
{
    /// <summary>
    /// This represents the encoding element entity.
    /// </summary>
    public class EncodingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the input encoding value. Default value is <c>ks_c_5601-1987</c>.
        /// </summary>
        [ConfigurationProperty("input", IsRequired = false, DefaultValue = "ks_c_5601-1987")]
        public string Input
        {
            get { return (string)this["input"]; }
            set { this["input"] = value; }
        }

        /// <summary>
        /// Gets or sets the output encoding value. Default value is <c>utf-8</c>.
        /// </summary>
        [ConfigurationProperty("output", IsRequired = false, DefaultValue = "utf-8")]
        public string Output
        {
            get { return (string)this["output"]; }
            set { this["output"] = value; }
        }
    }
}