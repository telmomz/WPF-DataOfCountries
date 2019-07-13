namespace WPFUI.Models
{
    /// <summary>
    /// System.Object Rate
    /// </summary>
    public class Rate
    {
        /// <summary>
        /// System.Int32 Id
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// System.String Currency Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// System.Double Tax Rate
        /// </summary>
        public double TaxRate { get; set; }

        /// <summary>
        /// System.String Name of Currency
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
