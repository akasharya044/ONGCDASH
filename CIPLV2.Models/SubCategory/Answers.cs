
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIPLV2.Models.SubCategory
{
	public class Answers
	{
		[Key]
		public int Id { get; set; }
		public string Answer { get; set; }
		[ForeignKey("QuestionId")]
		public int QuestionId { get; set; }
		//public Questions Question { get; set; }
	}
}
