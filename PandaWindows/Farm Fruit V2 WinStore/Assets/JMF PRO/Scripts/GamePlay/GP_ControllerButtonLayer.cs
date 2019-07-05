using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GP_ControllerButtonLayer : MonoBehaviour
{
    public GameObject ButtonShare;
    Time time;
    Animator anim;
    public static bool sharebuton;

    public enum TypeButton
    {
        btnNext,
        btnRetry,
        btnCloseLayer,
        btnShare
    }
    public TypeButton typeButton;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void OnMouseDown()
    {
        switch (typeButton)
        {
            case TypeButton.btnNext:               
                anim.Play("btnNextClick");
                StartCoroutine(NextLevel());
                break;
            case TypeButton.btnRetry:
                StartCoroutine(RetryLevel());
                anim.Play("btnNextClick");               
                break;
            case TypeButton.btnCloseLayer:
                break;
            case TypeButton.btnShare:
                ButtonShare.SetActive(false);
                Data.UpdateData(Data.keyGio, 2);
                Debug.Log("Share");
                ManagerDelegate.ShareFB();
                sharebuton = true;

                break;
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }

    IEnumerator RetryLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
                
    }
}
