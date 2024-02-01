namespace VIPS.Common.Models.Entities
{
    [PrimaryKey(nameof(ContractId), nameof(FromId), nameof(ToId))]
    public class Visualization
    {
        public int ContractId { get; set; }

        public string FromId { get; set; }

        public string ToId { get; set; }
    }
}
