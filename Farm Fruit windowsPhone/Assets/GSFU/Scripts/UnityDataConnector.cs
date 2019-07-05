using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class UnityDataConnector : MonoBehaviour
{
    public string webServiceUrl = "";
    public string spreadsheetId = "";
    public string worksheetName = "";
    public string password = "";
    public float maxWaitTime = 10f;
    public GameObject dataDestinationObject;
    public string statisticsWorksheetName = "Statistics";
    public bool debugMode;

    bool updating;
    string currentStatus;
    bool saveToGS;

    Rect guiBoxRect;
    Rect guiButtonRect;
    Rect guiButtonRect2;
    Rect guiButtonRect3;

    void Start()
    {
        updating = false;
        currentStatus = "Offline";
        saveToGS = false;

        guiBoxRect = new Rect(10, 10, 310, 140);
        guiButtonRect = new Rect(30, 40, 270, 30);
        guiButtonRect2 = new Rect(30, 75, 270, 30);
        guiButtonRect3 = new Rect(30, 110, 270, 30);
    }

    void OnGUI()
    {
        GUI.Box(guiBoxRect, currentStatus);
        if (GUI.Button(guiButtonRect, "Update From Google Spreadsheet"))
        {
            Connect();
        }

        saveToGS = GUI.Toggle(guiButtonRect2, saveToGS, "Save Stats To Google Spreadsheet");

        if (GUI.Button(guiButtonRect3, "Reset Balls values"))
        {
            dataDestinationObject.SendMessage("ResetBalls");
        }
    }

    void Connect()
    {
        if (updating)
            return;

        updating = true;
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + worksheetName + "&pass=" + password + "&action=GetData";
        //if (debugMode)
        //Debug.Log("Connecting to webservice on " + connectionString);

        WWW www = new WWW(connectionString);

        float elapsedTime = 0.0f;
        currentStatus = "Stablishing Connection... ";

        while (!www.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= maxWaitTime)
            {
                currentStatus = "Max wait time reached, connection aborted.";
                //Debug.Log(currentStatus);
                updating = false;
                break;
            }

            yield return null;
        }

        if (!www.isDone || !string.IsNullOrEmpty(www.error))
        {
            currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
            //Debug.LogError(currentStatus);
            updating = false;
            yield break;
        }

        string response = www.text;

        // myObject = JsonUtility.FromJson<RootObject>(www.text);
        //Debug.Log(myObject.Name);

        //Debug.Log(elapsedTime + " : " + response);
        currentStatus = "Connection stablished, parsing data...";

        if (response == "\"Incorrect Password.\"")
        {
            currentStatus = "Connection error: Incorrect Password.";
            Debug.LogError(currentStatus);
            updating = false;
            yield break;
        }


        try
        {
            //ssObjects = JsonMapper.ToObject<JsonData[]>(response);
            RootObject ro = new RootObject();
            List<RootObject> lstData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RootObject>>(response);
            Debug.Log(lstData[2].Name);
        }
        catch
        {
            currentStatus = "Data error: could nots parse retrieved data as json.";
            Debug.LogError(currentStatus);
            updating = false;
            yield break;
        }

        currentStatus = "Data Successfully Retrieved!";
        updating = false;

    }

    public class RootObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Package { get; set; }
        public int Status { get; set; }
        public string Idvideoadmob { get; set; }
        public string Idbanneradmob { get; set; }
        public string idfulladmob { get; set; }
    }



}

