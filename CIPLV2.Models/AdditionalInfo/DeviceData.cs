using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.AdditionalInfo
{
    public class DeviceData
    {
        [Key]
        public int Id { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceName { get; set; }

        public string? BIOS { get; set; }

        public string? MAC { get; set; }

        public string? IP { get; set; }
        public string? VirtualMemory { get; set; }
        public string? AvilableMemory { get; set; }

        public string? displayManufacture { get; set; }

        public string?  displaydetails { get; set; }

        public string? displayname { get; set; }

        public string? SystemId { get; set; }
    }

    public class DeviceDataDto
    {
        public string? DeviceType { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceName { get; set; }

        public string? BIOS { get; set; }

        public string? MAC { get; set; }

        public string? IP { get; set; }
        public string? VirtualMemory { get; set; }
        public string? AvilableMemory { get; set; }

        public string? displayManufacture { get; set; }

        public string? displaydetails { get; set; }

        public string? displayname { get; set; }

        public string? SystemId { get; set; }
    }
}
