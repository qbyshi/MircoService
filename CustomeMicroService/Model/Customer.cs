
namespace CustomeMicroService
{
    public class Customer : IComparable<Customer>
    {
        public long Id { get; set; } = 0;
        public decimal Score { get; set; }

        public int CompareTo(Customer? other)
        {
            if (other == null) return -1;

            int result = other.Score.CompareTo(this.Score);
            if (0 == result)
            {

                return Id.CompareTo(other.Id);
            }
            return result;
        }
    }

    public class LeaderBoard
    {
        public long CustomerId { get; set; }
        public decimal Score { get; set; }
        public int Rank { get; set; }
    }
}
