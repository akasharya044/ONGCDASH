using CIPLV2.Models.Admin;
using CIPLV2.Models.UserSystemHardware;
using CIPLV2.Models.UserSystemSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.DataService
{
    public interface IUserSystemSoftwareData 
    {
        Task<Response> AddUserSystemSoftware(List<UserSystemSoftwareDto> data);

        Task<Response> GetAllSystemSoftware();

        Task<Response> AddUserSystemHardware(List<UserSystemHardwareDto> data);

        Task<Response> GetAllSystemHardware();


    }
}
