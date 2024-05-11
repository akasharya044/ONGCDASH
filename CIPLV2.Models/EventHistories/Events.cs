
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.EventHistories
{
	public class Events
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
