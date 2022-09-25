using System.Collections.Concurrent;

namespace CustomeMicroService.Service
{
    public class CustomerService : ICustomerService
    {
        private static ConcurrentDictionary<long, decimal> _customerIdScore = new ConcurrentDictionary<long, decimal>();
        private static List<Customer> _Customers = new List<Customer>();


        public CustomerService()
        {
        }


        public async Task<decimal> UpdateScore(long customerId, decimal scoreGap)
        {

            var newScore = scoreGap;
            decimal customerScore;

            if (_customerIdScore.TryGetValue(customerId, out customerScore))
            {
                newScore = customerScore + scoreGap;

                Customer oldCustomer = new Customer { Id = customerId, Score = customerScore };
                int originIndex = _Customers.BinarySearch(oldCustomer);
                _Customers.RemoveAt(originIndex);
            }

            _customerIdScore.AddOrUpdate(customerId, newScore, (oldkey, oldvalue) => customerScore);

            Customer newCustomer = new Customer { Id = customerId, Score = newScore };
            _Customers.Add(newCustomer);

            return await Task.Run(() => newScore);
        }

        public async Task<IEnumerable<LeaderBoard>> GetCustomersByRank(int startRank, int endRank)
        {

            var customers = (await Task.Run(() => _Customers.Skip(startRank).Take(endRank - startRank))).ToList();

            var result = new List<LeaderBoard>();

            for (int index = 0; index < customers.Count(); index++)
            {
                result.Add(new LeaderBoard
                {
                    CustomerId = customers[index].Id,
                    Score = customers[index].Score,
                    Rank = startRank + index
                }
                );
            }

            return result;

        }

        public async Task<List<LeaderBoard>> GetCustomersById(long customerId, int highRank = 0, int lowRank = 0)
        {
            var customerInfo = GetCustomerIndexById(customerId);
            var index = customerInfo.index;
            var customer = customerInfo.customer;

            int startRank = customerInfo.index - highRank;
            int endRank = customerInfo.index + lowRank;

            if (highRank > 0 || lowRank > 0)
            {
                return (await GetCustomersByRank(startRank, endRank)).ToList();
            }
            else
            {
                return new List<LeaderBoard>()
                { 
                    new LeaderBoard { CustomerId = customer.Id, Score = customer.Score, Rank = index }
                };
            }
        }

        private (int index, Customer customer) GetCustomerIndexById(long customerId)
        {
            decimal customerScore;

            if (!_customerIdScore.TryGetValue(customerId, out customerScore))
            {
                string errorMessage = $"No customer existed with id:{customerId}";
                throw new Exception(errorMessage);
            }

            var customer = new Customer { Id = customerId, Score = customerScore };

            return (_Customers.BinarySearch(customer), customer);
        }


    }
}
