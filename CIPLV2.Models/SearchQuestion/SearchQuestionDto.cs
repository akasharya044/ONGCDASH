
namespace CIPLV2.Models.SearchQuestion
{
    public class SearchQuestionDto
    {
        public int Id { get; set; }
        public string? SearchText { get; set; } = string.Empty;
		public int count { get; set; }
		public DateTime CreatedDate { get; set; }
    }
}
