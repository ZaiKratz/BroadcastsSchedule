using System;
using System.Collections.Generic;
using Google.Apis.YouTube;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;
using System.Windows.Forms;

namespace BroadcastsSchedule
{
    class YoutubeStream
    {
        private static int FUTURE_DATE_OFFSET_MILLIS = 5 * 1000;
        private static string BroadcastPart = "id,snippet,contentDetails,status";
        private static string StreamPart = "id, snippet, cdn, contentDetails, status";

        public static YouTubeService AuthenticateOauth(string User)
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = User;
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        public static void TestEvent(YouTubeService Service, string BroadcastId)
        {
            try
            {
                LiveBroadcastsResource.TransitionRequest Testeq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Testing,
                    BroadcastId, BroadcastPart);
                Testeq.ExecuteAsync();
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static LiveBroadcast StartEvent(YouTubeService Service, string BroadcastId, string StreamId)
        {
            LiveBroadcast Broadcast = null;
            try
            {
                var BroadcastStatus = GetBroadcast(BroadcastId, Service).Status;

                Program.BSForm.SetLabelText("Checking for OBS connection...");
                while (GetStreamByID(StreamId, Service).Status.StreamStatus != "active")
                {
                    Program.BSForm.SetLabelText("Waiting for OBS connection...");
                }
                Program.BSForm.SetLabelText("OBS connected");


                LiveBroadcastsResource.TransitionRequest Testeq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Testing,
                    BroadcastId, BroadcastPart);
                Broadcast = Testeq.ExecuteAsync().Result;

                while (GetBroadcast(BroadcastId, Service).Status.LifeCycleStatus != "testing")
                {
                    Program.BSForm.SetLabelText("Testing connection...");
                }

                LiveBroadcastsResource.TransitionRequest LiveReq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Live,
                    BroadcastId, BroadcastPart);
                Broadcast = LiveReq.ExecuteAsync().Result;

                Program.BSForm.SetLabelText("Now You are Live!");
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Broadcast;
        }
        

        public static LiveBroadcast EndEvent(YouTubeService Service, string BroadcastId)
        {
            LiveBroadcast Broadcast = null;
            try
            {
                LiveBroadcastsResource.TransitionRequest TransReq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Complete,
                    BroadcastId, BroadcastPart);
                Broadcast = TransReq.ExecuteAsync().Result;
            }
            catch(Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Broadcast;
        }

        public static void DeleteEvent(YouTubeService Service, string BroadcastId)
        {
            try
            {
                LiveBroadcastsResource.DeleteRequest DelTrans =
                    Service.LiveBroadcasts.Delete(BroadcastId);
                DelTrans.ExecuteAsync();
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static LiveBroadcast CreateLiveEvent(YouTubeService Service, string CourseName, string Name, string Description)
        {
            var Stream = GetStreamByTitle(CourseName, Service);

            LiveBroadcast Broadcast = new LiveBroadcast();

            Broadcast.Snippet = new LiveBroadcastSnippet();
            Broadcast.Snippet.Title = Name;
            Broadcast.Snippet.Description = Description;
            Broadcast.Snippet.ScheduledStartTime = DateTime.UtcNow.AddMilliseconds(FUTURE_DATE_OFFSET_MILLIS);
            

            Broadcast.Status = new LiveBroadcastStatus();
            Broadcast.Status.PrivacyStatus = "private";

            Broadcast.ContentDetails = new LiveBroadcastContentDetails();
            Broadcast.ContentDetails.RecordFromStart = true;
            Broadcast.ContentDetails.EnableDvr = true;

            Broadcast.Kind = "youtube#liveBroadcast";

            try
            {
                LiveBroadcastsResource.InsertRequest InsertRequest = Service.LiveBroadcasts.Insert(Broadcast, BroadcastPart);
                Broadcast = InsertRequest.ExecuteAsync().Result;

                LiveBroadcastsResource.BindRequest BindRequest = Service.LiveBroadcasts.Bind(Broadcast.Id, BroadcastPart);
                BindRequest.StreamId = Stream.Id;
                Broadcast = BindRequest.ExecuteAsync().Result;
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var ReturnedResponce = Request.ExecuteAsync().Result;
                StreamsList = ReturnedResponce.Items as List<LiveStream>;
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static LiveStream GetStreamByTitle(string StreamTitle, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;

            try
            {
                var ReturnedResponce = Request.ExecuteAsync().Result;

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Snippet.Title.ToString().ToLower().Contains(StreamTitle))
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public static LiveStream GetStreamByID(string StreamID, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;

            try
            {
                var ReturnedResponce = Request.ExecuteAsync().Result;

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Id.ToString() == (StreamID))
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var ReturnedResponce = Request.ExecuteAsync().Result;
                BroadcastsList = ReturnedResponce.Items as List<LiveBroadcast>;
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static LiveBroadcast GetBroadcast(string BroadcastId, YouTubeService Service)
        {
            var Request = Service.LiveBroadcasts.List(BroadcastPart);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;

            try
            {
                var ReturnedResponce = Request.ExecuteAsync().Result;

                foreach (var Item in ReturnedResponce.Items)
                {
                    if (Item.Id.ToString() == BroadcastId)
                        return Item;
                }
            }
            catch (Google.GoogleApiException e)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}
