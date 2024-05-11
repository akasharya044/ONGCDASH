
namespace CIPLV2.Models.Tickets
{
     
    public class TicketData
    {
        public string _id { get; set; }
        public string incident_id { set; get; }
        public string severity { set; get; }
        public string subject { set; get; }
        public string location { set; get; }
        public string region { set; get; }
        public string user_name { set; get; }
        public string system_id { set; get; }
        public string user_id { set; get; }
        public string engineer_name { set; get; }
        public string status { set; get; }
        public string resolution_date { set; get; }
        public string feedback_status { set; get; }
        public string feedback_comment { set; get; }
        public string feedback_action { set; get; }
        public string feedback_rating { set; get; }
        public string feedback_date { set; get; }
        public string created_at { set; get; }
        public string updated_at { set; get; }
        public Int32 next_request { get; set; }
        public int close_count { get; set; }
    }

    public class CommonResponse
    {
        public Int32 code { get; set; }
        public string message { get; set; }
        public Int32 status { get; set; }
    }
    public class TicketResponse : CommonResponse
    {
        public List<TicketData> data { get; set; }
    }

    public class TicketSubmitResponse : CommonResponse
    {
        public TicketData data { get; set; }
    }
}
