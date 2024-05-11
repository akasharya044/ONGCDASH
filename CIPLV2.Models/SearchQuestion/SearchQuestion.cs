
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.SearchQuestion
{
    public  class SearchQuestion
    {
        [Key]
        public int Id { get; set; }
        public string? SearchText { get; set; } = string.Empty;  
        public int Count {  get; set; }
        public DateTime CreatedDate { get; set; }   = DateTime.Now;
    }
}
