using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace BroadcastsSchedule
{
    class Sheets
    {
        public static SheetsService AuthenticateOauthSheetService()
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = "3dmaya.com.ua@gmail.com";

            string[] scopes = new string[]
            {
                SheetsService.Scope.SpreadsheetsReadonly
            };

            try
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet.json");

                UserCredential credential =
                    GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets { ClientId = ClientID, ClientSecret = ClientSecret },
                        scopes,
                        UserName,
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore(credPath, true)).Result;

                SheetsService service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
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

        public static List<List<object>> GetCourses()
        {
            SheetsService Service = AuthenticateOauthSheetService();

            string SpreadsheetIDEmails = "1lE5SWFY7ZJizcNGKohtyg4T1JI3fZfCSbRdc5LrSdxk";
            String Range = "A1:C1";

            List<List<object>> Courses = new List<List<object>>();

            SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(SpreadsheetIDEmails, Range);
            ValueRange RequestValues = Request.Execute();
            var Values = RequestValues.Values;

            foreach (var Item in Values)
                Courses.Add(Item.ToList());

            return Courses;
        }

        public static List<List<object>> GetEMails(int Column)
        {
            SheetsService Service = AuthenticateOauthSheetService();

            string SpreadsheetIDEmails = "1lE5SWFY7ZJizcNGKohtyg4T1JI3fZfCSbRdc5LrSdxk";
            char SelItem = 'a';
            for (int i = 0; i < Column; i++)
                SelItem++;

            string Range = (SelItem.ToString() + 2 + ":" + SelItem.ToString());

            List<List<object>> Emails = new List<List<object>>();

            SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(SpreadsheetIDEmails, Range);
            ValueRange RequestValues = Request.Execute();
            var Values = RequestValues.Values;

            foreach (var Item in Values)
                Emails.Add(Item.ToList());

            return Emails;
        }       

        public static List<List<object>> GetLectures(string Course)
        {
            SheetsService Service = AuthenticateOauthSheetService();
            string SpreadsheetIDLectures = "1N4OTWU0u9e2UGtCmPomNao1w6TUaaJzKCA3PeGglPIg";

            string Range = "A2:D";
            List<List<object>> Lectures = new List<List<object>>();

            SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(SpreadsheetIDLectures, Range);
            ValueRange RequestValues = Request.Execute();
            var Values = RequestValues.Values;
            string LectureName;
            string LectureDate;
            string LectureTime;
            foreach (var Item in Values)
            {
                if (Item[0].ToString() == Course)
                {
                    LectureName = Item[1].ToString();
                    LectureDate = Item[2].ToString();
                    LectureTime = Item[3].ToString();
                    Lectures.Add(new List<object> { LectureName, LectureDate, LectureTime });
                }
            }
            return Lectures;
        }
           
    }
}