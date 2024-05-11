
namespace CIPLV2.Models.SubCategory
{
	public class SubCategoriesDto
	{
		public int Id { get; set; }
		public long MfSubCatId { get; set; }
		public string? entitytype { get; set; }
		public string? displaylabel { get; set; }
		public string? Category_c_DisplayLabel { get; set; }
		public string? phaseid { get; set; }
		public long category_c { get; set; }
		public bool impmcategory_c { get; set; }
		public DateTime? updated_at { get; set; }
		public DateTime? created_at { get; set; }
	}
}
