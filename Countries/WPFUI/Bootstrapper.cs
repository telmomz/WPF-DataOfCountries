namespace WPFUI
{
    using Caliburn.Micro;
    using System.Windows;
    using WPFUI.ViewModels;

    /// <summary>
    /// Initializing the window
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// BootstrapperBase module initialiezer 
        /// </summary>
        public Bootstrapper()
        {
            Initialize();
        }
       
        /// <summary>
        /// Starts View Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

    }
}
