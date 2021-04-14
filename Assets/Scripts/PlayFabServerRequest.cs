using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using System;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;



public class PlayFabServerRequest : MonoBehaviour
{
    [SerializeField] string buildId;
    [SerializeField] List<string> regions;

    // Start is called before the first frame update
    void Start()
    {
        LoginRemoteUser();
    }

	private void LoginRemoteUser()
	{
		Debug.Log("[ClientStartUp].LoginRemoteUser");

		//We need to login a user to get at PlayFab API's. 
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
		{
			TitleId = PlayFabSettings.TitleId,
			CreateAccount = true,
			CustomId = GUIDUtility.getUniqueID()
		};

		PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnLoginError);
	}

    private void OnLoginError(PlayFabError response)
    {
        Debug.Log(response.ToString());
    }

    private void OnPlayFabLoginSuccess(LoginResult response)
    {
        
            RequestMultiplayerServer();
     
    }
    private void RequestMultiplayerServer()
    {
        Debug.Log("[ClientStartUp].RequestMultiplayerServer");
        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
        requestData.BuildId = buildId;
        requestData.SessionId = Guid.NewGuid().ToString();
        requestData.PreferredRegions = regions;
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
    }

    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        ConnectRemoteClient(response);
    }

    private void ConnectRemoteClient(RequestMultiplayerServerResponse response = null)
    {
        if (response == null)
        {
            Debug.Log("NO RESPONSE");
        }
        else
        {
            Debug.Log("**** ADD THIS TO YOUR CONFIGURATION **** -- IP: " + response.IPV4Address + " Port: " + (ushort)response.Ports[0].Num);
       
        }

    }

    private void OnRequestMultiplayerServerError(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public static class GUIDUtility
{

	public static string getUniqueID(bool generateNewIDState = false)
	{
		string uniqueID;

		if (PlayerPrefsUtility.hasKey("guid") && !generateNewIDState)
		{
			uniqueID = PlayerPrefsUtility.getString("guid");
		}
		else
		{
			uniqueID = generateGUID();
			PlayerPrefsUtility.setString("guid", uniqueID);
		}

		return uniqueID;
	}

	public static string generateGUID()
	{
		var random = new System.Random();
		DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

		string uniqueID = String.Format("{0:X}", Convert.ToInt32(timestamp))                //Time
						+ "-" + String.Format("{0:X}", random.Next(1000000000))                   //Random Number
						+ "-" + String.Format("{0:X}", random.Next(1000000000))                 //Random Number
						+ "-" + String.Format("{0:X}", random.Next(1000000000))                  //Random Number
						+ "-" + String.Format("{0:X}", random.Next(1000000000));                  //Random Number

		Debug.Log(uniqueID);

		return uniqueID;
	}

}


public static class PlayerPrefsUtility
{

    public static void setString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static void setInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static void setFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public static void setBool(string key, bool value)
    {
        int intValue = 0; //Default is FALSE
        if (value) intValue = 1; //If value is TRUE, then it's a 1.
        PlayerPrefs.SetInt(key, intValue);
        PlayerPrefs.Save();
    }


    //The methods below are silly, but the above ones have a bit more validity, so I wanted to have everything wrapped at that point.
    public static string getString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static float getFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public static int getInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static bool getBool(string key)
    {
        int intValue = PlayerPrefs.GetInt(key);
        if (intValue >= 1) return true; //If it's 1, then TRUE. The greater than is just a sanity check.
        return false; //Default is FALSE which should be 0.
    }

    public static bool hasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }


}