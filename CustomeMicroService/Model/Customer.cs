using Microsoft.EntityFrameworkCore;
using CustomeMicroService;
namespace CustomeMicroService
{
    public class Customer
    {
        public long Id { get; set; } = 0;
        public decimal Score { get; set; }
        public int Rank { get; set; }
	
    }
}
