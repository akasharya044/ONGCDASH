
namespace CIPLV2.Models.Tickets
{
    public class RaiseTicketDto
    {
        public string Description { get; set; }
        public int RegisteredForDevice_c { get; set; }
        public int ONGCCAT_c { get; set; }
        public int ONGCSUB_c { get; set; }
        public int ONGCAREA_c { get; set; }
        public int ContactPerson { get; set; }
        public string SystemId { get; set; } 
    }
}
