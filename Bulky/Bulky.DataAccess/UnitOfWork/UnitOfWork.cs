using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository categoryRepo { get; private set; }
        public IProductRepository productRepo { get; private set; }
        public ICompanyRepository companyRepo { get; private set; }
        public IShoppingCartRepository shoppingCartRepo { get; private set; }
        public IApplicationUserRepository applicationUserRepo { get; private set; }
        public IOrderDetailRepository orderDetailRepo { get; private set; }
        public IOrderHeaderRepository orderHeaderRepo { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRepo = new CategoryRepository(_db);
            productRepo = new ProductRepository(_db);
            companyRepo = new CompanyRepository(_db);
            shoppingCartRepo = new ShoppingCartRepository(_db);
            applicationUserRepo = new ApplicationUserRepository(_db);
            orderDetailRepo = new OrderDetailRepository(_db);
            orderHeaderRepo = new OrderHeaderRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
