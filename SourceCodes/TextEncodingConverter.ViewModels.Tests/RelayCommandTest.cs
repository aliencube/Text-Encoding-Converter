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
            var sut = new RelayCommand(x => false, x => { });
            Assert.IsInstanceOf<ICommand>(sut);
        }

        [Test]
        public void InitializeWithNullOnCanExecuteThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null, x => { }));
        }

        [Test]
        public void InitializeWithNullOnExecuteThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(x => false, null));
        }

        [Test]
        public void OnCanExecuteIsCorrect()
        {
            Predicate<object> onCanExecute = x => false;
            var sut = new RelayCommand(onCanExecute, x => { });

            var actual = sut.OnCanExecute;

            Assert.AreEqual(onCanExecute, actual);
        }

        [Test]
        public void OnExecuteIsCorrect()
        {
            Action<object> onExecute = x => { };
            var sut = new RelayCommand(x => false, onExecute);

            var actual = sut.OnExecute;

            Assert.AreEqual(onExecute, actual);
        }

        [Test]
        public void CanExecuteCallsOnCanExecute()
        {
            var parameter = new object();
            var expected = true;
            Predicate<object> onCanExecute =
                x =>
                {
                    Assert.AreEqual(parameter, x);
                    return expected;
                };
            var sut = new RelayCommand(onCanExecute, x => { });

            var actual = sut.CanExecute(parameter);

            Assert.AreEqual(expected, actual);
        }
    }
}