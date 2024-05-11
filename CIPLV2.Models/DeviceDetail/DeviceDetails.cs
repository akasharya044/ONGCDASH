
using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.DeviceDetail
{
	public class DeviceDetails
	{
		[Key]
		public int Id { get; set; }
        public string? MfDeviceId { get; set; } = string.Empty;
		public string? entity_type { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
		public string? SubType { get; set; } = string.Empty;
		public long LastUpdateTime { get; set; }
		public string? DisplayLabel { get; set; } = string.Empty;
		public string? IpAddress {  get; set; } = string.Empty;
		public string? MacAddress {  get; set; } = string.Empty;
		public string? AgentVersion { get; set; } = string.Empty;
	}
    public class DeviceEntity
    {
        public string entity_type { get; set; }
        public DeviceProperties properties { get; set; }
        public Dictionary<string, string> related_properties { get; set; }
    }

    public class DeviceProperties
    {
        public long LastUpdateTime { get; set; }
        public string Id { get; set; }
        public string DisplayLabel { get; set; }
		public string? SubType { get; set; }
	}

	public class DeviceMetaData
    {
        public string completion_status { get; set; }
        public int total_count { get; set; }
        public List<object> errorDetailsList { get; set; }
        public List<object> errorDetailsMetaList { get; set; }
        public long query_time { get; set; }
    }

    public class DeviceJsonObject
    {
        public List<DeviceEntity> entities { get; set; }
        public DeviceMetaData meta { get; set; }
    }
}
