using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Incidents
{
    public class Incident
    {
        [Key]
        public int TicketId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Requester { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string AssignedTo { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Subject { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Description { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string CategoryType { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Priority { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Tags { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Notes { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Feedback { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Location { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Queues { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }

    }

    public class IncidentDTO
    {
        public int TicketId { get; set; }
        public string Requester { get; set; }
        public string AssignedTo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string CategoryType { get; set; }
        public string Priority { get; set; }
        public string Tags { get; set; }
        public string Notes { get; set; }
        public string Feedback { get; set; }
        public string Location { get; set; }
        public string Queues { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
