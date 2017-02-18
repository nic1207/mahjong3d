using UnityEngine;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System;

public static class YkiApi
{

    public delegate void RequestCallBack(WebExceptionStatus status, string result);
    //public delegate void AsyncCallback(IAsyncResult ar);
    public static RequestCallBack _callback;

    private static string serverUrl = "https://192.168.22.19:8000/";
    //private static string serverUrl = "https://yki.herokuapp.com/";
    //private static string serverUrl = "https://www.google.com.tw/";
    private static string secretKey = "KQgZFQFLWL0qyRjCbgpEIYUhjYjmZOvbywbdGABb46cGzeevCMQU2LXvornsNkScfeCS9BmZ0KkebfYTvgvfLwUpl0QjR4LL5hHOYzaHxGQcVfvvY2wtiPRRMxGqhxVq";
    //private static string _pData = string.Empty;
    //private static ManualResetEvent allDone = new ManualResetEvent(false);
    //private static WebExceptionStatus _status;
    //private static string _rData = string.Empty;

    public static void Login(string type, string name, string id, RequestCallBack callback)
    {
        string api = "V1/login";
        string auth = "Bearer " + secretKey;
        string method = "POST";
        string pdata = "[{\"Name\":\"" + name + "\", \"Type\":\"" + type + "\", \"Id\":\"" + id + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void AddMember(string name, string pass, RequestCallBack callback)
    {
        string api = "V1/addMember";
        string auth = "Bearer " + secretKey;
        string method = "POST";
        string pdata = "[{\"Name\":\"" + name + "\", \"Pass\":\"" + pass + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getModelUrl(string version, RequestCallBack callback)
    {
        string api = "V1/getModelUrl";
        string auth = "Bearer " + secretKey;
        string method = "POST";
        string pdata = "[{\"Version\":\"" + version + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void setForgetPwd(string name, string pass, string nick_name, string code, RequestCallBack callback)
    {
        string api = "V1/setForgetPwd";
        string auth = "Bearer " + secretKey;
        string method = "POST";
        string pdata = "[{\"Name\":\"" + name + "\", \"Pass\":\"" + pass + "\", \"NickName\":\"" + nick_name + "\", \"Code\":\"" + code + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getAuthCode(string name, RequestCallBack callback)
    {
        string api = "V1/getAuthCode";
        string auth = "Bearer " + secretKey;
        string method = "POST";
        string pdata = "[{\"Name\":\"" + name + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }


    //-----------------------------------------------------------------------------------------------------

    public static void setTaskStatus(string token, string name, string taskStatus, RequestCallBack callback)
    {
        string api = "V1/setTaskStatus";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        string pdata = "[{\"TaskStatus\":\"" + taskStatus + "\"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void setPlayer(string token, string name, string pdata, RequestCallBack callback)
    {
        string api = "V1/setPlayer";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getPlayer(string token, string name, RequestCallBack callback)
    {
        string api = "V1/getPlayer";
        string auth = "Bearer " + name + ":" + token;
        string method = "GET";
        string pdata = string.Empty;
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void setBPItemNum(string token, string name, string itemNum, RequestCallBack callback)
    {
        string api = "V1/setBPItemNum";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        string pdata = "[" + itemNum + "]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void setBPItemNum(string token, string name, int id, int num, RequestCallBack callback)
    {
        string api = "V1/setOneBPItemNum";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        string pdata = "[{\"Id\":"+ id +",\"Num\":" +  num +"}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }


    public static void getBPItemNum(string token, string name, RequestCallBack callback)
    {
        string api = "V1/getBPItemNum";
        string auth = "Bearer " + name + ":" + token;
        string method = "GET";
        string pdata = string.Empty;
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getBPItemNum(string token, string name, int id, RequestCallBack callback)
    {
        string api = "V1/getOneBPItemNum?bpitemid=" + id;
        string auth = "Bearer " + name + ":" + token;
        string method = "GET";
        string pdata = string.Empty;
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);

    }

    public static void setOwnYokai(string token, string name, string yokia, RequestCallBack callback)
    {
        string api = "V1/setYokai";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        string pdata = "[" + yokia + "]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getOwnYokais(string token, string name, RequestCallBack callback)
    {
        string api = "V1/getOwnYokais";
        string auth = "Bearer " + name + ":" + token;
        string method = "GET";
        string pdata = string.Empty;
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void addOwnYokai(string token, string name, string yokia, RequestCallBack callback)
    {
        string api = "V1/addYokai";
        string auth = "Bearer " + name + ":" + token;
        string method = "PUT";
        string pdata = "[" + yokia + "]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getNewYokai(string token, string name, int baseId, int userLevel, RequestCallBack callback)
    {
        string api = "V1/getNewYokai";
        string auth = "Bearer " + name + ":" + token;
        string method = "POST";
        string pdata = "[{\"id\":" + baseId + ",\"userlevel\":" + userLevel + "}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }

    public static void getMapYokais(string token, string name, double lat, double lon, float radius, RequestCallBack callback)
    {
        string api = "V1/getMapYokais";
        string auth = "Bearer " + name + ":" + token;
        string method = "POST";
        string pdata = "[{\"latitude\":" + lat + ",\"longitude\":" + lon + ",\"radius\":" + radius + "}]";
        LoginClient.Instance.SendRequest(serverUrl + api, auth, method, pdata, callback);
    }


}