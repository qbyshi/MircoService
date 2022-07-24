namespace CustomeMicroService.Service
{
    public interface ICustomerRepository
    {
        decimal UpdateScore(long customerId, decimal scoreGap);
        IEnumerable<Customer> GetCustomersByRank(int startRank, int endRank);

        Customer GetCustomerById(long customerId);
    }
}
