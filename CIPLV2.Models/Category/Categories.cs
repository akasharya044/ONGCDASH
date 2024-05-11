
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.Category
{
	public class Categories
	{
		[Key]
		public int Id { get; set; }
		public int MfCatId { get; set; }
		public string? entitytype { get; set; }
		public string? displaylabel { get; set; }
		public string? phaseid { get; set; }
		public string? Category_c { get; set; }
		public bool impmcategory_c { get; set; }
		public DateTime? updated_at { get; set; }
		public DateTime? created_at { get; set; }
	}
}
