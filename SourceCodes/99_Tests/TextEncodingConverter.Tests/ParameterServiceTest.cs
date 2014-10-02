using Aliencube.TextEncodingConverter.Services;
using Aliencube.TextEncodingConverter.Services.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Aliencube.TextEncodingConverter.Tests
{
    [TestFixture]
    public class ParameterServiceTest
    {
        #region SetUp / TearDown

        private IParameterService _parameterService;

        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Dispose()
        {
            if (this._parameterService != null)
            this._parameterService.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase(true, "/d", "/ie:949", "/oe:utf-8", "/i:test", "/o:output")]
        [TestCase(false, "/ie:949", "/oe:utf-8", "/i:test", "/o:output")]
        [TestCase(false, "/d", "/oe:utf-8", "/i:test", "/o:output")]
        [TestCase(false, "/d", "/ie:949", "/i:test", "/o:output")]
        [TestCase(false, "/d", "/ie:949", "/oe:utf-8", "/o:output")]
        [TestCase(false, "/d", "/ie:949", "/oe:utf-8", "/i:test")]
        public void GetValidated_GivenArgs_ReturnValidated(bool expected, params string[] args)
        {
            this._parameterService = new ParameterService(args);
            var validated = this._parameterService.Validate();

            validated.Should().Be(expected);
        }

        [Test]
        [TestCase("/d", "/ie:949", "/oe:utf-8", "/i:test", "/o:output")]
        public void GetInputParameter_GivenArgs_ReturnInputParameter(params string[] args)
        {
            this._parameterService = new ParameterService(args);
            var param = this._parameterService.GetInput();

            Assert.AreEqual(param.EncodingInfo.CodePage.ToString(), args.Single(p => p.StartsWith("/ie:")).Replace("/ie:", ""));
            Assert.AreEqual(param.Directories.First(), args.Single(p => p.StartsWith("/i:")).Replace("/i:", ""));
        }

        [Test]
        [TestCase("/d", "/ie:949", "/oe:utf-8", "/i:test", "/o:output")]
        public void GetOutputParameter_GivenArgs_ReturnOutputParameter(params string[] args)
        {
            this._parameterService = new ParameterService(args);
            var param = this._parameterService.GetOutput();

            Assert.AreEqual(param.EncodingInfo.Name, args.Single(p => p.StartsWith("/oe:")).Replace("/oe:", ""));
            Assert.AreEqual(param.Directories.First(), args.Single(p => p.StartsWith("/o:")).Replace("/o:", ""));
        }

        [Test]
        [TestCase("/d", "/ie:949", "/oe:utf-8", "/i:test", "/o:output")]
        public void GetConversionType_GivenArgs_ReturnConversionType(params string[] args)
        {
            this._parameterService = new ParameterService(args);
            var conversionType = this._parameterService.GetConversioinType();

            Assert.AreEqual(conversionType, ConversionType.Directory);
        }

        #endregion Tests
    }
}