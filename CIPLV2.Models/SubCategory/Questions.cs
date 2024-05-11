
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.SubCategory
{
	public class Questions
	{
		[Key]
		public int Id { get; set; }
		public int MfAreaId { get; set; }
		public string Question { get; set; }
	}
}
