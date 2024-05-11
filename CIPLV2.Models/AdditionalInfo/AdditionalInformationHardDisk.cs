using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.AdditionalInfo
{
    public class AdditionalInformationHardDisk
    {

        [Key]
        public int Id { get; set; }
        public string? DriveName { get; set; }
        public string? DriveType { get; set; }
        public string? DriveFormat { get; set; }
        public string? SerialNumber { get; set; }
        public long? TotalSize { get; set; }
        public long? FreeSpace { get; set; }
        public long? AvailableFreeSpace { get; set; }

        public string? NonSystemDriveName { get; set; }

        public long? NonSystemTotalSpace { get; set; }

        public long? NonSystemFreeSpace { get; set; }

        public string? SystemId { get; set; }

    }

    public class AdditionalInformationHardDiskDto
    {
        public int Id { get; set; }
        public string? DriveName { get; set; }
        public string? DriveType { get; set; }
        public string? DriveFormat { get; set; }
        public string? SerialNumber { get; set; }
        public long? TotalSize { get; set; }
        public long? FreeSpace { get; set; }
        public long? AvailableFreeSpace { get; set; }

        public string? NonSystemDriveName { get; set; }

        public long? NonSystemTotalSpace { get; set; }

        public long? NonSystemFreeSpace { get; set; }

        public string? SystemId { get; set; }


    }
}
