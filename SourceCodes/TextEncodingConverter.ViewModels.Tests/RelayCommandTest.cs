using System;
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
            var sut = new RelayCommand(null, null);
            Assert.IsInstanceOf<ICommand>(sut);
        }

        [Test]
        public void OnCanExecuteIsCorrect()
        {
            Predicate<object> onCanExecute = x => false;
            var sut = new RelayCommand(onCanExecute, null);

            var actual = sut.OnCanExecute;

            Assert.AreEqual(onCanExecute, actual);
        }
    }
}