using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;

namespace VeniceApi.Dto
{
    public class Helper : Profile
    {
        public Helper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<Product, ProductDtoAdd>();
            CreateMap<ProductDtoAdd, Product>();

            CreateMap<Product, ProductDtoUpdate>();
            CreateMap<ProductDtoUpdate, Product>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Category, CategoryDtoModfiy>();
            CreateMap<CategoryDtoModfiy, Category>();

            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseDto, Expense>();
        }

    }
}
