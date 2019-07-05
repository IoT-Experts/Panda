using UnityEngine;
public class FacebookControler : MonoBehaviour
{
    public GameObject LayerFriendFacebook;
    public GameObject LayerSpinFacebook;
       
    void Start()
    {
        if (Application.loadedLevelName=="GameMap")
        {
            LayerFriendFacebook.SetActive(false);
            if (!string.IsNullOrEmpty(ManagerDelegate.IdFacebook))
            {
                gameObject.SetActive(false);
                LayerFriendFacebook.SetActive(true);
            }
        }
        
    }
    public void LoginFB()
    {
        iTween.PunchScale(gameObject, new Vector3(0.5f, 0.5f), 0.5f);        
        ManagerDelegate.LoginFacebook();
    }  
     
    public class RootObject
    {
        public bool success { get; set; }
    }

}
