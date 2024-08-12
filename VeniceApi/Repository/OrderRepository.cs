using AutoMapper;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class OrderRepository : BaseReopsitory<Order>, IOrderRepository
    {
        private readonly OTContext _context;
        public OrderRepository(OTContext context) : base(context)
        {
            _context = context;
        }

        
    }
}
