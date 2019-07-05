using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    public GameObject PopupListMission;
    public Button[] buttons;
    public int indexButton;
    void Start()
    {
        PopupListMission.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPopupMission()
    {
        PopupListMission.SetActive(true);        
    }

    public void HidePopupMission()
    {
        PopupListMission.SetActive(false);
    }

    public void ChangeSprite(Sprite sprite,string name)
    {
        buttons[indexButton].GetComponent<Image>().sprite = sprite;
        buttons[indexButton].GetComponent<Image>().name = name;
    }
}
