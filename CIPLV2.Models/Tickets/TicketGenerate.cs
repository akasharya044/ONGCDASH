using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Tickets
{
    public class Properties
    {
        public long? LastUpdateTime { get; set; }
        public string? Id { get; set; }
    }

    public class Entity
    {
        public string entity_type { get; set; }
        public Properties properties { get; set; }
        public Dictionary<string, object> related_properties { get; set; }
    }

    public class EntityResult
    {
        public Entity entity { get; set; }
        public string? completion_status { get; set; }
    }
    public class Meta
    {
        public string? completion_status { get; set; }
    }
    public class Data
    {
        public List<EntityResult> entity_result_list { get; set; }
        public List<object> relationship_result_list { get; set; }
        public List<object> translation_result_list { get; set; }
        public Meta meta { get; set; }
    }
    //public class TicketGenerate
    //{
    //    public List<EntityResult> entity_result_list { get; set; }
    //}
    //public class Entities
    //{
    //    public string entity_type { get; set; }
    //    public CIProperties properties { get; set; }

    //}
    //public class EntityResult
    //{
    //    public Entities entity { get; set; }
    //}
    //public class CIProperties
    //{
    //    public int Id { get; set; }
    //    public string DisplayLabel { get; set; }
    //    public string Name { get; set; }
    //    public string FirstName { get; set; }

    //}
}
