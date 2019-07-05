using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [HideInInspector]
    public int Index;
    //public int _index;
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { Click(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Click()
    {
        MissionButton missionButton = FindObjectOfType<MissionButton>();
        missionButton.ShowPopupMission();
        missionButton.indexButton = Index;
    }
}
 