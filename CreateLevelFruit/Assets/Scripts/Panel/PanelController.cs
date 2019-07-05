using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelController : MonoBehaviour
{
    public GameObject tile;
    public GameObject[,] arrayPanel;
    public GridLayoutGroup grid;
    public Sprite[] sprites;
    bool check;
    [HideInInspector]
    public Sprite sprite;
    public int indexType;
    // Use this for initialization
    void Start()
    {
        arrayPanel = new GameObject[9, 9];
        CreatePanel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void CreatePanel()
    {
        for (int colum = 0; colum < 9; colum++)
        {
            for (int row = 0; row < 9; row++)
            {
                arrayPanel[row, colum] = Instantiate(tile);
                arrayPanel[row, colum].transform.parent = grid.transform;
                arrayPanel[row, colum].GetComponent<ScriptsPanel>().lstText[0].text = "x:" + row.ToString();
                arrayPanel[row, colum].GetComponent<ScriptsPanel>().lstText[1].text = "y:" + colum.ToString();
                arrayPanel[row, colum].GetComponent<ScriptsPanel>().arrayRef[0] = row;
                arrayPanel[row, colum].GetComponent<ScriptsPanel>().arrayRef[1] = colum;
                arrayPanel[row, colum].transform.localScale = new Vector3(1, 1, 1);
                arrayPanel[row, colum].transform.localPosition = new Vector3(0, 0, 0);
                if (!check)
                {
                    arrayPanel[row, colum].GetComponent<Image>().sprite = sprites[0];
                    arrayPanel[row, colum].GetComponent<ScriptsPanel>().indexType = 0;
                    check = true;
                }
                else
                {
                    arrayPanel[row, colum].GetComponent<Image>().sprite = sprites[1];
                    arrayPanel[row, colum].GetComponent<ScriptsPanel>().indexType = 8;
                    check = false;
                }
            }
        }
    }

}
