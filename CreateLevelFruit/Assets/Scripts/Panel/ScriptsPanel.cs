using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptsPanel : MonoBehaviour
{    
    public int indexType;
    Button button;
    public Text[] lstText;
    public int[] arrayRef;
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
        indexType = panelController.indexType;
        if (panelController.sprite != null)
        {
            GetComponent<Image>().sprite = panelController.sprite;
        }
    }
}
