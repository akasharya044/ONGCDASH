using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Frontdesk.Components
{
    public class DeviceDetailDto
    {
        public int Id { get; set; }
        public string? MfDeviceId { get; set; }
        public string? entity_type { get; set; }
        public bool IsDeleted { get; set; }
        public string? SubType { get; set; }
        public long LastUpdateTime { get; set; }
        public string? DisplayLabel { get; set; }
    }
    public class DeviceDetailsDtos
    {
        public string? entity_type { get; set; }
        public Property properties { get; set; } = new Property();
    }
    public class Property
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? SubType { get; set; }
        public long LastUpdateTime { get; set; }
        public string? DisplayLabel { get; set; }
        public string? MfDeviceId { get; set; }
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? AgentVersion { get; set; }
        public string? Location { get; set;}
        public string? Status { get; set;}
    }
	public class DeviceDetailsDtoss
	{
		public string? entity_type { get; set; }
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public string? SubType { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public string? DisplayLabel { get; set; }
		public string? MfDeviceId { get; set; }
		public string? IpAddress { get; set; }
		public string? MacAddress { get; set; }
		public string? AgentVersion { get; set; }
		public string? Location { get; set; }
		public string? Status { get; set; }
	}
	public class DeviceDailyStatusDto
	{
		public int Id { get; set; }
		public string SystemId { get; set; }
		public DateTime LastLoginTime { get; set; }
	}
}
