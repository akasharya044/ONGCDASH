

using System.ComponentModel.DataAnnotations;

namespace CIPLV2.Models.AdditionalInfo;

public class AdditionalInformation
{
	[Key]
    public int Id { get; set; }
    public string? Caption { get; set; }
    public string? OSArchitecture { get; set; }
    public string? Version { get; set; }
    public string? BuildNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? SerialNumber { get; set; }
    public string? DomainRole { get; set; }
    public DateTime LastBootUpTime { get; set; }
    public int? NoOfDaysLastBoot { get; set; }
    public bool? IsActivated { get; set; }
    public string? WindowKey { get; set; }
    public string? SystemId { get; set; }

    public string? Role { get; set; }

    public string? LastLogged { get; set; }

    public string? AdminStatus { get; set; }
}
public class AdditionalInformationDTO
{
    public int Id { get; set; }
    public string? Caption { get; set; }
    public string? OSArchitecture { get; set; }
    public string? Version { get; set; }
    public string? BuildNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? SerialNumber { get; set; }
    public string? DomainRole { get; set; }
    public DateTime LastBootUpTime { get; set; }
    public int? NoOfDaysLastBoot { get; set; }
    public bool? IsActivated { get; set; }
    public string? WindowKey { get; set; }
    public string? SystemId { get; set; }

    public string? Role { get; set; }

    public string? LastLogged { get; set; }

    public string? AdminStatus { get; set; }
}
