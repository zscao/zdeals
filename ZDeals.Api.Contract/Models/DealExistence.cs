namespace ZDeals.Api.Contract.Models
{
    public class DealExistence
    {
        public bool Existing { get; set; }
        /// <summary>
        /// if the deal is existing,the Deal property holds the details; otherwise null. 
        /// </summary>
        public Deal Deal { get; set; }
    }
}
