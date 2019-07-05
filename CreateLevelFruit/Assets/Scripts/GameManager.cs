using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    //time
    public Toggle isTime;
    public InputField timeGiven;

    //move
    public Toggle isMove;
    public InputField maxMove;

    //score    
    public Toggle isScore;
    public Toggle scoreRequiredWin;
    public Toggle scoreEndsGame;
    public InputField score1;
    public InputField score2;
    public InputField score3;

    //get type game
    public Toggle isGetTypesGame;
    public Toggle typesRequiredWin;
    public Toggle typeEndsGame;
    public InputField[] lstMissionFruit;
    public GameObject[] lstQua;
    //TreasureGame
    public Toggle isTreasureGame;
    public Toggle treasureRequiredWin;
    public Toggle treasureEndsGame;
    public InputField[] InPutSoluongSau;
    //public InputField InPutSoluongSau2;

    //FruitBom
    public Toggle isFruitBom;
    public InputField maxBom;

    //level
    public InputField txtLevel;

    //NumOfType
    // public InputField txtNumOfType;
    public Toggle[] LoaiQua;

    int countNumOfFruit;
    Gp_ClassData targetGame = new Gp_ClassData();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SaveDataJson()
    {
        PanelController panelController = FindObjectOfType<PanelController>();
        int count = 0;

        //time
        targetGame.timeGiven = float.Parse(timeGiven.text);
        targetGame.targetTime = isTime.isOn;

        //move 
        targetGame.move = int.Parse(maxMove.text);
        targetGame.targetMove = isMove.isOn;

        //score        
        targetGame.score1 = int.Parse(score1.text);
        targetGame.score2 = int.Parse(score2.text);
        targetGame.score3 = int.Parse(score3.text);
        targetGame.taregtScore = isScore.isOn;

        // TargetFruit
        targetGame.targetFruit = isGetTypesGame.isOn;
        targetGame.lstMissionFruitAmout = new List<Gp_ClassData.MissionType>();

        for (int i = 0; i < lstMissionFruit.Length; i++)
        {
            if (!string.IsNullOrEmpty(lstMissionFruit[i].text) && lstMissionFruit[i].text != "0")
            {
                var missionType = new Gp_ClassData.MissionType();
                missionType.name = lstQua[i].name;
                missionType.amount = int.Parse(lstMissionFruit[i].text);
                targetGame.lstMissionFruitAmout.Add(missionType);
            }
        }
        //TargetSau
        targetGame.targetBug = isTreasureGame.isOn;
        targetGame.soluongSau = new List<int>();
        for (int i = 0; i < InPutSoluongSau.Length; i++)
        {
            targetGame.soluongSau.Add(int.Parse(InPutSoluongSau[i].text));
        }

        //FruitBom
        targetGame.targetBom = isFruitBom.isOn;
        targetGame.soluongBom = int.Parse(maxBom.text);

        //LoaiQua
        targetGame.loaiqua = new List<string>();
        foreach (var item in LoaiQua)
        {
            Debug.Log(item.name);
            if (item.isOn)
            {
                targetGame.loaiqua.Add(item.name);
            }
        }
        //Save
        targetGame.num = new List<int>();
        for (int j = 0; j < 9; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                int num = panelController.arrayPanel[i, j].GetComponent<ScriptsPanel>().indexType;
                targetGame.num.Add(num);
            }
        }
        var obj = JsonConvert.SerializeObject(targetGame); 
        Debug.Log(obj.ToString());
        //File.WriteAllText(Environment.CurrentDirectory + "/Assets/Resources/Level/" +"Level"+ txtLevel.text + ".json",obj);
        File.WriteAllText("C:/Users/info/Desktop/UPDATE/CreateLevelFruit/Levelgame/level" + txtLevel.text + ".json", obj);
    }
}
