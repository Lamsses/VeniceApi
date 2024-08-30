using AutoMapper;
using EFDataAccessLibrary.DataAccess;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly OTContext _context;
        private readonly IMapper _mapper;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ICustomerRepository> _customersRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<IExpenseRepository> _expenseRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IOrderItemRepository> _orderItemRepository;


        public RepositoryManager(OTContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
            _customersRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(context));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
            _expenseRepository = new Lazy<IExpenseRepository>(() => new ExpenseRepository(context));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context)); // Add null as the third argument
            _orderItemRepository = new Lazy<IOrderItemRepository>(() => new OrderItemRepository(context));

        }

        public IProductRepository Product => _productRepository.Value;
        public ICategoryRepository Category => _categoryRepository.Value;
        public ICustomerRepository Customer => _customersRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public IExpenseRepository Expense => _expenseRepository.Value;
        public IOrderRepository Order => _orderRepository.Value;
        public IOrderItemRepository OrderItem => _orderItemRepository.Value;


        public async Task Save() => await _context.SaveChangesAsync();
    }
}
