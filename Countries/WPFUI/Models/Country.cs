namespace WPFUI.Models
{
    ///References
    using System.Collections.Generic;
    
    /// <summary>
    /// System.Object Country
    /// </summary>
    public class Country
    {

        #region Propreties

        /// <summary>
        /// System.String Country Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Collection of System.String Country Top Level Domain
        /// </summary>
        public List<string> topLevelDomain { get; set; }

        /// <summary>
        /// System.String Country Alpha 2 Code
        /// </summary>
        public string alpha2Code { get; set; }

        /// <summary>
        /// System.String Country Alpha 3 Code
        /// </summary>
        public string alpha3Code { get; set; }

        /// <summary>
        /// Collection of System.String Country Calling Codes
        /// </summary>
        public List<string> callingCodes { get; set; }

        /// <summary>
        /// System.String Country Capital
        /// </summary>
        public string capital { get; set; }

        /// <summary>
        /// Collection of System.Object Country Alternative Spellings
        /// </summary>
        public List<object> altSpellings { get; set; }
        
        /// <summary>
        /// System.String Country Region
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// System.String Country Sub Region
        /// </summary>
        public string subregion { get; set; }
        
        /// <summary>
        /// System.String Country Population
        /// </summary>
        public int population { get; set; }

        /// <summary>
        /// Collection of System.Object Country Latitute / Longitude
        /// </summary>
        public List<object> latlng { get; set; }
        
        /// <summary>
        /// System.String Country Native Name
        /// </summary>
        public string nativeName { get; set; }

        /// <summary>
        /// System.String Country Numeric Code
        /// </summary>
        public string numericCode { get; set; }

        /// <summary>
        /// Collection of System.Object Country Currency
        /// </summary>
        public List<Currency> currencies { get; set; }
        /// <summary>
        /// System.Double Country Gini
        /// </summary>
        public double? gini { get; set; }

        /// <summary>
        /// System.String Country Flag
        /// </summary>
        public string flag { get; set; }
        
        /// <summary>
        /// Collection of System.Object Country Regional Blocs
        /// </summary>
        public List<object> regionalBlocs { get; set; }

        /// <summary>
        /// Override ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{name}";
        }

        #endregion
    }
}
