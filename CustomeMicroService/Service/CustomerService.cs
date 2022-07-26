namespace CustomeMicroService.Service
{
    public class CustomerService : ICustomerService
    {
        
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<decimal> UpdateScore(long customerId, decimal scoreGap)
        {
            return await Task.Run(() => _customerRepository.UpdateScore(customerId, scoreGap));
        }

        public async Task<IEnumerable<Customer>> GetCustomersByRank(int startRank, int endRank)
        {
            return await Task.Run(() => _customerRepository.GetCustomersByRank(startRank, endRank));
        }

        public async Task<List<Customer>> GetCustomersById(long customerId, int highRank = 0, int lowRank = 0)
        {
            var custmoer = await Task.Run(() => _customerRepository.GetCustomerById(customerId));
            if (custmoer == null)
            {
                string errorMessage = $"No customer existed with id:{customerId}";
                throw new Exception(errorMessage);
            }

            List<Customer> customers = new List<Customer>() { custmoer };

            if (highRank > 0)
            {
                var highCustomers = await GetCustomersByRank(custmoer.Rank- highRank, custmoer.Rank-1);
                customers.AddRange(highCustomers);
            }

            if (lowRank > 0)
            {
                var lowCustomers = await GetCustomersByRank(custmoer.Rank+1, custmoer.Rank + lowRank);
                customers.AddRange(lowCustomers);
            }

            return customers.OrderBy(x=>x.Rank).ToList();            

        }

    }
}
