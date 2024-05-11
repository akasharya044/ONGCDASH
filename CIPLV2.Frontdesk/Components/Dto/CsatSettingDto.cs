namespace CIPLV2.Frontdesk.Components
{
	
	public class CsatSettingDto
	{
		public int Id { get; set; }
		public int FeedbackPopupTime { get; set; }
		public string? Name { get; set; } = string.Empty;
		public bool IsEdit { get; set; } = false;
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime UpdatedDate { get; set; } = DateTime.Now;
	}
}
