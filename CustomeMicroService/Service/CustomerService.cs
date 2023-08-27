using System.Collections.Concurrent;

namespace CustomeMicroService.Service
{
    public class CustomerService : ICustomerService
    {      
        private static ConcurrentDictionary<long, Customer> userDictionary = new ConcurrentDictionary<long, Customer>();

        private static AVLTree<Customer> rankingTree = new AVLTree<Customer>();



        public CustomerService()
        {

        }


        public async Task<decimal> UpdateScore(long customerId, decimal scoreGap)
        {

            if (userDictionary.TryGetValue(customerId, out Customer user))
            {
                rankingTree.Delete(user); 
                user.Score += scoreGap;
                rankingTree.Insert(user); 
                return user.Score;
            }
            else
            {
                var customer = new Customer { Id = customerId, Score = scoreGap };
                userDictionary.TryAdd(customerId, customer);
                rankingTree.Insert(customer);
                return scoreGap;
            }

        }

        public async Task<IEnumerable<LeaderBoard>> GetCustomersByRank(int startRank, int endRank)
        {

            var result = new List<LeaderBoard>();

            var customers = rankingTree.GetElementsByRankRange(startRank, endRank);
            int index = 0;

            foreach (var customer in customers)
            {
                result.Add(new LeaderBoard() { CustomerId = customer.Id, Score = customer.Score, Rank = startRank + index });
                index++;
            }


            return result;
        }

        public async Task<List<LeaderBoard>> GetCustomersById(long customerId, int highRank = 0, int lowRank = 0)
        {
            
            if (!userDictionary.TryGetValue(customerId, out Customer customerInfo))
            {
                throw new Exception($"can not find cusomter with id:{customerId}");
            }

            int index = rankingTree.GetRank(customerInfo);

            int startRank = index - highRank;
            int endRank = index + lowRank;

            if (highRank > 0 || lowRank > 0)
            {
                return (await GetCustomersByRank(startRank, endRank)).ToList();
            }
            else
            {
                return new List<LeaderBoard>()
                {
                    new LeaderBoard { CustomerId = customerInfo.Id, Score = customerInfo.Score, Rank = index }
                };
            }
        }

    }
}
