
namespace CIPLV2.Models.Tickets
{
	public class CsatSetting
	{
		public int Id {  get; set; }
		public int FeedbackPopupTime {  get; set; }
		public string? Name { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; }= DateTime.Now;
		public DateTime UpdatedDate { get; set; }=DateTime.Now;
	}
}
