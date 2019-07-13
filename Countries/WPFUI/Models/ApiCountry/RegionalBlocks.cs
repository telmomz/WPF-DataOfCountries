namespace WPFUI.Models.ApiCountry
{
    using System.Collections.Generic;

    public class RegionalBlocks
    {
        public string acronym { get; set; }

        public string name { get; set; }

        public List<string> otherAcronyms { get; set; }

        public List<string> otherNames { get; set; }
    }
}
