using VIPS.Models.Data;

namespace VIPS.Models.ViewModels.Search
{
    public class SearchViewModel
    {
        public List<Contract> ContractList { get; set; }


        public SearchViewModel()
        {
            ContractList = new List<Contract>();
        }

        
    }
}
