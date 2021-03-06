﻿using Ghasedak.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using static Ghasedak.Models.Results;
using Ghasedak.Models;

namespace Ghasedak
{
    public class Api : ISMSService, IAccountService, IVoiceService, IReceiveService, IContactService
    {
        #region Fields
        private readonly string _apikey;
        private static readonly JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();
        #endregion

        #region Ctor
        public Api(string apikey)
        {
            _apikey = apikey;
        }
        #endregion




        #region sms
        public SendResult SendSMS(string message,  string receptor,string linenumber = null, DateTime? senddate=null, String checkid=null, string dep = null)
        {
            var url = "v2/sms/send/simple";
            var param = new Dictionary<string, object>();
            param.Add("message", message);
            param.Add("receptor", receptor);

            if (!string.IsNullOrEmpty(linenumber))
                param.Add("linenumber", linenumber);
            if (senddate.HasValue)
                param.Add("senddate", Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(senddate)));
            if(!string.IsNullOrEmpty(checkid))
                param.Add("checkid", checkid);

            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();

            return MakeSendRequest(url, param);
        }
        public SendResult SendSMS(string[] message, string[] linenumber, string[] receptor, DateTime[] senddate =null, string[] checkid=null, string dep = null)
        {
            var url = "v2/sms/send/bulk";
            var msg = new System.Text.StringBuilder();
            var date = new System.Text.StringBuilder();
            var check = new System.Text.StringBuilder();
            var line = new System.Text.StringBuilder();

            var param = new Dictionary<string, object>();

            //foreach (var item in message)
            //{
            //    msg.Append(item).Append(",");
            //}

            //foreach (var item in linenumber)
            //{
            //    line.Append(item).Append(",");
            //}

            param.Add("linenumber", string.Join(",", linenumber));
            param.Add("message", string.Join(",", message));
            param.Add("receptor", string.Join(",", receptor));
            if (senddate != null && senddate.Length > 0)
            {
                foreach (var item in senddate)
                {
                    date.Append(Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(item))).Append(",");
                }
                param.Add("senddate", date);
            }

            if (checkid != null &&  checkid.Length > 0)
                param.Add("checkid", string.Join(",", checkid));


            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();

            return MakeSendRequest(url, param);
        }
        public SendResult SendSMS(string message, string[] receptor, string linenumber=null, DateTime? senddate=null, string[] checkid=null, string dep = null)
        {
            var url = "v2/sms/send/pair";
            var param = new Dictionary<string, object>();


            param.Add("message", message);
            param.Add("receptor", string.Join(",", receptor));

            if (!string.IsNullOrEmpty(linenumber))
              param.Add("linenumber", linenumber);

            if (senddate.HasValue)
                param.Add("senddate", Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(senddate)));
            if (checkid != null && checkid.Length > 0)
                param.Add("checkid", string.Join(",", checkid));

            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeSendRequest(url, param);
        }
        public SendResult Verify(int type, string template, string[] receptor, string param1, string param2=null, string param3=null, string param4=null, string param5=null, string param6=null, string param7=null, string param8=null, string param9=null, string param10=null, string dep = null)
        {
            var url = "v2/verification/send/simple";
            var param = new Dictionary<string, object>
        {
            {"type", type},
            {"receptor",string.Join(",",receptor) },
            {"template", template},
            {"param1", param1},
            {"param2", param2},
            {"param3", param3},
            {"param4", param4},
            {"param5", param5},
            {"param6", param6},
            {"param7", param7},
            {"param8", param8},
            {"param9", param9},
            {"param10", param10},
        };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeSendRequest(url, param);
        }
        public StatusResult GetStatus(int type,long[] id, string dep = null)
        {
            var url = "v2/sms/status";
            var param = new Dictionary<string, object>
               {
       
                   {"type", type},
                   {"id", string.Join(",",id)},
               };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return _JavaScriptSerializer.Deserialize<StatusResult>(Client.ApiClient.Execute(url, _apikey, param,"Get"));
        }
        public SendResult CancelSMS(long[] messageid, string dep = null)
        {
            var url = "v2/sms/cancel";
            var param = new Dictionary<string, object>
             {
                {"messageid", string.Join(",",messageid)},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeSendRequest(url, param);
        }
        #endregion

        #region Contact
        public GroupResult AddGroup(string name, int parent, string dep = null)
        {
            var url = "v2/contact/group/new";
            var param = new Dictionary<string, object>
             {
    
                {"name", name},
                {"parent", parent},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return _JavaScriptSerializer.Deserialize<GroupResult>(Client.ApiClient.Execute(url, _apikey, param));
        }

        public ApiResult RemoveGroup(int groupid, string dep = null)
        {
            var url = "v2/contact/group/remove";
            var param = new Dictionary<string, object>
             {
    
                {"groupid", groupid},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeRequest(url, param);
        }

        public ApiResult EditGroup(int groupid, string name, string dep = null)
        {
            var url = "v2/contact/group/edit";
            var param = new Dictionary<string, object>
             {
    
                {"groupid", groupid},
                {"name", name},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeRequest(url, param);
        }

        public ApiResult AddNumberToGroup(int groupid, string[] number, string[] firstname, string[] lastname, string[] email, string dep = null)
        {
            var url = "v2/contact/group/addnumber";
            var param = new Dictionary<string, object>
             {
                {"groupid", groupid},
                {"number", string.Join(",",number)},
                {"firstname", string.Join(",",firstname)},
                {"lastname", string.Join(",",lastname)},
                {"email", string.Join(",",email)},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeRequest(url, param);
        }

        public GroupListResult GroupList(int parent, string dep = null)
        {
            var url = "v2/contact/group/list";
            var param = new Dictionary<string, object>
             {
                {"parent", parent},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return _JavaScriptSerializer.Deserialize<GroupListResult>(Client.ApiClient.Execute(url, _apikey, param));
        }

        public GroupNumbersResult GroupNumbersList(int groupid, int page, int offset, string dep = null)
        {
            var url = "v2/contact/group/listnumber";
            var param = new Dictionary<string, object>
             {
                {"groupid", groupid},
                {"page", page},
                {"offset", offset},
             };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return _JavaScriptSerializer.Deserialize<GroupNumbersResult>(Client.ApiClient.Execute(url, _apikey, param));
        }

        #endregion

        #region Account
        public AccountResult AccountInfo( string dep = null)
        {
            var url = "v2/account/info";
            var param = new Dictionary<string, object>
        {
            {"apikey", _apikey}
        };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            var response = Client.ApiClient.Execute(url, _apikey, param);
            return _JavaScriptSerializer.Deserialize<AccountResult>(response);
        }
        #endregion

        #region Voice
        public SendResult SendVoice(string message, string[] receptor, DateTime? senddate, string dep = null)
        {
            var url = "v2/voice/send/simple";
            var param = new Dictionary<string, object>();

            param.Add("message", message);
            if (senddate.HasValue)
                param.Add("senddate", Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(senddate)));
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            return MakeSendRequest(url, param);
        }
        #endregion

        #region Receive
        public ReceiveMessageResult ReceiveList(string linenumber, int isRead, string dep = null)
        {
            var url = "v2/sms/receive/last";
            var param = new Dictionary<string, object>
                {
                 {"linenumber", linenumber},
                 {"isRead", isRead},
                };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            var response = Client.ApiClient.Execute(url, _apikey, param);
            return _JavaScriptSerializer.Deserialize<ReceiveMessageResult>(response);
        }
        public ReceiveMessageResult ReceiveListPaging(string linenumber, int isRead , DateTime fromdate , DateTime todate , int page=0 , int offset=200, string dep = null)
        {
            var url = "v2/sms/receive/paging";
            var param = new Dictionary<string, object>
                {
                 {"linenumber", linenumber},
                 {"isRead", isRead},
                 {"fromdate", Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(fromdate))},
                 {"todate", Utilities.Date_Time.DatetimeToUnixTimeStamp(Convert.ToDateTime(todate))},
                 {"page", page},
                 {"offset", offset},
                };
            if (!string.IsNullOrEmpty(dep))
                url += "?dep=" + dep.Trim();
            var response = Client.ApiClient.Execute(url, _apikey, param);
            return _JavaScriptSerializer.Deserialize<ReceiveMessageResult>(response);
        }
        #endregion


        #region Utility
        private ApiResult MakeRequest(string url, Dictionary<string, object> param)
        {
            return _JavaScriptSerializer.Deserialize<ApiResult>(Client.ApiClient.Execute(url, _apikey, param));
        }
        private SendResult MakeSendRequest(string url, Dictionary<string, object> param)
        {
            return _JavaScriptSerializer.Deserialize<SendResult>(Client.ApiClient.Execute(url, _apikey, param));
        }
        #endregion

    }
}
