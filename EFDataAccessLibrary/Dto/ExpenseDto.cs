
namespace EFDataAccessLibrary.Dto
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        public int RandomId { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; } = true;
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

    }
    public enum ExpenseType
    {

        ClubNeed,
        AdvancePayment,
        Purchase,
        Other,


    }


}
