using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class ChooseLevel : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { btnClick(); });
    }
    public static bool bannercontrol;
    void btnClick()
    {
        iTween.PunchScale(gameObject, new Vector3(0.2f, 0.2f), 0.3f);
        ControllerButtonMap controllerButtonMap = FindObjectOfType<ControllerButtonMap>();
        ReadDataMap readData = FindObjectOfType<ReadDataMap>();
        readData.ReadMission(gameObject.name);
        controllerButtonMap.btnChooseLevel();
        int level = int.Parse(gameObject.name.Substring(5));
        ObscuredPrefs.SetInt("level", level);
        if (!string.IsNullOrEmpty(ManagerDelegate.IdFacebook))
        {
            GetDataFacebook getdatafacebook = FindObjectOfType<GetDataFacebook>();
            getdatafacebook.ClearData();
            AzureUI azure = FindObjectOfType<AzureUI>();
            azure.QueryListLevel(gameObject.name);
        }
        bannercontrol = true;
        // admodads.bannerView.Show();

    }
}
