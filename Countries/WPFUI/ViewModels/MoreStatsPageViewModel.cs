namespace WPFUI.ViewModels
{
    ///References
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using WPFUI.Models;

    /// <summary>
    /// More Stats Page
    /// </summary>
    public class MoreStatsPageViewModel : Screen
    {
        #region Atributes

        /// <summary>
        /// Selected country
        /// </summary>
        private Country _selected;

        /// <summary>
        /// System.String Top Level Domain
        /// </summary>
        private string _gini;

        /// <summary>
        /// System.String Region
        /// </summary>
        private string _region;

        /// <summary>
        /// System.String Subregion
        /// </summary>
        private string _subregion;

        /// <summary>
        /// Collection.String Top Level Domain
        /// </summary>
        private List<string> _topLevelDomain;

        /// <summary>
        /// Collection.String Calling Codes
        /// </summary>
        private List<string> _callingCodes;

        #endregion

        #region Propriedades

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
                NotifyOfPropertyChange(() => Selected);
            }
        }

        /// <summary>
        /// System.String Gini
        /// </summary>
        public string Gini
        {
            get
            {
                return _gini;
            }
            set
            {
                _gini = value;
                NotifyOfPropertyChange(() => Gini);
            }
        }

        /// <summary>
        /// System.String Region
        /// </summary>
        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                NotifyOfPropertyChange(() => Region);
            }
        }

        /// <summary>
        /// System.String Subregion
        /// </summary>
        public string Subregion
        {
            get { return _subregion; }
            set
            {
                _subregion = value;
                NotifyOfPropertyChange(() => Subregion);
            }
        }

        /// <summary>
        /// Collection.String Top Level Domain
        /// </summary>
        public List<string> TopLevelDomain
        {
            get
            {
                return _topLevelDomain;
            }
            set
            {
                _topLevelDomain = value;
                NotifyOfPropertyChange(() => TopLevelDomain);
            }
        }

        /// <summary>
        /// Collection.String Calling Codes
        /// </summary>
        public List<string> CallingCodes
        {
            get
            {
                return _callingCodes;
            }
            set
            {
                _callingCodes = value;
                NotifyOfPropertyChange(() => CallingCodes);
            }
        }

        /// <summary>
        /// System.Boolean Page Active
        /// </summary>
        public static bool PageActive { get; set; }

        #endregion

        /// <summary>
        /// More Stats Page Initializer
        /// </summary>
        /// <param name="selectCountry">System.Object Country</param>
        public MoreStatsPageViewModel(Country selectCountry)
        {
            Validation(selectCountry);

            this._selected = selectCountry;

            PageActive = true;
        }

        #region Methods

        /// <summary>
        /// Validation of fields
        /// </summary>
        /// <param name="selectCountry">Class country</param>
        private void Validation(Country selectCountry)
        {
            _topLevelDomain  = new List<string>();
            _callingCodes    = new List<string>();

            string r = "(none)";

            //gini
            if (selectCountry.gini == 100 || string.IsNullOrEmpty(selectCountry.gini.ToString()))
            {
                _gini       = "No data avaliable";
            }
            //region
            else if (selectCountry.region == r || string.IsNullOrEmpty(selectCountry.region))
            {
                _region     = "No data avaliable";
            }
            //subregion
            else if (selectCountry.subregion == r || string.IsNullOrEmpty(selectCountry.subregion))
            {
                _subregion  = "No data avaliable";
            }
            else
            {
                _gini   = selectCountry.gini.ToString();
                _region = selectCountry.region;
            }
            //topLevelDomain
            foreach (var item in selectCountry.topLevelDomain)
            {
                string n = "none";

                if(item == n || item == null)
                {
                    _topLevelDomain.Add("No data avaliable");
                }
                else
                {
                    _topLevelDomain.Add(item);
                }
            }
            //callingCodes
            foreach (var item in selectCountry.callingCodes)
            {
                string n = "none";

                if (item == n || item == null)
                {
                    _callingCodes.Add("No data avaliable");
                }
                else
                {
                    string aux = $"+{item}";
                    _callingCodes.Add(aux);
                }
            }

        }

        #endregion
    }
}
