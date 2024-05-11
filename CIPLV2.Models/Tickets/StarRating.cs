using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Tickets
{
	public class StarRating
	{
		[Key]
		public int Id { get; set; }
		public int ONGCCAT_c { get; set; }
		public int ONGCSUB_c { get; set; }
		public int ONGCAREA_c { get; set; }
		public string UserName { get; set; }
		public int starcount { get; set; }
	}
	public class StarRatingDTO
	{
		public int ONGCCAT_c { get; set; }
		public int ONGCSUB_c { get; set; }
		public int ONGCAREA_c { get; set; }
		public string UserName { get; set; }
		public int starcount { get; set; }
	}
}
