using VIPS.Common.Models.Entities;

namespace VIPS.Models.ViewModels.Account
{
    public class IndexViewModel
    {
        public IEnumerable<AppUser> UserList { get; set; }
        public int AccountTotal { get; set; }
    }
}
