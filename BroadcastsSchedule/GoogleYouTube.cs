using System;
using System.Collections.Generic;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;

namespace BroadcastsSchedule
{
    class GoogleYouTube
    {
        private static string BroadcastPart = "id,snippet,contentDetails,status";
        private static string StreamPart = "id, snippet, cdn, contentDetails, status";
        static string UserName = null;
        public static YouTubeService AuthenticateOauth(string User)
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            UserName = User;
            string[] scopes = new string[]
            {
                YouTubeService.Scope.Youtube,  // view and manage your YouTube account
                YouTubeService.Scope.YoutubeForceSsl,
                YouTubeService.Scope.Youtubepartner,
                YouTubeService.Scope.YoutubepartnerChannelAudit,
                YouTubeService.Scope.YoutubeUpload
            };

            var credPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            credPath = System.IO.Path.Combine(credPath, ".credentials", User, "youtube.googleapis.com-dotnet.json");

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential =
                    GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                        scopes,
                        UserName,
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore(credPath, true)).Result;
                YouTubeService service = new YouTubeService(new YouTubeService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Broadcasts Schedule",
                });
                return service;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }

        }

        public static LiveBroadcast StartEvent(YouTubeService Service, string BroadcastId, string StreamId)
        {
            LiveBroadcast Broadcast = null;

            Program.BSForm.SetLabelText("Checking for OBS connection...");
            while (GetStreamByID(StreamId, Service).Status.StreamStatus != "active")
            {
                Program.BSForm.SetLabelText("Waiting for OBS connection...");
            }
            Program.BSForm.SetLabelText("OBS connected");


            try
            {
                LiveBroadcastsResource.TransitionRequest Testeq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Testing,
                BroadcastId, BroadcastPart);
                Broadcast = Testeq.Execute();



                while (GetBroadcast(BroadcastId, Service).Status.LifeCycleStatus != "testing")
                {
                    Program.BSForm.SetLabelText("Testing connection...");
                }

                LiveBroadcastsResource.TransitionRequest LiveReq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Live,
                    BroadcastId, BroadcastPart);
                Broadcast = LiveReq.Execute();

                Program.BSForm.SetLabelText("Now You are Live!");
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
            return Broadcast;
        }
        

        public static LiveBroadcast EndEvent(YouTubeService Service, string BroadcastId)
        {
            Program.BSForm.SetLabelText("Ending Stream...");
            LiveBroadcast Broadcast = null;

            try
            {
                LiveBroadcastsResource.TransitionRequest TransReq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Complete,
                BroadcastId, BroadcastPart);
                Broadcast = TransReq.Execute();
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
            return Broadcast;
        }

        public static bool DeleteEvent(YouTubeService Service, string BroadcastId)
        {
            try
            {
                LiveBroadcastsResource.DeleteRequest DelTrans =
                Service.LiveBroadcasts.Delete(BroadcastId);
                DelTrans.Execute();
                return true;
            }
            catch (Google.GoogleApiException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static LiveBroadcast CreateLiveEvent(YouTubeService Service, string CourseName, string Name, DateTime Date, string Description)
        {
            var Stream = GetStreamByTitle(CourseName, Service);

            LiveBroadcast Broadcast = new LiveBroadcast();

            Broadcast.Snippet = new LiveBroadcastSnippet();
            Broadcast.Snippet.Title = Name;
            Broadcast.Snippet.Description = Description;
            Broadcast.Snippet.ScheduledStartTime = Date.ToUniversalTime();


            Broadcast.Status = new LiveBroadcastStatus();
            Broadcast.Status.PrivacyStatus = "private";

            Broadcast.ContentDetails = new LiveBroadcastContentDetails();
            Broadcast.ContentDetails.RecordFromStart = true;
            Broadcast.ContentDetails.EnableDvr = true;

            Broadcast.Kind = "youtube#liveBroadcast";

            try
            {

                LiveBroadcastsResource.InsertRequest InsertRequest = Service.LiveBroadcasts.Insert(Broadcast, BroadcastPart);
                Broadcast = InsertRequest.Execute();

                if (Stream != null)
                {
                    LiveBroadcastsResource.BindRequest BindRequest = Service.LiveBroadcasts.Bind(Broadcast.Id, BroadcastPart);
                    BindRequest.StreamId = Stream.Id;
                    Broadcast = BindRequest.Execute();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Stream for " + CourseName + " not found", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

                var ListRequest = Service.Videos.List(BroadcastPart);
                ListRequest.Id = Broadcast.Id;

                var ListResponce = ListRequest.Execute();

                foreach (var BroadcastVideo in ListResponce.Items)
                {
                    BroadcastVideo.Snippet.CategoryId = "27";
                    var UpdateRequest = Service.Videos.Update(BroadcastVideo, BroadcastPart);
                    UpdateRequest.Execute();
                }

                Program.BSForm.SetLabelText("Preparing to download image form drive...");
                System.IO.MemoryStream Image = GoogleDrive.GetImage(GoogleDrive.AuthenticateOauth(), Stream.Snippet.Title);

                if (Image != null)
                {
                    Program.BSForm.SetLabelText("Setting up downloaded image...");
                    var InputStream = new System.IO.BufferedStream(Image);
                    var req = Service.Thumbnails.Set(Broadcast.Id, InputStream, "image/jpeg");
                    req.Upload();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Image \"" + Stream.Snippet.Title + ".jpg\" not found", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
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

            return Broadcast;
        }

        public static void GetStreams(ref List<LiveStream> StreamsList, YouTubeService Service)
        {
            StreamsList.Clear();
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;
            try
            {
                var ReturnedResponce = Request.Execute();
                StreamsList = ReturnedResponce.Items as List<LiveStream>;
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch(System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static LiveStream GetStreamByTitle(string StreamTitle, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;

            try
            {
                var ReturnedResponce = Request.Execute();

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Snippet.Title.ToString().ToLower().Contains(StreamTitle))
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            return null;
        }

        public static LiveStream GetStreamByID(string StreamID, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;

            try
            {
                var ReturnedResponce = Request.Execute();

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Id.ToString() == (StreamID))
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            return null;
        }

        public static void GetBroadcasts(ref List<LiveBroadcast> BroadcastsList, YouTubeService Service)
        {
            BroadcastsList.Clear();
            var Request = Service.LiveBroadcasts.List(BroadcastPart);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;
            try
            {
                var ReturnedResponce = Request.Execute();
                BroadcastsList = ReturnedResponce.Items as List<LiveBroadcast>;
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
        }

        public static LiveBroadcast GetBroadcast(string BroadcastId, YouTubeService Service)
        {
            var Request = Service.LiveBroadcasts.List(BroadcastPart);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;

            try
            {
                var ReturnedResponce = Request.Execute();

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Id.ToString() == BroadcastId)
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            return null;
        }
    }
}
