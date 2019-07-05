using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelTemplateScripts : MonoBehaviour
{

    Button button;
    public int typeIndex;
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
        PanelController panelController = FindObjectOfType<PanelController>();
        panelController.sprite = GetComponent<Image>().sprite;
        panelController.indexType = typeIndex;
    }
}
