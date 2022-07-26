//using Microsoft.EntityFrameworkCore;
using CustomeMicroService;
using CustomeMicroService.Service;

namespace CustomeMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.     
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<ICustomerRepository, FakeCustomerRepository>();
            //builder.Services.Scan(scan => scan.FromAssemblyOf<CustomerService>()
            //    .AddClasses(t => t.Where(type => type.Name.EndsWith("Service") || type.Name.EndsWith("Repository")))
            //    .AsImplementedInterfaces());


            builder.Services.AddControllers();
            //builder.Services.AddDbContext<CustomerDBContext>(opt =>
            //   opt.UseInMemoryDatabase("Customers"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.


            app.MapControllers();

            app.Run();
        }
    }
}