using System.Collections.Concurrent;

namespace CustomeMicroService.Service
{
    public class CustomerService : ICustomerService
    {
        SortedDictionary<long,Customer> _Customers = new SortedDictionary<long, Customer>();
        private static readonly ConcurrentDictionary<long, Customer> _customerIdScore = new ConcurrentDictionary<long, Customer>();

        private readonly ICustomerRepository _customerRepository;


        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }


        public async Task<decimal> UpdateScore(long customerId, decimal scoreGap)
        {
            return await Task.Run(() => _customerRepository.UpdateScore(customerId, scoreGap));
        }

        public async Task<IEnumerable<LeaderBoard>> GetCustomersByRank(int startRank, int endRank)
        {

            var customers = (await Task.Run(() => _Customers.Skip(startRank).Take(endRank - startRank)));

            var result = new List<LeaderBoard>();
           
            for(int index = 0; index < customers.Count(); index++)
            {
                result.Add(new LeaderBoard { CustomerId = customers[index].Value.Id });
            }


           
        }

        public async Task<List<Customer>> GetCustomersById(long customerId, int highRank = 0, int lowRank = 0)
        {
            var custmoer = await Task.Run(() => _customerIdScore[customerId]);
            if (custmoer == null)
            {
                string errorMessage = $"No customer existed with id:{customerId}";
                throw new Exception(errorMessage);
            }          

            int startRank = custmoer.Rank - highRank;
            int endRank = custmoer.Rank + lowRank;

            if (highRank > 0|| lowRank > 0)
            {
                return (await GetCustomersByRank(startRank, endRank)).ToList();
                
            }     
            else
            {
                return new List<Customer>() { custmoer };
            }
        }

    }
}
