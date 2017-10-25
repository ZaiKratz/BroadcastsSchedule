using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace BroadcastsSchedule
{
    class GoogleSheets
    {
        private static string EmailsSpreadsheetID = "1duZ1lhwlBppThjRIAT09b5APSGkrLp35jXwFEHa0yf4";
        private static string LecturesSpreadsheetID = "1pXBy3i1FpJ0VCJng78uuDeJu4xN49sUNiTy7NulARJo";
        private static SheetsService Service = null;

        public static List<string> GetTablesIDs()
        {
            return new List<string> { EmailsSpreadsheetID, LecturesSpreadsheetID };
        }

        public static void SetTablesIDs(string EmailsTableID, string LecturesTableID)
        {
            EmailsSpreadsheetID = EmailsTableID;
            LecturesSpreadsheetID = LecturesTableID;
        }

        public static async System.Threading.Tasks.Task AuthenticateOauth(string User)
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = User;

            string[] scopes = new string[]
            {
                SheetsService.Scope.Spreadsheets
            };

            var credPath = System.IO.Directory.GetCurrentDirectory();
            credPath = Path.Combine(credPath, ".credentials", User, "sheets.googleapis.com-dotnet.json");

            UserCredential credential = await
                GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                    scopes,
                    UserName,
                    System.Threading.CancellationToken.None,
                    new Google.Apis.Util.Store.FileDataStore(credPath, true));

            Service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Broadcasts Schedule",
            });

        }

        public static List<List<object>> GetCourses()
        {
            
            String Range = "A1:1";

            List<List<object>> Courses = null;
            if(Service != null)
            {
                try
                {
                    SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(EmailsSpreadsheetID, Range);
                    ValueRange RequestValues = Request.Execute();
                    var Values = RequestValues.Values;
                    Courses = new List<List<object>>();
                    foreach (var Item in Values)
                        Courses.Add(Item.ToList());
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

            return Courses;
        }

        public static void EditEmailsSheet(int CourseColumnID, List<IList<object>> Emails)
        {
            if(Service != null)
            {
                char SelItem = 'a';
                for (int i = 0; i < CourseColumnID; i++)
                    SelItem++;

                string Range = (SelItem.ToString() + 2 + ":" + SelItem.ToString());

                ValueRange Body = new ValueRange();
                Body.Values = Emails;
                Body.Range = Range;

                var ValueRes = Service.Spreadsheets.Values.Update(Body, EmailsSpreadsheetID, Range);
                ValueRes.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.INPUTVALUEOPTIONUNSPECIFIED;
                //ValueRes.Execute();

                //UpdateValuesResponse Result = Service.Spreadsheets.Values.Update(Body, EmailsSpreadsheetID, Range).Execute();
                //ValueRange Body = new ValueRange();
                //Body.Values = Emails;
                //Body.Range = Range;
                //List<ValueRange> data = new List<ValueRange>();
                //data.Add(Body);

                //BatchUpdateValuesRequest body = new BatchUpdateValuesRequest();
                //body.ValueInputOption = "USER_ENTERED";
                //body.Data = data;

                //BatchUpdateValuesResponse result = Service.Spreadsheets.Values.BatchUpdate(body, EmailsSpreadsheetID).Execute();
            }
        }

        public static List<List<object>> GetEMails(int CourseColumnID)
        {
            char SelItem = 'a';
            for (int i = 0; i < CourseColumnID; i++)
                SelItem++;

            string Range = (SelItem.ToString() + 2 + ":" + SelItem.ToString());

            List<List<object>> Emails = null;

            if(Service != null)
            {
                try
                {
                    SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(EmailsSpreadsheetID, Range);
                    ValueRange RequestValues = Request.Execute();
                    var Values = RequestValues.Values;
                    if (Values != null)
                    {
                        Emails = new List<List<object>>();
                        foreach (var Item in Values)
                            Emails.Add(Item.ToList());
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
            }

            return Emails;
        }       

        public static List<List<object>> GetLectures(string Course)
        { 
            string Range = "A2:E";
            List<List<object>> Lectures = null;

            if(Service != null)
            {
                try
                {
                    SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(LecturesSpreadsheetID, Range);
                    ValueRange RequestValues = Request.Execute();
                    var Values = RequestValues.Values;
                    string LectureName = "";
                    string LectureDate = "";
                    string LectureTime = "";
                    string LectureDescription = "";
                    Lectures = new List<List<object>>();

                    if (Values != null)
                        foreach (var Item in Values)
                        {
                            if (Item[0].ToString().ToLower().Contains(Course.ToLower()))
                            {
                                LectureName = Item.ElementAtOrDefault(1) != null ? Item.ElementAtOrDefault(1).ToString() : "";
                                LectureDate = Item.ElementAtOrDefault(2) != null ? Item.ElementAtOrDefault(2).ToString() : "";
                                LectureTime = Item.ElementAtOrDefault(3) != null ? Item.ElementAtOrDefault(3).ToString() : "";
                                LectureDescription = Item.ElementAtOrDefault(4) != null ? Item.ElementAtOrDefault(4).ToString() : "";
                                Lectures.Add(new List<object> { LectureName, LectureDate, LectureTime, LectureDescription });
                            }

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
            }

            return Lectures;
        }
           
    }
}