﻿using System;
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
        private static string EmailsSpreadsheetID = "1duZ1lhwlBppThjRIAT09b5APSGkrLp35jXwFEHa0yf4";
        private static string LecturesSpreadsheetID = "1pXBy3i1FpJ0VCJng78uuDeJu4xN49sUNiTy7NulARJo";

        public static List<string> GetTablesIDs()
        {
            return new List<string> { EmailsSpreadsheetID, LecturesSpreadsheetID };
        }

        public static void SetTablesIDs(string EmailsTableID, string LecturesTableID)
        {
            EmailsSpreadsheetID = EmailsTableID;
            LecturesSpreadsheetID = LecturesTableID;
        }

        public static SheetsService AuthenticateOauth(string User)
        {
            string ClientID = "332318931456-l5ob314tvkjs593ae07f6ialut3c9560.apps.googleusercontent.com";
            string ClientSecret = "kVnZk-9oPd_t0J50qJG_wwmn";
            string UserName = User;            

            string[] scopes = new string[]
            {
                SheetsService.Scope.Spreadsheets
            };

            try
            {
                var credPath = System.IO.Directory.GetCurrentDirectory();
                credPath = Path.Combine(credPath, ".credentials", User, "sheets.googleapis.com-dotnet.json");

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
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }

        }

        public static List<List<object>> GetCourses(SheetsService Service)
        {
            
            String Range = "A1:1";

            List<List<object>> Courses = new List<List<object>>();

            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(EmailsSpreadsheetID, Range);
                ValueRange RequestValues = Request.Execute();
                var Values = RequestValues.Values;

                foreach (var Item in Values)
                    Courses.Add(Item.ToList());
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return Courses;
        }

        public static void EditEmailsSheet(SheetsService Service, int CourseColumnID, List<IList<object>> Emails)
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

        public static List<List<object>> GetEMails(SheetsService Service, int CourseColumnID)
        {
            char SelItem = 'a';
            for (int i = 0; i < CourseColumnID; i++)
                SelItem++;

            string Range = (SelItem.ToString() + 2 + ":" + SelItem.ToString());

            List<List<object>> Emails = null;

            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(EmailsSpreadsheetID, Range);
                ValueRange RequestValues = Request.ExecuteAsync().Result;
                var Values = RequestValues.Values;
                if(Values != null)
                {
                    Emails = new List<List<object>>();
                    foreach (var Item in Values)
                        Emails.Add(Item.ToList());
                }
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return Emails;
        }       

        public static List<List<object>> GetLectures(SheetsService Service, string Course)
        { 
            string Range = "A2:E";
            List<List<object>> Lectures = new List<List<object>>();

            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest Request = Service.Spreadsheets.Values.Get(LecturesSpreadsheetID, Range);
                ValueRange RequestValues = Request.ExecuteAsync().Result;
                var Values = RequestValues.Values;
                string LectureName;
                string LectureDate;
                string LectureTime;
                string LectureDescription;
                foreach (var Item in Values)
                {
                    if (Item[0].ToString().ToLower().Contains(Course.ToLower()))
                    {
                        LectureName = Item[1].ToString();
                        LectureDate = Item[2].ToString();
                        LectureTime = Item[3].ToString();
                        LectureDescription = Item[4].ToString();
                        Lectures.Add(new List<object> { LectureName, LectureDate, LectureTime, LectureDescription });
                    }
                }
            }
            catch (Google.GoogleApiException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Error.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return Lectures;
        }
           
    }
}