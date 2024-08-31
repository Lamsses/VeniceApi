namespace EFDataAccessLibrary.Models
{
    public class OrderReportRequest
    {
        public List<int>? OrderStatuses { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}