namespace WPFUI.ViewModels
{
    ///References
    using Caliburn.Micro;
    using System;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// About user control page
    /// </summary>
    public class AboutPageViewModel : Screen
    {
        #region Propreties

        /// <summary>
        /// Bitmap image
        /// </summary>
        public BitmapImage About { get; set; }

        #endregion

        /// <summary>
        /// About page initializer
        /// </summary>
        public AboutPageViewModel()
        {
            About = new BitmapImage();

            About.BeginInit();
            About.UriSource = new Uri($@".\about.png", UriKind.Relative);
            About.CacheOption = BitmapCacheOption.OnLoad;
            About.EndInit();

            NotifyOfPropertyChange(() => About);
        }
    }
}
