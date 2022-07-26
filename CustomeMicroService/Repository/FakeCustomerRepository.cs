namespace CustomeMicroService.Service
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        List<Customer> _Customers = new List<Customer>();


        public FakeCustomerRepository()
        {
            InitData();
        }


        public Customer GetCustomerById(long customerId)
        {
            return _Customers.FirstOrDefault(x => x.Id == customerId);
        }

        public decimal UpdateScore(long customerId, decimal scoreGap)
        {
            var customer = _Customers.FirstOrDefault(x => x.Id == customerId);

            if (customer != null)
            {
                customer.Score = customer.Score + scoreGap;
            }
            else
            {
                string errorMessage = $"No customer existed with id:{customerId}";
                throw new Exception(errorMessage);
            }

            UpdateCustomerRank();

            return customer.Score;

        }
        public IEnumerable<Customer> GetCustomersByRank(int startRank, int endRank)
        {
            var customers = _Customers.Where(x => x.Rank >= startRank && x.Rank <= endRank).OrderBy(x => x.Rank).ThenBy(x => x.Id);

            return customers;
        }

        void InitData()
        {
            _Customers.Add(new Customer { Id = 15514665, Score = 124, Rank = 1 });
            _Customers.Add(new Customer { Id = 81546541, Score = 113, Rank = 2 });
            _Customers.Add(new Customer { Id = 1745431, Score = 100, Rank = 3 });
            _Customers.Add(new Customer { Id = 76786448, Score = 100, Rank = 4 });
            _Customers.Add(new Customer { Id = 254814111, Score = 96, Rank = 5 });
            _Customers.Add(new Customer { Id = 53274324, Score = 95, Rank = 6 });
            _Customers.Add(new Customer { Id = 6144320, Score = 93, Rank = 7 });
            _Customers.Add(new Customer { Id = 8009471, Score = 93, Rank = 8 });
            _Customers.Add(new Customer { Id = 11028481, Score = 93, Rank = 9 });
            _Customers.Add(new Customer { Id = 38819, Score = 92, Rank = 10 });
        }

        void UpdateCustomerRank()
        {
            var newCustomers = _Customers.OrderByDescending(c => c.Score).ThenBy(c => c.Id).Select((c, index) => new
            {
                Id = c.Id,
                Score = c.Score,
                Rank = index + 1
            });

            _Customers.ForEach(c => c.Rank = newCustomers.First(x => x.Id == c.Id).Rank);
        }
    }
}
