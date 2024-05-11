
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.PersonDetail
{
	public class PersonDetails
	{
		[Key]
		public int Id { get; set; }
        public string MfpersonId {  get; set; }
		public string? entity_type { get; set; }
		public bool IsDeleted { get; set; }
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? EmployeeNumber { get; set; }
		public bool IsVIP { get; set; }
		public string? Name { get; set; }
		public string? Upn { get; set; }
		public long LastUpdateTime { get; set; }
		public int Location { get; set; }

	}
    public class Entity
    {
        public string entity_type { get; set; }
        public Propertyperson properties { get; set; }
        public Dictionary<string, string> related_properties { get; set; }
    }

    public class Propertyperson
    {
        public string Upn { get; set; }
        public long LastUpdateTime { get; set; }
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeNumber { get; set; }
        public bool IsVIP { get; set; }
        public int Location { get; set; }

    }

    public class MetaData
    {
        public string completion_status { get; set; }
        public int total_count { get; set; }
        public List<object> errorDetailsList { get; set; }
        public List<object> errorDetailsMetaList { get; set; }
        public long query_time { get; set; }
    }

    public class Persondata
    {
        public List<Entity> entities { get; set; }
        public MetaData meta { get; set; }
    }
}
