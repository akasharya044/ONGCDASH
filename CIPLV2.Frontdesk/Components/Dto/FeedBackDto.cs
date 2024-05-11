namespace CIPLV2.Frontdesk.Components
{
	public class FeedBackDto
	{
		public int SrNo {  get; set; }
		public string SystemId { get; set; }
		public string RequestedByPerson { get; set; }
		public int feedback_rating { get; set; }
		public string? feedback_comment {  get; set; }
	}
}
