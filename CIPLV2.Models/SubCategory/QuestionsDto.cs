
namespace CIPLV2.Models.SubCategory
{
	public class QuestionsDto
	{
		public int Id { get; set; }
        public int MfAreaId { get; set; }
        public int categoryId { get; set; }
		public int subCategoryId { get; set; }
		public string Question { get; set; }
	}
	public class QuestionDto
	{
		public int Id { get; set; }
		public long MfAreaId { get; set; }
		public long categoryId { get; set; }
		public long subCategoryId { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string? category { get; set; }
		public string? Subcategory { get; set; }
		public string? Area { get; set; }
	}
}
