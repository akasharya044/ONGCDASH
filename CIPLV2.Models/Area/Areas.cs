using System.ComponentModel.DataAnnotations;
namespace CIPLV2.Models.Area
{
	public class Areas
	{
		[Key]
		public int Id { get; set; }
		public int MfAreaId { get; set; }
		public string? entitytype { get; set; }
		public string? displaylabel { get; set; }
		public string? phaseid { get; set; }
		public int priority_c { get; set; }
		public int category_c { get; set; }
		public string? Category_c_DisplayLabel { get; set; }
		public string? Subcategory_c_DisplayLabel { get; set; }
		public int subcategory_c { get; set; }
		public bool impmcategory_c { get; set; }
		public DateTime? updated_at { get; set; }
		public DateTime? created_at { get; set; }

	}
}
