using System.Windows.Input;
using Aliencube.TextEncodingConverter.ViewModels;
using NUnit.Framework;

namespace TextEncodingConverter.ViewModels.Tests
{
    [TestFixture]
    public class RelayCommandTest
    {
        [Test]
        public void SutIsCommand()
        {
            var sut = new RelayCommand();
            Assert.IsInstanceOf<ICommand>(sut);
        }
    }
}