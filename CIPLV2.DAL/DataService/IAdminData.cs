using CIPLV2.Models.Admin;
namespace CIPLV2.DAL.DataService
{
    public interface IAdminData
    {
        Task<Response> GetTestList();
        Task<Response> UpsertTest(TestDTO test);
        Task<Response> Login(AdminUsersDto data);
        Task<Response> UpsertAdminUsers(AdminUsersDto data);
		Task<Response> Logindashboard(AdminUsersDto data);


	}
}
