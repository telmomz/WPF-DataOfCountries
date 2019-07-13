namespace WPFUI.Models.ApiRate
{
    public class Rate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Currency Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tax Rate
        /// </summary>
        public double TaxRate { get; set; }

        /// <summary>
        /// Name of Currency
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
