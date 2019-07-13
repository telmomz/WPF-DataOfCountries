namespace WPFUI.ViewModels
{
    ///References
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using WPFUI.Models;

    /// <summary>
    /// Money Converter Page
    /// </summary>
    public class MoneyConverterPageViewModel : Screen
    {
        #region Atributes


        NumberFormatInfo symbolOrigin, symbolDestiny;

        /// <summary>
        /// Selected country
        /// </summary>
        private Country _country;

        /// <summary>
        /// Selected rate
        /// </summary>
        private Rate _select;

        /// <summary>
        /// Selected rate
        /// </summary>
        private Rate _selectCountry;

        /// <summary>
        /// Rates
        /// </summary>
        private BindableCollection<Rate> _rates;

        /// <summary>
        /// Value
        /// </summary>
        private string _value;

        /// <summary>
        /// Money
        /// </summary>
        private string _money;

        #endregion

        #region Propreties

        /// <summary>
        /// Country selected
        /// </summary>
        public Country Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                if (_country != null)
                {
                    NotifyOfPropertyChange(() => Country);
                }
            }
        }

        /// <summary>
        /// Bindable Collection Rates
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
                NotifyOfPropertyChange(() => Rates);
            }
        }

        /// <summary>
        /// Rate of selected country
        /// </summary>
        public Rate SelectCountry
        {
            get
            {
                return _selectCountry;
            }
            set
            {
                _selectCountry = value;
                NotifyOfPropertyChange(() => SelectCountry);
            }
        }

        /// <summary>
        /// Rate selected
        /// </summary>
        public Rate Select
        {
            get
            {
                return _select;
            }
            set
            {
                _select = value;
                NotifyOfPropertyChange(() => Select);
            }
        }

        /// <summary>
        /// Text Box
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        /// <summary>
        /// Text Box
        /// </summary>
        public string Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
                NotifyOfPropertyChange(() => Money);
            }
        }

        /// <summary>
        /// Page Active
        /// </summary>
        public static bool PageActive { get; set; }

        #endregion

        /// <summary>
        /// Money Converter Page Initializer
        /// </summary>
        /// <param name="selected"></param>
        public MoneyConverterPageViewModel(Country selected, List<Rate> rates)
        {
            _rates          = new BindableCollection<Rate>();
            this._country   = selected;

            _rates.AddRange(rates);

            PageActive       = true;
        }

        #region Methods

        /// <summary>
        /// Convertor
        /// </summary>
        public void converterButton()
        {
            decimal value;
            if (!decimal.TryParse(Value, out value))
            {
                Money = "Insira um número para converter";
                return;
            }

            if (Value.Contains(","))
            {
                Money = "O valor numérico tem que ser por '.' ";
                return;
            }
            
            var taxDestiny  = Select;

            var taxOrigin   = getCountryRate();
            Validation(taxDestiny, taxOrigin);
            try
            {
                var temp = taxDestiny.Code.Substring(0, 2);
                var valueConvert = value / (decimal)taxOrigin.TaxRate * (decimal)taxDestiny.TaxRate;
                try
                {

                    RegionInfo origin   = new RegionInfo(_country.alpha2Code.ToUpper());
                    RegionInfo destiny  = new RegionInfo(temp.ToUpper());

                    string o = origin.CurrencySymbol;
                    string d = destiny.CurrencySymbol;

                    if (destiny.CurrencySymbol == string.Empty)
                    {
                        Money = $"{taxOrigin.Code} {value:N3} \t=\t {taxDestiny.Code} {valueConvert:N3}";
                    }

                    Money = $"{origin.CurrencySymbol} {value:N3} \t=\t {destiny.CurrencySymbol} {valueConvert:N3}";
                }
                catch (Exception)
                {
                    Money = $"{taxOrigin.Code} {value:N3} \t=\t {taxDestiny.Code} {valueConvert:N3}";
                }

            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// Get destiny conversion
        /// </summary>
        /// <param name="rate">object Rate</param>
        /// <returns>object Rate</returns>
        private Rate getCountryRate()
        {
            Rate rate           = new Rate();
            List<Rate> rates    = new List<Rate>();

            rates.AddRange(Rates);

            try
            {
                List<Currency> t = Country.currencies.ToList();

                if(t.Count > 0)
                {
                    foreach (var r in rates)
                    {
                        if (t.Single().code == r.Code)
                        {
                            return rate = r;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Validation
        /// </summary>
        private void Validation(Rate texDestiny, Rate taxOrigin)
        {
            if (Select == null)
                Money = "Seleccione um país";

            else if(texDestiny == null)
                Money = "Seleccione uma moeda";
            
            else if (taxOrigin == null)
                Money = "Não há dados disponíveis, tente outro país";

        }

        #endregion
    }
}
