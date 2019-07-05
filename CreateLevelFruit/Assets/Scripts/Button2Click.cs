using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Button2Click : MonoBehaviour
{
    Button button;
    //public int index;
    // Use this for initialization
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
        missionButton.HidePopupMission();
        Sprite sprite = GetComponent<Image>().sprite;
        missionButton.ChangeSprite(sprite, gameObject.name);
    }
}
