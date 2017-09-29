using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;

namespace BroadcastsSchedule
{
    class YoutubeStream
    {
        private static int FUTURE_DATE_OFFSET_MILLIS = 5 * 1000;

        public static YouTubeService AuthenticateOauth()
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = "3dmaya.com.ua@gmail.com";
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
        public static void StartEvent(YouTubeService Service, string BroadcastId)
        {
            var BroadcastPart = "id,snippet,contentDetails,status";
            //var Service = AuthenticateOauth();

            LiveBroadcastsResource.TransitionRequest TransReq = 
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Live,
                BroadcastId, BroadcastPart);
            TransReq.Execute();
        }

        public static void EndEvent(YouTubeService Service, string BroadcastId)
        {
            var BroadcastPart = "id,snippet,contentDetails,status";
            //var Service = AuthenticateOauth();

            LiveBroadcastsResource.TransitionRequest TransReq =
                Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Complete,
                BroadcastId, BroadcastPart);
            TransReq.Execute();
        }

        public static void CreateLiveEvent(YouTubeService Service, string Name, string Description)
        {
            var StreamPart = "id, snippet, cdn, contentDetails, status";
            var BroadcastPart = "id,snippet,contentDetails,status";
            LiveStream Stream = new LiveStream();

            Stream.Snippet = new LiveStreamSnippet();
            Stream.Snippet.Title = Name;
            Stream.Snippet.Description = Description;

            Stream.Cdn = new CdnSettings();
            Stream.Cdn.Format = "1080p_hfr:";
            Stream.Cdn.IngestionType = "dash";
            Stream.Cdn.FrameRate = "60fps";
            Stream.Cdn.Resolution = "1080p";
            Stream.Kind = "youtube#liveStream";

            LiveStreamsResource.InsertRequest InsertStreamRequest = Service.LiveStreams.Insert(Stream, StreamPart);
            Stream = InsertStreamRequest.Execute();

            
            LiveBroadcast Broadcast = new LiveBroadcast();

            Broadcast.Snippet = new LiveBroadcastSnippet();
            Broadcast.Snippet.Title = "Some";
            Broadcast.Snippet.Description = "Test broadcast";
            Broadcast.Snippet.ScheduledStartTime = DateTime.UtcNow.AddMilliseconds(FUTURE_DATE_OFFSET_MILLIS);
            

            Broadcast.Status = new LiveBroadcastStatus();
            Broadcast.Status.PrivacyStatus = "private";

            Broadcast.ContentDetails = new LiveBroadcastContentDetails();
            Broadcast.ContentDetails.RecordFromStart = true;
            Broadcast.ContentDetails.EnableDvr = true;

            Broadcast.Kind = "youtube#liveBroadcast";

            LiveBroadcastsResource.InsertRequest InsertRequest = Service.LiveBroadcasts.Insert(Broadcast, BroadcastPart);
            Broadcast = InsertRequest.Execute();
            

            LiveBroadcastsResource.BindRequest BindRequest = Service.LiveBroadcasts.Bind(Broadcast.Id, BroadcastPart);
            BindRequest.StreamId = Stream.Id;
            Broadcast = BindRequest.Execute();
        }

        public static void GetBroadcasts(ref List<LiveBroadcast> BroadcastsList, YouTubeService Service, string Part)
        {
            BroadcastsList.Clear();
            var Request = Service.LiveBroadcasts.List(Part);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;
            var ReturnedResponce = Request.Execute();
            BroadcastsList = ReturnedResponce.Items as List<LiveBroadcast>;
        }

        public static LiveBroadcast GetBroadcast(string BroadcastTitle, YouTubeService Service, string Part)
        {
            var Request = Service.LiveBroadcasts.List(Part);
            Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.All;
            var ReturnedResponce = Request.Execute();
            
            foreach(var Item in ReturnedResponce.Items)
            {
                if (Item.Snippet.Title.ToString() == BroadcastTitle)
                    return Item;
            }

            return null;
        }
    }
}
