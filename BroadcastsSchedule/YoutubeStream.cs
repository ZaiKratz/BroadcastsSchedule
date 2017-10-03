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

        public static YouTubeService AuthenticateOauth()
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = "sadovnichy95@gmail.com";
            string[] scopes = new string[]
            {
                YouTubeService.Scope.Youtube,  // view and manage your YouTube account
                YouTubeService.Scope.YoutubeForceSsl,
                YouTubeService.Scope.Youtubepartner,
                YouTubeService.Scope.YoutubepartnerChannelAudit,
                YouTubeService.Scope.YoutubeReadonly,
                YouTubeService.Scope.YoutubeUpload
            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential =
                    GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                        scopes,
                        UserName,
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore("Daimto.YouTube.Auth.Store")).Result;

                YouTubeService service = new YouTubeService(new YouTubeService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Broadcasts Schedule",
                });
                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }

        }

        public static void TestEvent(YouTubeService Service, string BroadcastId)
        {
            LiveBroadcastsResource.TransitionRequest Testeq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Testing,
                BroadcastId, BroadcastPart);
            Testeq.ExecuteAsync();
        }

        public static void StartEvent(YouTubeService Service, string BroadcastId, string StreamId)
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
            Testeq.ExecuteAsync();

            while (GetBroadcast(BroadcastId, Service).Status.LifeCycleStatus != "testing")
            {
                Program.BSForm.SetLabelText("Testing connection...");
            } 

            LiveBroadcastsResource.TransitionRequest LiveReq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Live,
                BroadcastId, BroadcastPart);
            LiveReq.ExecuteAsync();

            Program.BSForm.SetLabelText("Now You are Live!");
        }
        

        public static void EndEvent(YouTubeService Service, string BroadcastId)
        {
            LiveBroadcastsResource.TransitionRequest TransReq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Complete,
                BroadcastId, BroadcastPart);
            TransReq.ExecuteAsync();
        }

        public static void DeleteLiveEvent(YouTubeService Service, string BroadcastId)
        {
            LiveBroadcastsResource.DeleteRequest DelTrans =
                Service.LiveBroadcasts.Delete(BroadcastId);
            DelTrans.ExecuteAsync();
        }

        public static LiveBroadcast CreateLiveEvent(YouTubeService Service, string CourseName, string Name, string Description)
        {
            //             LiveStream Stream = new LiveStream();
            // 
            //             Stream.Snippet = new LiveStreamSnippet();
            //             Stream.Snippet.Title = Name;
            //             Stream.Snippet.Description = Description;
            // 
            //             Stream.Cdn = new CdnSettings();
            //             Stream.Cdn.Format = "1080p_hfr:";
            //             Stream.Cdn.IngestionType = "dash";
            //             Stream.Cdn.FrameRate = "60fps";
            //             Stream.Cdn.Resolution = "1080p";
            //             Stream.Kind = "youtube#liveStream";
            // 
            //             LiveStreamsResource.InsertRequest InsertStreamRequest = Service.LiveStreams.Insert(Stream, StreamPart);
            //             Stream = InsertStreamRequest.Execute();
            
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

            LiveBroadcastsResource.InsertRequest InsertRequest = Service.LiveBroadcasts.Insert(Broadcast, BroadcastPart);
            Broadcast =  InsertRequest.ExecuteAsync().Result;
            

            LiveBroadcastsResource.BindRequest BindRequest = Service.LiveBroadcasts.Bind(Broadcast.Id, BroadcastPart);
            BindRequest.StreamId = Stream.Id;
            Broadcast = BindRequest.ExecuteAsync().Result;
            
            return Broadcast;
        }

        public static void GetStreams(ref List<LiveStream> StreamsList, YouTubeService Service)
        {
            StreamsList.Clear();
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;
            var ReturnedResponce = Request.ExecuteAsync().Result;
            StreamsList = ReturnedResponce.Items as List<LiveStream>;
        }

        public static LiveStream GetStreamByTitle(string StreamTitle, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;
            var ReturnedResponce = Request.ExecuteAsync().Result;

            foreach (var Item in ReturnedResponce.Items)
            {
                if (Item.Snippet.Title.ToString().ToLower().Contains(StreamTitle))
                    return Item;
            }

            return null;
        }

        public static LiveStream GetStreamByID(string StreamID, YouTubeService Service)
        {
            var Request = Service.LiveStreams.List(StreamPart);
            Request.Mine = true;
            var ReturnedResponce = Request.ExecuteAsync().Result;

            foreach (var Item in ReturnedResponce.Items)
            {
                if (Item.Id.ToString() == (StreamID))
                    return Item;
            }

            return null;
        }

        public static void GetBroadcasts(ref List<LiveBroadcast> BroadcastsList, YouTubeService Service)
        {
            BroadcastsList.Clear();
            var Request = Service.LiveBroadcasts.List(BroadcastPart);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;
            var ReturnedResponce = Request.ExecuteAsync().Result;
            BroadcastsList = ReturnedResponce.Items as List<LiveBroadcast>;
        }

        public static LiveBroadcast GetBroadcast(string BroadcastId, YouTubeService Service)
        {
            var Request = Service.LiveBroadcasts.List(BroadcastPart);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;
            var ReturnedResponce = Request.ExecuteAsync().Result;
            
            foreach(var Item in ReturnedResponce.Items)
            {
                if (Item.Id.ToString() == BroadcastId)
                    return Item;
            }

            return null;
        }
    }
}
