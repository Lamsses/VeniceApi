
namespace EFDataAccessLibrary.Models
{
   public class Expense
{
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; }

    public ExpenseType Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
}
    public enum ExpenseType
    {

        ClubNeed,
        AdvancePayment,
        Purchase,
        Other,

        
    }


}
