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
        private static string UserName = null;
        private static YouTubeService Service = null;

        public static async System.Threading.Tasks.Task AuthenticateOauth(string User)
        {
            if(User != null)
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

                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = await
                    GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                        scopes,
                        UserName,
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore(credPath, true));
                Service = new YouTubeService(new YouTubeService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Broadcasts Schedule",
                });

            }

        }

        public static LiveBroadcast StartEvent(string BroadcastId, string StreamId)
        {
            LiveBroadcast Broadcast = null;
            if (Service != null)
            {
                Program.BSForm.SetStatus("Checking for OBS connection...");
                while (GetStreamByID(StreamId).Status.StreamStatus != "active")
                {
                    Program.BSForm.SetStatus("Waiting for OBS connection...");
                }
                Program.BSForm.SetStatus("OBS connected");


                try
                {
                    LiveBroadcastsResource.TransitionRequest Testeq =
                    Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Testing,
                    BroadcastId, BroadcastPart);
                    Broadcast = Testeq.Execute();


                    while (GetBroadcast(BroadcastId).Status.LifeCycleStatus != "testing")
                    {
                        Program.BSForm.SetStatus("Testing connection...");
                    }

                    LiveBroadcastsResource.TransitionRequest LiveReq =
                        Service.LiveBroadcasts.Transition(LiveBroadcastsResource.TransitionRequest.BroadcastStatusEnum.Live,
                        BroadcastId, BroadcastPart);
                    Broadcast = LiveReq.Execute();

                    Program.BSForm.SetStatus("Now You are Live!");
                }
                catch (Google.GoogleApiException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while starting broadcast. Cancel it and try again.");
                    return null;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while starting broadcast. Cancel it and try again.");
                    return null;
                }     
            }
            return Broadcast;
        }
        
        public static LiveBroadcast EndEvent(string BroadcastId)
        {
            Program.BSForm.SetStatus("Ending Stream...");
            LiveBroadcast Broadcast = null;

            if(Service != null)
            {
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
                    Program.BSForm.SetStatus("En error was occurred while ending broadcast.");
                    return null;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while ending broadcast.");
                    return null;
                }
            }
            return Broadcast;
        }

        public static bool DeleteEvent(string BroadcastId)
        {
            if(Service != null)
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
                    Program.BSForm.SetStatus("En error was occurred while deleting broadcast.");
                    return false;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast.");
                    return false;
                }
            }
            return false;
        }

        public static List<LiveBroadcast> CreateListOfLiveEvents(List<BroadcastData> ListOfLiveEventsData)
        {
            List<LiveBroadcast> liveBroadcasts = new List<LiveBroadcast>();

            foreach(var Data in ListOfLiveEventsData)
            {
                LiveBroadcast liveBroadcast = CreateLiveEvent(Data);
                if (liveBroadcast != null)
                    liveBroadcasts.Add(liveBroadcast);
            }

            return liveBroadcasts;
        }

        public static LiveBroadcast CreateLiveEvent(BroadcastData broadcastData)
        {
            Program.BSForm.SetStatus("Creatint Live Broadcast: " + broadcastData.broadcastName);
            LiveBroadcast Broadcast = null;
            if (Service != null)
            {
                var Stream = GetStreamByTitle(broadcastData.streamTitle);
                if(Stream != null)
                {
                    Broadcast = new LiveBroadcast
                    {
                        Snippet = new LiveBroadcastSnippet
                        {
                            Title = broadcastData.broadcastName,
                            Description = broadcastData.broadcastDescription,
                            ScheduledStartTime = broadcastData.scheduledDateTime.ToUniversalTime()
                        },


                        Status = new LiveBroadcastStatus
                        {
                            PrivacyStatus = "private"
                        },

                        ContentDetails = new LiveBroadcastContentDetails
                        {
                            RecordFromStart = true,
                            EnableDvr = true
                        },

                        Kind = "youtube#liveBroadcast"
                    };

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
                            System.Windows.Forms.MessageBox.Show("Stream for " + broadcastData.streamTitle + " not found", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            Program.BSForm.SetStatus("En error was occurred while creating broadcast " + broadcastData.broadcastName);
                            Broadcast = null;
                            return Broadcast;
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

                        Program.BSForm.SetStatus("Preparing to download image form drive...");
                        System.IO.MemoryStream Image = GoogleDrive.GetImage(GoogleDrive.AuthenticateOauth(), Stream.Snippet.Title);

                        if (Image != null)
                        {
                            Program.BSForm.SetStatus("Setting up downloaded image...");
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
                        Program.BSForm.SetStatus("En error was occurred while creating broadcast" + broadcastData.broadcastName);
                        Broadcast = null;
                        return Broadcast;
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        Program.BSForm.SetStatus("En error was occurred while creating broadcast" + broadcastData.broadcastName);
                        Broadcast = null;
                        return Broadcast;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Stream is not found.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast" + broadcastData.broadcastName);
                    Broadcast = null;
                    return Broadcast;
                }
            }
            else
            {
                Program.BSForm.SetStatus("En error was occurred while creating broadcast" + broadcastData.broadcastName);
                Broadcast = null;
            }
            Program.BSForm.SetStatus("Live Broadcast " + broadcastData.broadcastName + " was created");
            return Broadcast;
        }

        public static void GetStreams(ref List<LiveStream> StreamsList)
        {
            if(Service != null)
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
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        public static LiveStream GetStreamByTitle(string StreamTitle)
        {
            if(Service != null)
            {
                var Request = Service.LiveStreams.List(StreamPart);
                Request.Mine = true;

                try
                {
                    var ReturnedResponce = Request.Execute();

                    foreach (var Item in ReturnedResponce.Items)
                    {
                        if (Item.Snippet.Title.ToString().ToLower() == (StreamTitle.ToLower()))
                            return Item;
                    }
                }
                catch (Google.GoogleApiException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast. Cancel it and try again.");
                    return null;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast. Cancel it and try again.");
                    return null;
                }
            }
            return null;
        }

        public static LiveStream GetStreamByID(string StreamID)
        {
            if(Service != null)
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
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast. Cancel it and try again.");
                    return null;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Program.BSForm.SetStatus("En error was occurred while creating broadcast. Cancel it and try again.");
                    return null;
                }
            }
            return null;
        }

        public static LiveBroadcast GetBroadcast(string BroadcastId)
        {
            if (Service != null)
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
            }
            return null;
        }

        public static List<LiveBroadcast> GetScheduledBroadcasts()
        {
            if (Service != null)
            {
                var Request = Service.LiveBroadcasts.List(BroadcastPart);
                Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.Upcoming;

                try
                {
                    var ReturnedResponce = Request.Execute();
                    if (ReturnedResponce.Items.Count > 0)
                        return ReturnedResponce.Items as List<LiveBroadcast>;
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
            }
            return null;
        }

        public static void GetOnlineBroadcasts(ref List<LiveBroadcast> list)
        {
            if (Service != null)
            {
                var Request = Service.LiveBroadcasts.List(BroadcastPart);
                Request.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.Active;

                try
                {
                    var ReturnedResponce = Request.Execute();
                    if (ReturnedResponce.Items.Count > 0)
                        list = ReturnedResponce.Items as List<LiveBroadcast>;
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
        }
    }
}
