
namespace CIPLV2.Models.PersonDetail
{
	public class PersonDetailsDto
	{
		public string entity_type {  get; set; }
		public Properties properties { get; set; }
	}
	public class Properties
	{
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmployeeNumber { get; set; }
		public bool IsVIP { get; set; }
		public string Name { get; set; }
		public string Upn { get; set; }
		public long LastUpdateTime { get; set; }
        public string MfpersonId { get; set; }

    }
}
