
namespace CIPLV2.Models.Tickets
{
	public class TicketRecord
	{
		public int Id { get; set; }
		public string? Description { get; set; } = string.Empty;
		public string? EntityType { get; set; } = string.Empty;
		public string? ServiceDeskGroup { get; set; } = string.Empty;
		public string? RegisteredForActualService { get; set; } = string.Empty;
		public string? RequestedByPerson { get; set; } = string.Empty;
		public string? Priority { get; set; } = string.Empty;
		public int? RegisteredForDevice_c { get; set; } = 0;
		public int? ONGCCAT_c { get; set; } = 0;
		public int? ONGCSUB_c { get; set; } = 0;
		public int? ONGCAREA_c { get; set; } = 0;
		public int? ContactPerson { get; set; } = 0;
		public string? DisplayLabel { get; set; } = string.Empty;
		public bool? TicketGenerated { get; set; } = true; //If not generated reprocess it in handler
		public int? TicketId { get; set; } = 0;
		public int? TicketStatus { get; set; } = 1;//0 pending,1 ticket generated,2 resolved,3 feedback received
		public int? FeedbackCount { get; set; } = 0;
		public bool? FeedBackReceived { get; set; } = false;
		public DateTime? FeedBackReceivedOn { get; set; } = DateTime.Now;
		public DateTime? NextFeedBackSchedule { get; set; } = DateTime.Now;
		public DateTime? CreatedDateTime { get; set; } = DateTime.Now;
		public string? ResolvedDateTime { get; set; } = string.Empty;
		public DateTime? LastUpdateTime { get; set; } = DateTime.Now;
		public string? SystemId { get; set; } //New field added 
		public string? AssignedTo { get; set; } = string.Empty;
		public string? FeedBackRemark { get; set; } = string.Empty;
		public string? Completion_status { get; set; } = string.Empty;
		public int? starcount { get; set; } = 0;
		//get resonse
		public string? CurrentStatus_c { get; set; } = string.Empty;
		public string? ExpertAssignee { get; set; } = string.Empty;
		public string? ExpertAssigneeName { get; set; } = string.Empty;
		public string? RequestedByPersonName { get; set; } = string.Empty;
		public string? RegisteredForLocation { get; set; } = string.Empty;
		public int? close_count { get; set; } = 0;
		//update 
		public string? feedback_status { set; get; }=string.Empty;
		public string? feedback_comment { set; get; } = string.Empty;
		public string? feedback_action { set; get; } = string.Empty;
        public string? feedback_rating { set; get; } = string.Empty;
		public string? Region { get; set; } = string.Empty;
        public string? Severity {  get; set; } = string.Empty;
        public string? Location {  get; set; } = string.Empty;
    }
	public class UpdateTicket
	{
		public int IncidentId { get; set; }
		public string action { get; set; }
		public int starcount { get; set; }
		public string? Remarks { get; set; }
	}

	public class RaiseAgentTicketDTO
	{
		public List<AgentEntity> entities { get; set; }
		public string operation { get; set; }
	}
	public class AgentEntity
	{
		public string entity_type { get; set; }
		public AgentProperty properties { get; set; }
	}
	public class AgentProperty
	{
		public string ServiceDeskGroup { get; set; }
		public string Description { get; set; }
		public string RegisteredForActualService { get; set; }
		public string RequestedByPerson { get; set; }
		public string Priority { get; set; }
		public string ONGCCAT_c { get; set; }
		public string ONGCSUB_c { get; set; }
		public string ContactPerson { get; set; }
		public string DisplayLabel { get; set; }
		public string SystemId { get; set; }
        public string ONGCAREA_c { get; set; }
        public int RegisteredForDevice_c { get; set; }


    }

    public class RaiseMFTicketDTO
	{
		public List<MFEntity> entities { get; set; }
		public string operation { get; set; }
	}
	public class MFEntity
	{
		public string entity_type { get; set; }
		public MFProperty properties { get; set; }
	}
	public class MFProperty
	{
		public string ServiceDeskGroup { get; set; }
		public string Description { get; set; }
		public string RegisteredForActualService { get; set; }
		public string RequestedByPerson { get; set; }
		public string Priority { get; set; }
		public string ONGCCAT_c { get; set; }
		public string ONGCSUB_c { get; set; }
		public string ContactPerson { get; set; }
		public string DisplayLabel { get; set; }
        public int RegisteredForDevice_c { get; set; }
        public string ONGCAREA_c { get; set; }

    }


    public class TicketReport
	{
		public int sr_no { get; set; }
		public int? incident_id { get; set; }
		public string? priority { get; set; } = string.Empty;
		public string? engineer_name { get; set; } = string.Empty;
		public string? system_id { get;set; }=string.Empty;
		public string? subject { get; set; } = string.Empty;
		public string? user_name { get; set; } = string.Empty;
		public string? status { get; set; } = string.Empty;
		public DateTime? resolved_date { get; set; }
		public string? location { get; set; } = string.Empty;
		public string? feedback_comment { get; set; } = string.Empty;
		public int? feedback_rating { get; set; } 
		public int? close_count { get; set; }
		public string feedback_status { get; set; } = string.Empty;
		public DateTime? created_at { get; set; } = DateTime.Now;

		
	}
}
