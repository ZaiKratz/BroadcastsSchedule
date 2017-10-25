using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Auth.OAuth2;

namespace BroadcastsSchedule
{

    class GoogleDrive
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Broadcasts Schedule";
        private static string _YouTubePicturesFolderID = "0B4PpJ-s2evO5NUE4VktUVUVVOU0";
        public static string YouTubePicturesFolderID
        {
            get
            {
                return _YouTubePicturesFolderID;
            }
            set
            {
                _YouTubePicturesFolderID = value;
            }
        }

        public static DriveService AuthenticateOauth()
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = "3dmaya.com.ua@gmail.com";

            var credPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            credPath = System.IO.Path.Combine(credPath, ".credentials", UserName, "drive.googleapis.com-dotnet.json");

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential =
                    GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                        Scopes,
                        UserName,
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore(credPath, true)).Result;
                DriveService service = new DriveService(new DriveService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                return service;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }


        public static System.IO.MemoryStream GetImage(DriveService Service, string Course)
        {
            string PageToken = null;
            File Image = null;
            System.IO.MemoryStream Stream = null;
            if (Service != null)
            {
                try
                {
                    do
                    {
                        var ListRequest = Service.Files.List();
                        ListRequest.Q = "mimeType='image/jpeg'";
                        ListRequest.Spaces = "drive";
                        ListRequest.PageToken = PageToken;
                        var Result = ListRequest.Execute();

                        foreach (var file in Result.Items)
                        {
                            foreach (var parent in file.Parents)
                            {
                                if (parent.Id == YouTubePicturesFolderID)
                                {
                                    if (file.Title.ToLower().Contains(Course.ToLower()))
                                    {
                                        Image = file;
                                        break;
                                    }
                                }
                            }
                        }
                        if (Image != null)
                            break;
                        PageToken = Result.NextPageToken;
                    } while (string.IsNullOrEmpty(PageToken));

                    if (Image != null)
                    {
                        var GetRequest = Service.Files.Get(Image.Id);
                        GetRequest.Download(Stream = new System.IO.MemoryStream());
                    }
                    if (Stream != null)
                        return Stream;
                    return null;
                }
                catch (Google.GoogleApiException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            else
                return null;
        }
    }


}
