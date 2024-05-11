namespace CIPLV2.API.Endpoints
{
    public static partial class UserSystemSoftwareExtension
    {
        public static RouteGroupBuilder MapSystemInfoEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/softwareinfo", UserSystemSoftware);
            group.MapGet("/softwareinfo", GetUserSystemSoftware);
            group.MapPost("/hardwareinfo", AddSystemHardware);
            group.MapGet("/hardwareinfo", GetallHardware);
            return group;
        }
    }
}
