
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.Registration
{
	public class MachineRegistration
	{
		[Key]
		public Guid Id { get; set; }
		public string systemid { get; set; }
		public string mac_address { get; set; }
		public string ipaddress { get; set; }
		public string status { get; set; }
		public string agent_version { get; set; }
		public string install_ram { get; set; }
		public string hard_disk { get; set; }
		 
	}
}
