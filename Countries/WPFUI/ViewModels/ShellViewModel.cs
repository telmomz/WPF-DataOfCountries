namespace WPFUI.ViewModels
{
    ///References
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using WPFUI.Models;
    using WPFUI.Services;

    /// <summary>
    /// MainWindow
    /// </summary>
    public class ShellViewModel : Conductor<object>
    {
        #region Atributes

        // Gets a NumberFormatInfo associated with the en-US culture.
        //NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        /// <summary>
        /// Private Atribute Network Service
        /// </summary>
        private NetworkService networkService;

        /// <summary>
        /// Private Atribute Api Service
        /// </summary>
        private ApiService apiService;

        /// <summary>
        /// Private Atribute Data Service
        /// </summary>
        private DataService dataService;

        /// <summary>
        /// Private Atribute Collection countries 
        /// </summary>
        private List<Country> countryList;

        /// <summary>
        /// Private Atribute Collection rates
        /// </summary>
        private List<Rate> rateList;

        /// <summary>
        /// Private Atribute Object Country
        /// </summary>
        private Country _selected;

        /// <summary>
        /// Private Atribute BindableCollection Country
        /// </summary>
        private BindableCollection<Country> _countries;

        /// <summary>
        /// Private Atribute BindableCollection Rate
        /// </summary>
        private BindableCollection<Rate> _rates;

        /// <summary>
        /// Private Atribute System.String 
        /// </summary>
        private string _status;

        /// <summary>
        /// Private Atribute System.Int32 
        /// </summary>
        private int _progressBar;

        /// <summary>
        /// Private Atribute System.Boolean
        /// </summary>
        private bool _enable;

        /// <summary>
        /// Private Atribute System.Boolean
        /// </summary>
        private bool _load;

        #endregion

        #region Properties

        /// <summary>
        /// Public Propertie Bindable Collection Countries
        /// </summary>
        public BindableCollection<Country> Countries
        {
            get
            {
                return _countries;
            }
            set
            {
                _countries = value;
                NotifyOfPropertyChange(() => _countries);
            }
        }

        /// <summary>
        /// Public Propertie Bindable Collection Rates
        /// </summary>
        public BindableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }
            set
            {
                _rates = value;
                NotifyOfPropertyChange(() => _rates);
            }
        }

        /// <summary>
        /// Public Propertie Object Country
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
                ValidationPage();
                NotifyOfPropertyChange(() => _selected);
            }
        }
                
        /// <summary>
        /// Public Propertie System.String
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        /// <summary>
        /// Public Propertie System.Int32
        /// </summary>
        public int ProgressBar
        {
            get { return _progressBar; }
            set
            {
                _progressBar = value;
                NotifyOfPropertyChange(() => ProgressBar);
            }
        }

        /// <summary>
        /// Public Propertie System.Boolean
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                NotifyOfPropertyChange(() => Enable);
            }
        }

        /// <summary>
        /// Public Propertie System.Boolean
        /// </summary>
        public bool Load
        {
            get { return _load; }
            set { _load = value; }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Initializer the view model for MVVM
        /// </summary>
        public ShellViewModel()
        {
            networkService  = new NetworkService();
            apiService      = new ApiService();
            dataService     = new DataService();

            countryList     = new List<Country>();
            rateList        = new List<Rate>();

            _countries      = new BindableCollection<Country>();
            _rates          = new BindableCollection<Rate>();

            Enable          = false;
            _status         = "A carregar o países...";
            _progressBar    = 0;
            

            LoadApi();
        }

        #endregion

        #region User Control

        /// <summary>
        /// Flag user control
        /// </summary>
        public void FlagPage()
        {
            try
            {
                if (Selected != null)
                { ActivateItem(new FlagPageViewModel(Selected, networkService)); return; }

                ActivateItem(null);
            }
            catch (Exception) { ActivateItem(null); }
        }
        
        /// <summary>
        /// Money Converter Page control
        /// </summary>
        public void MoneyConverterPage()
        {
            rateList.AddRange(Rates);
            try
            {
                if (Selected != null && Rates.Count() > 0)
                {
                    FlagPageViewModel.PageActive        = false;
                    MoreStatsPageViewModel.PageActive   = false;
                    ActivateItem(new MoneyConverterPageViewModel(Selected, rateList));
                    return;
                }
                ActivateItem(null);
            }
            catch (Exception) { ActivateItem(null); }

            rateList.Clear();
        }

        /// <summary>
        /// More Stats Page control
        /// </summary>
        public void MoreStatsPage()
        {
            try
            {
                if (Selected != null)
                {
                    FlagPageViewModel.PageActive            = false;
                    MoneyConverterPageViewModel.PageActive  = false;
                    ActivateItem(new MoreStatsPageViewModel(Selected));
                    return;
                }
                ActivateItem(null);

            }
            catch (Exception) { ActivateItem(null); }
        }

        /// <summary>
        /// About page control
        /// </summary>
        public void About()
        {
            try
            {
                ActivateItem(new AboutPageViewModel());
            }
            catch (Exception) { ActivateItem(null); }
        }

        /// <summary>
        /// Exit de app
        /// </summary>
        public async void ExitButton()
        {
            Response connection = await networkService.CheckConnection();

            if (!connection.IsSucess)
            {
                Application.Current.Shutdown();
            }
            else
            {
                DataAccess.BackupDB();
                Application.Current.Shutdown();
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Load local DB
        /// </summary>
        private void LoadLocal()
        {
            try
            {
                countryList = DataAccess.StoredCountry();

                ProgressBar = 50;
                Status      = "Loading rates";

                rateList    = DataAccess.StoredRates();


                countryList = countryList.OrderBy(c => c.name).ToList();
                rateList    = rateList.OrderBy(r => r.Name).ToList();

                _countries.AddRange(countryList);
                _rates.AddRange(rateList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Page active control
        /// </summary>
        private void ValidationPage()
        {
            rateList.AddRange(Rates);
            if (FlagPageViewModel.PageActive)
            {
                ActivateItem(new FlagPageViewModel(Selected, networkService));
            }
            else if (MoneyConverterPageViewModel.PageActive)
            {
                ActivateItem(new MoneyConverterPageViewModel(Selected, rateList));
            }
            else if (MoreStatsPageViewModel.PageActive)
            {
                ActivateItem(new MoreStatsPageViewModel(Selected));
            }

            rateList.Clear();
        }

        /// <summary>
        /// Checks if client has internet connection
        /// </summary>
        private async void LoadApi()
        {
            Response connection = await networkService.CheckConnection();

            if (!connection.IsSucess)
            {
                LoadLocal();
                ProgressBar += 50;
                Enable      = true;
                Status      = "Modo offline";
            }
            else
            {
                DataAccess.DropDB();
                try
                {
                    await LoadApiCountry();
                    Enable = true;
                    await LoadApiRate();
                }
                catch (Exception)
                {
                    ExitButton();
                }

                ProgressBar += 50;
                Status      = "Seleccione um país";
            }

            if (Countries.Count == 0)
            {
                Enable      = false;
                Status      = "Sem conexão, tente mais tarde";

                return;
            }
        }

        /// <summary>
        /// Loads internet API
        /// </summary>
        /// <returns>Collection Rate</returns>
        private async Task LoadApiRate()
        {
            string url          = "http://rafasaints-001-site3.ctempurl.com";
            string controller   = "/api/rates";
            var response        = await apiService.GetRates(url, controller);

            rateList            = (List<Rate>)response.Result;
            _rates.AddRange(rateList);
            
            dataService.setRateData(rateList);

            Enable = true;
            //Resets temp list
            rateList.Clear();
        }

        /// <summary>
        /// Loads internet API
        /// </summary>
        /// <returns>Collection Country</returns>
        private async Task LoadApiCountry()
        {
            string url          = "http://restcountries.eu";
            string controller   = "/rest/v2/all";
            var response        = await apiService.GetCountries(url, controller);
            
            countryList         = (List<Country>)response.Result;

            ProgressBar         = 50;
            _countries.AddRange(countryList);

            try
            {
                Status          = "A carregar bandeiras";
                await LoadFlags(countryList);
                ProgressBar     = 80;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Status              = "A carregar o cofre...";

            dataService.setCountryData(countryList);

            //Resets temp list
            countryList.Clear();
        }

        /// <summary>
        /// Loads the flags on object Country
        /// </summary>
        /// <param name="Countries">Collection of Country</param>
        private async Task<List<Country>> LoadFlags(List<Country> Countries)
        {
            List<Task<Flags>> tasks = new List<Task<Flags>>();

            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");
                foreach (var country in Countries)
                {
                    tasks.Add(apiService.GetFlags(country.flag, country.name));
                }
            }

            var results = await Task.WhenAll(tasks);

            return Countries;
        }
        
        #endregion
        
    }
}
