using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GroupToogle : MonoBehaviour {
    public Toggle[] toggles;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    public void toggle0Click()
    {
        toggles[1].isOn = false;
    }

    public void toggle1Click()
    {
        toggles[0].isOn = false;
    }



}
