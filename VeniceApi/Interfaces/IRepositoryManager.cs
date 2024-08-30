namespace VeniceApi.Interfaces
{
    public interface IRepositoryManager
    {
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        ICustomerRepository Customer { get; }
        IEmployeeRepository Employee { get; }
        IExpenseRepository Expense { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        Task Save();
    }
}
