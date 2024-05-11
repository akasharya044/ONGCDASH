using CIPLV2.Models.Admin;
using CIPLV2.Models.Common;
using Newtonsoft.Json;
using Radzen;
using System.Reflection.Metadata;

namespace CIPLV2.Frontdesk.Components.Pages
{
    public partial class Login
    {
        UserMasterDTO FormData { get; set; } = new UserMasterDTO();
        LoginDTO loginDTO { get; set; } = new LoginDTO();
        public bool IsUpdating { get; set; } = false;
        public bool IsLoading { get; set; } = false;


        //public bool hide { get; set; } = true;
        //protected override async Task OnInitializedAsync()
        //{
        //    sessionData.UserData = null;
        //    //NavigationManager.NavigateTo("/");

        //    //await base.OnInitializedAsync();
        //}
        //private async Task SignIn()
        //{
        //    System.Threading.Thread.Sleep(1500);
        //    try
        //    {
        //        if (!IsUpdating && !IsLoading)
        //        {
        //            IsLoading = true; // Show loader
        //            IsUpdating = true;
        //            if (loginDTO != null)
        //            {
        //                var response = await ciplapiservice.PostData(loginDTO, $"api/admin/dash/login");
        //                if (response != null && response.Status == "Success")
        //                {
        //                    FormData = JsonConvert.DeserializeObject<UserMasterDTO>(response.Data.ToString());
        //                    if (FormData != null)
        //                    {
        //                        IsUpdating = false;
        //                        sessionData.UserData = FormData;
        //                        NavigationManager.NavigateTo("dashboard");
        //                    }
        //                }
        //                else
        //                {

        //                    await dialogService.Alert("Invalid Username or Password", "Error", new AlertOptions() { OkButtonText = "Yes" });
        //                    IsUpdating = false;
        //                    IsLoading = false; // Hide loader
        //                }
        //                StateHasChanged();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await dialogService.Alert(ex.Message, "Communication Failure");
        //        IsUpdating = false;
        //        IsLoading = false; // Hide loader
        //    }
        //}




        //UserMasterDTO FormData { get; set; } = new UserMasterDTO();
        //    LoginDTO loginDTO { get; set; } = new LoginDTO();
        //    public bool IsUpdating { get; set; } = false;

            public bool hide { get; set; } = true;

            public DialogService dialogService1 { get; set; }
            
            private bool showErrorMessage = false; // Initially set to false

            private async Task SignIn()
            {
                try
                {
                    if (!IsUpdating)
                    {
                        IsUpdating = true;
                        if (loginDTO != null)
                        {
                        var response = await ciplapiservice.PostData(loginDTO, $"api/admin/dash/login");
                        if (response != null && response.Status == "Success")
                        {
                            FormData = JsonConvert.DeserializeObject<UserMasterDTO>(response.Data.ToString());
                            if (FormData != null)
                            {
                                IsUpdating = false;
                                sessionData.UserData = FormData;
                                NavigationManager.NavigateTo("dashboard");
                            }
                        }
                        else if (response != null && response.StatusCode == "Failure")
                            {
                                showErrorMessage = true;
                                IsUpdating = false;
                            }
                            else
                            {
                                await dialogService.Alert("Invalid Username or Password", "Error", new AlertOptions() { OkButtonText = "Yes" });
                                IsUpdating = false;
                                // await dialogService.Alert("Some message!", "MyTitle", new AlertOptions() { OkButtonText = "Yes" });
                            }
                            StateHasChanged();
                        }
                    }

                }
            catch (Exception ex)
            {
                await dialogService.Alert(ex.Message, "Communication Failure");
                IsUpdating = false;
            }
        }

        public async void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {

        }








    }
}
