using System;
using System.Collections.Generic;

namespace VIPS.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        // Existing properties
        public List<CondensedContract> ContractList { get; set; }
        public string SearchQuery { get; set; }
        public int TotalContracts { get; set; }

        // New property to hold the sorting order
        public string SortOrder { get; set; }
    }
}