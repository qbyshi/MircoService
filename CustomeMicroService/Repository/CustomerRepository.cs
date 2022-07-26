//namespace CustomeMicroService.Service
//{
//    public class CustomerRepository: ICustomerRepository
//    {
//        private readonly CustomerDBContext _customerDBContext;

//        public CustomerRepository(CustomerDBContext customerDBContext)
//        {
//            _customerDBContext = customerDBContext;
//        }

//        public Customer GetCustomerById(long customerId)
//        {
//            return _customerDBContext.Customers.Local.FirstOrDefault(x => x.Id== customerId);
//        }

//        public decimal UpdateScore(long customerId, decimal scoreGap)
//        {
//            var customer =  _customerDBContext.Customers.Local.FirstOrDefault(x => x.Id == customerId);

//            if (customer != null)
//            {
//                customer.Score = customer.Score - scoreGap;
//                _customerDBContext.SaveChanges();
//            }
//            else
//            {
//                string errorMessage = $"No customer existed with id:{customerId}";
//                throw new Exception(errorMessage);
//            }

//            return customer.Score;
           
//        }
//        public IEnumerable<Customer> GetCustomersByRank(int startRank, int endRank)
//        {
//            var customers = _customerDBContext.Customers.Local.Where(x => x.Rank >= startRank && x.Rank <= endRank).OrderBy(x=>x.Rank).ThenBy(x=>x.Id);
           
//            return customers;
//        }
       
//    }
//}
