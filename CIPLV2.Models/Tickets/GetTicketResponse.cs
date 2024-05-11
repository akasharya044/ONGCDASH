using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Tickets
{
    public class GetTicketResponse
    {
        public string? Id { get; set; }
        public string? DisplayLabel { get; set; }
        public string? Priority { get; set; }
        public string? RegisteredForActualService { get; set; }
        public string? ExpertAssignee {  get; set; }
        public long? LastUpdateTime {  get; set; }
        public string? CurrentStatus_c { get; set; }

        public long? ResolvedTime_c { get; set; }
        public string? RegisteredForDevice_c { get; set; }

    }
    public class ContactPerson
    {
        public string? Name { get; set; }
    }
    public class ExpertAssignee
    {
        public string? Name { get; set; }
    }
    public class RegisteredForLocation
    {
        public string? Name { get; set; }
    }
    public class RegisteredForDevice_c
    {
        public string? DisplayLabel { get; set; }
    }

    public class Entities
    {
        public string? entity_type { get; set; }
        public GetTicketResponse? properties { get; set; }
        public RelatedProperties? related_properties { get; set; }

	}
    public class RelatedProperties
    {
        public ExpertAssignee? ExpertAssignee { get; set; }
        public RegisteredForLocation? RegisteredForLocation { get; set; }
        public RegisteredForDevice_c? RegisteredForDevice_C { get; set; }
        public ContactPerson? ContactPerson { get; set; }
	}
    public class TicketResponseGet
    {
        public List<Entities>? entities { get; set; }
        public Metas? meta { get; set; }
    }
    public class Metas
    {
        public string completion_status { get; set;}
        public List<object> errorDetailsList { get; set; }
        public List<object> errorDetailsMetaList { get; set; }
        public int total_count { get; set;}
        public long query_time { get; set;}
    }
}
