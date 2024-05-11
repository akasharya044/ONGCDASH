
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.DeviceDetail
{
	public class DeviceDailyStatus
	{
		[Key]
		public int Id {  get; set; }
		public string SystemId { get; set; }
		public DateTime LastLoginTime {  get; set; }
	}
}
