namespace CIPLV2.Models.Registration
{
	public class MachineRegistrationDto
	{
		public Guid Id { get; set; }
		public string systemid { get; set; }
		public string userid { get; set; }
		public string mac_address { get; set; }
		public string ipaddress { get; set; }
		public string status { get; set; }
		public string agent_version { get; set; }
		public string install_ram { get; set; }
		public string hard_disk { get; set; }
		public string Location { get; set; }
		public DateTime LastRunningTime { get; set; }
		public int Sno { get; set; }
	}
}
