using CustomeMicroService.Service;
using Microsoft.AspNetCore.Mvc;

namespace CustomeMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {      

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        [HttpPost("{customerId}/score/{score}")]
        public async Task<decimal> UpdateScoreByCustomerId(long customerId, decimal score)
        {
            return await _customerService.UpdateScore(customerId, score);
        }

        [HttpGet("Leaderboard")]
        public async Task<IEnumerable<Customer>> Leaderboard(int startRank, int endRank)
        {
            return await _customerService.GetCustomersByRank(startRank, endRank);
        }

        [HttpGet("Leaderboard/{customerId}")]
        public async Task<IEnumerable<Customer>> GetLeaderboardByCustomerId(long customerId,int high, int low)
        {
            return await _customerService.GetCustomersById(customerId, high, low);
        }

    }
}