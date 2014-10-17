using Aliencube.TextEncodingConverter.Configs;
using Aliencube.TextEncodingConverter.Configs.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Aliencube.TextEncodingConverter.Tests
{
    [TestFixture]
    public class ConfigSettingsTest
    {
        #region SetUp / TearDown

        private ITextEncodingConverterSettings _settings;

        [SetUp]
        public void Init()
        {
            this._settings = TextEncodingConverterSettings.CreateInstance();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._settings != null)
            {
                this._settings.Dispose();
            }
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase("ks_c_5601-1987", "utf-8", "csv,txt", true, "Backup", "Output")]
        public void GetSettings_GivenAppConfig_ReturnSettings(string input, string output, string extensions, bool backup, string backupPath, string outputPath)
        {
            var encoding = this._settings.Encoding;
            encoding.Input.Should().Be(input);
            encoding.Output.Should().Be(output);

            var converter = this._settings.Converter;
            converter.Extensions.Should().Be(extensions);
            converter.Backup.Should().Be(backup);
            converter.BackupPath.Should().Be(backupPath);
            converter.OutputPath.Should().Be(outputPath);
        }

        #endregion Tests
    }
}