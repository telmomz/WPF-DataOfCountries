namespace WPFUI.ViewModels
{
    ///References
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using WPFUI.Models;
    using WPFUI.Services;

    /// <summary>
    /// Flag user control page
    /// </summary>
    public class FlagPageViewModel : Screen
    {
        #region Atributes

        /// <summary>
        /// Private Atribute Network Service
        /// </summary>
        private NetworkService networkService;

        /// <summary>
        /// Selected country
        /// </summary>
        private Country _selected;

        /// <summary>
        /// Bitmap image
        /// </summary>
        private BitmapImage _countryFlag;

        /// <summary>
        /// Latitude
        /// </summary>
        private string lat;

        /// <summary>
        /// Longeti
        /// </summary>
        private string longtd;

        /// <summary>
        /// Visibility
        /// </summary>
        private string _visible;

        #endregion

        #region Propreties

        /// <summary>
        /// Country selected
        /// </summary>
        public Country Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;

                if (Selected != null)
                {
                    ShowFlag();
                    Cordenadas();
                    Validation();
                    NotifyOfPropertyChange(() => Selected);
                }
            }
        }

        /// <summary>
        /// Bitmap image
        /// </summary>
        public BitmapImage CountryFlag
        {
            get
            {
                return _countryFlag;
            }
            set
            {
                _countryFlag = value;
            }
        }

        /// <summary>
        /// Coordinates
        /// </summary>
        public string Cords
        {
            get
            {
                return $"{lat}, {longtd}";
            }
        }
        
               
        /// <summary>
        /// Visibility
        /// </summary>
        public string Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                NotifyOfPropertyChange(() => Visible);
            }
        }

        /// <summary>
        /// Page Active
        /// </summary>
        public static bool PageActive { get; set; }

        #endregion

        /// <summary>
        /// Flag page initializer
        /// </summary>
        /// <param name="country">Class country</param>
        public FlagPageViewModel(Country country, NetworkService networkService)
        {
            this.networkService = networkService;
            this.Selected       = country;

            PageActive          = true;
        }

        #region Methods

        /// <summary>
        /// Shows selected flag
        /// </summary>
        private void ShowFlag()
        {
            CountryFlag = DataAccess.getFlag(Selected);
            NotifyOfPropertyChange(() => CountryFlag);
        }

        /// <summary>
        /// Cordenadas
        /// </summary>
        private void Cordenadas()
        {
            Map();
            
            try
            {
                lat     = Selected.latlng[0].ToString();
                longtd  = Selected.latlng[1].ToString();
            }
            catch (Exception)
            {
                lat     = "33,00";
                longtd  = "-33,00";
            }

        }

        /// <summary>
        /// Show/Hide Map
        /// </summary>
        private async void Map()
        {
            Response con = await networkService.CheckConnection();
            if (!con.IsSucess)
            {
                Visible = "Hidden";
            }
            else
            {
                Visible = "Visible";
            }
        }

        /// <summary>
        /// Validation
        /// </summary>
        private void Validation()
        {
            if (string.IsNullOrEmpty(Selected.capital))
                Selected.capital = "Data unavailable";
        }

        #endregion

    }
}
