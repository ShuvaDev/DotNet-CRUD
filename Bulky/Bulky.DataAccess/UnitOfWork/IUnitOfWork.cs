using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository categoryRepo { get; }
        public IProductRepository productRepo { get; }
        public ICompanyRepository companyRepo { get; }
        public IShoppingCartRepository shoppingCartRepo { get; }
        public IApplicationUserRepository applicationUserRepo { get; }
        public IOrderHeaderRepository orderHeaderRepo { get; }
        public IOrderDetailRepository orderDetailRepo { get; }
        void Save();
    }
}
