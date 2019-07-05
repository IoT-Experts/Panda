using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LayerMoreGame : MonoBehaviour
{

    public string webServiceUrl = "";
    public string spreadsheetId = "";
    public string worksheetName = "";
    bool updating;
    public float maxWaitTime = 10f;
    public string password = "";

    public List<RootObject> _moregame = new List<RootObject>();
    //public GameObject Content;
    public GameObject ItemMoregame;
    int count;
    List<GameObject> listItem = new List<GameObject>();
    void Start()
   {
        Connect();
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

        WWW www = new WWW(connectionString);

        float elapsedTime = 0.0f;

        while (!www.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= maxWaitTime)
            {
                updating = false;
                break;
            }

            yield return null;
        }

        if (!www.isDone || !string.IsNullOrEmpty(www.error))
        {
            updating = false;
            yield break;
        }

        string response = www.text;

        if (response == "\"Incorrect Password.\"")
        {
            updating = false;
            yield break;
        }


        try
        {
            RootObject ro = new RootObject();
            _moregame = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RootObject>>(response);
        }
        catch
        {
            updating = false;
            yield break;
        }

        updating = false;

    }  

    public void BindingData()
    {
       for (int i = 0; i < _moregame.Count; i++)
        {
            if (_moregame[i].Status == "1")
            {
                Debug.Log(_moregame[i].Images);
                ItemMoregame.name = _moregame[i].Package;
                ItemMoregame.GetComponent<Button>().onClick.AddListener(() => OpenUrl(ItemMoregame.name));
                StartCoroutine(GetIconGame(_moregame[i].Images));
                break;
            }
        }
       
    }

    public void RemoveData()
    {
        foreach (var item in listItem)
        {
            Destroy(item);
        }
        count = 0;
    }

    IEnumerator GetIconGame( string link)
    {
        WWW getTexture = new WWW(link);
        yield return getTexture;
        ItemMoregame.GetComponent<Image>().sprite = Sprite.Create(getTexture.texture, new Rect(0, 0, getTexture.texture.width, getTexture.texture.height), new Vector2(0.5f, 0.5f));        
    }

    void OpenUrl(string package)
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + package);
    }
    public class RootObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Package { get; set; }
        public string Status { get; set; }
        public string Idvideoadmob { get; set; }
        public string Idbanneradmob { get; set; }
        public string idfulladmob { get; set; }
    }

}
