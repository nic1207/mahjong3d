using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System;
using UnityEngine.Experimental.Networking;

public class LoginClient : MonoBehaviour {

    public class RequestState
    {
        // This class stores the State of the request.
        public HttpWebRequest request;
        public YkiApi.RequestCallBack _callback;
        public string _pData;
    }

    //private LoginClient _instance;
    static public LoginClient Instance;

	private WebExceptionStatus _status;
	private string _rData;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (Instance != this) {
			Destroy (this.gameObject);
		}
	}
	//public delegate void RequestCallBack(WebExceptionStatus status, string result);
	public void SendRequest(string url, string auth, string method, string pdata, YkiApi.RequestCallBack callback)
	{
		StartCoroutine (SendRequestCor(url, auth, method, pdata, callback));
	}
	public IEnumerator SendRequestCor(string url, string auth, string method, string pdata, YkiApi.RequestCallBack callback)
	{
        RequestState requestState = new RequestState();

        ServicePointManager.DefaultConnectionLimit = 30;
		ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;

		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.ContentType = "application/json";
		httpWebRequest.Headers.Set("Authorization", auth);
		httpWebRequest.Method = method;
		httpWebRequest.KeepAlive = true;
		httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

        requestState.request = httpWebRequest;
        requestState._callback = callback;
        requestState._pData = pdata;

        if (method == "GET") {
			httpWebRequest.BeginGetResponse (new AsyncCallback (GetResponseCallback), requestState);
		} else {
			httpWebRequest.BeginGetRequestStream (new AsyncCallback (GetRequestStreamCallback), requestState);
		}
		yield return null;
	}

	private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
	{
        RequestState requestState = (RequestState)asynchronousResult.AsyncState;
        HttpWebRequest httpWebRequest = requestState.request;
		// End the operation
		Stream postStream = httpWebRequest.EndGetRequestStream(asynchronousResult);
		// Convert the string into a byte array.
		byte[] byteArray = Encoding.UTF8.GetBytes(requestState._pData);
		postStream.Write(byteArray, 0, requestState._pData.Length);
		postStream.Close();
		// Start the asynchronous operation to get the response
		httpWebRequest.BeginGetResponse(new AsyncCallback(GetResponseCallback), requestState);
	}

	private void GetResponseCallback(IAsyncResult asynchronousResult)
	{
        RequestState requestState = (RequestState)asynchronousResult.AsyncState;
		try {
            HttpWebRequest httpWebRequest = requestState.request;
            // End the operation
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult);
			Stream streamResponse = response.GetResponseStream();
			StreamReader streamRead = new StreamReader(streamResponse);
			_status = WebExceptionStatus.Success;
			_rData = streamRead.ReadToEnd();
			if (requestState._callback != null) {
                requestState._callback(_status, _rData);
			}
			streamResponse.Close();
			streamRead.Close();
			response.Close();
		}
		catch (WebException ex) {
			//Debug.Log (ex.Message);
			_status = ex.Status;
			_rData = ex.Message;
			if (requestState._callback != null) {
                requestState._callback(ex.Status, ex.Message);
			}
		}
		finally {
			//allDone.Set ();
		}
	}
}