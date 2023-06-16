using Autofac;
using Microsoft.AspNetCore.Mvc;
using SimApi.Base;
using SimApi.Data.Repository;
using SimApi.Data.Uow;
using SimApi.Operation;
using System.Configuration;

namespace SimApi.Service.Autofac
{
    public class AutofacModule :Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserLogService>().As<ITokenService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<TransactionService>().As<ITransactionService>();
            builder.RegisterType<TransactionReportService>().As<ITransactionReportService>();
            builder.RegisterType<DapperAccountService>().As<IDapperAccountService>();
            builder.RegisterType<DapperCategoryService>().As<IDapperCategoryService>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }

    }
}
