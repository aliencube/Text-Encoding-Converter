using Microsoft.Practices.ServiceLocation;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    /// <summary>
    /// This represents the static entity for view model locator.
    /// </summary>
    public static class ViewModelLocator
    {
        /// <summary>
        /// Gets the instance of the given type.
        /// </summary>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Returns the instnance of the given type.</returns>
        private static T GetInstance<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }

        /// <summary>
        /// Gets the main window view model instance.
        /// </summary>
        public static MainWindowViewModel MainWindowViewModel
        {
            get { return GetInstance<MainWindowViewModel>(); }
        }
    }
}