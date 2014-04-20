using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using NUnit.Framework;

namespace Aliencube.TextEncodingConverter.Tests
{
    [TestFixture]
    public class ConverterServiceTest
    {
        #region SetUp / TearDown

        private IConverterService _converterService;

        [SetUp]
        public void Init()
        {
            this._converterService = new ConverterService();
        }

        [TearDown]
        public void Dispose()
        {
            this._converterService.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase("text.txt", true)]
        [TestCase("text.smi", true)]
        [TestCase("text.srt", true)]
        [TestCase("text.csv", false)]
        public void ValidateFilename_GivenFilename_ReturnResult(string filename, bool expected)
        {
            var result = this._converterService.IsValidFile(filename);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(@"c:\\temp\text.txt", false)]
        [TestCase(@"text.txt", true)]
        public void GetQualifiedPath_GivenPath_ReturnQualifiedPath(string path, bool expected)
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (String.IsNullOrWhiteSpace(assemblyDirectory))
                Assert.Fail();
            assemblyDirectory = assemblyDirectory.TrimEnd('\\');

            if (!path.Contains(":\\"))
                path = String.Format(path.StartsWith("\\") ? "{0}{1}" : "{0}\\{1}",
                                     assemblyDirectory,
                                     path);
            var qualifiedPath = this._converterService.GetQualifiedPath(path);
            var segments = qualifiedPath.Split(new string[] {"\\"}, StringSplitOptions.RemoveEmptyEntries);
            var qualifiedDirectory = String.Join("\\", segments.Take(segments.Length - 1)).TrimEnd('\\');

            Assert.AreEqual(expected, qualifiedDirectory.Equals(assemblyDirectory));
        }

        #endregion Tests
    }
}