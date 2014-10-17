using System.Configuration;

namespace Aliencube.TextEncodingConverter.Configs
{
    /// <summary>
    /// This represents the converter element entity.
    /// </summary>
    public class ConverterElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the comma delimited extensions value. Default value is <c>csv,txt</c>.
        /// </summary>
        [ConfigurationProperty("extensions", IsRequired = false, DefaultValue = "csv,txt")]
        public string Extensions
        {
            get { return (string)this["extensions"]; }
            set { this["extensions"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to backup the original files or not. Default value is <c>True</c>.
        /// </summary>
        [ConfigurationProperty("backup", IsRequired = false, DefaultValue = true)]
        public bool Backup
        {
            get { return (bool)this["backup"]; }
            set { this["backup"] = value; }
        }

        /// <summary>
        /// Gets or sets the backup path value. If <c>Backup</c> is <c>False</c>, this will be ignored. Default value is <c>Backup</c>.
        /// </summary>
        [ConfigurationProperty("backupPath", IsRequired = false, DefaultValue = "Backup")]
        public string BackupPath
        {
            get { return (string)this["backupPath"]; }
            set { this["backupPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the output path value. Default value is <c>Output</c>.
        /// </summary>
        [ConfigurationProperty("outputPath", IsRequired = false, DefaultValue = "Output")]
        public string OutputPath
        {
            get { return (string)this["outputPath"]; }
            set { this["outputPath"] = value; }
        }
    }
}