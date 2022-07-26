
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics.CodeAnalysis;

//namespace CustomeMicroService
//{
//    public class CustomerDBContext : DbContext
//    {
     
//        public CustomerDBContext(DbContextOptions<CustomerDBContext> options)
//            : base(options)
//        {
//            InitData();
//        }

//        void InitData()
//        {
//            Customers.Add(new Customer { Id = 15514665 , Score = 124,Rank=1 });
//            Customers.Add(new Customer { Id = 81546541, Score = 113, Rank = 2 });
//            Customers.Add(new Customer { Id = 1745431, Score = 100, Rank = 3 });
//            Customers.Add(new Customer { Id = 76786448, Score = 100, Rank = 4 });
//            Customers.Add(new Customer { Id = 254814111, Score = 96, Rank = 5 });
//            Customers.Add(new Customer { Id = 53274324, Score = 95, Rank = 6 });
//            Customers.Add(new Customer { Id = 6144320, Score = 93, Rank = 7 });
//            Customers.Add(new Customer { Id = 8009471, Score = 93, Rank = 8 });
//            Customers.Add(new Customer { Id = 11028481, Score = 93, Rank = 9 });
//            Customers.Add(new Customer { Id = 38819, Score = 92, Rank = 10 }); 
//        }


//        public DbSet<Customer> Customers { get; set; } 
//    }
//}