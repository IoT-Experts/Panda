using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class ControllerButton : MonoBehaviour
{
    public Animator animButtonBgMenu;
    public Sprite[] SpriteSounds;
    public Sprite[] SpriteMusics;
    public Button ButtonMenu;
    public Button ButtonRate;
    public Button ButtonSound;
    public Button ButtonMusic;
    public Button ButtonPlay;
    public Button ButtonFacebook;

    public GameObject FadeScene;
    public GameObject Music_Controll;

    private bool isMenu;
    bool isSoundOn = true;
    bool isMusicOn = true;
    GameObject music;

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        ButtonMenu.onClick.AddListener(() => btnMenu());
        if (GameObject.FindGameObjectWithTag("music") == null)
        {
            music = Instantiate(Music_Controll);
        }
        else
        {
            music = GameObject.FindGameObjectWithTag("music");
        }
        ButtonSound.onClick.AddListener(() => ButtonSoundClick());
        ButtonMusic.onClick.AddListener(() => ButtonMusicClick());
        ButtonRate.onClick.AddListener(() => ButtonRateClick());
        ButtonPlay.onClick.AddListener(() => ButtonPlayClick());

        int[] abc = { 1, 2, 3, 4, 5, 6 };
        var rand = new System.Random();      
    }

    // Update is called once per frame   
   
    public void ButtonPlayClick()
    {
        MusicControll.musicControll.MakeSound(MusicControll.musicControll.ButtonClick);
        FadeScene.GetComponent<Animator>().Play("FadeSceneClose");
        StartCoroutine(PlayGame(0.5f));
    }

    public void btnMenu()
    {
        MusicControll.musicControll.MakeSound(MusicControll.musicControll.ButtonClick);
        iTween.PunchScale(ButtonMenu.gameObject, new Vector3(0.5f, 0.5f), 0.5f);
        if (!isMenu)
        {
            animButtonBgMenu.SetTrigger("buttonbgmenushow");
            isMenu = true;
        }
        else
        {
            animButtonBgMenu.SetTrigger("buttonbgmenuidle");
            isMenu = false;
        }
    }

    IEnumerator PlayGame(float time)
    {
        yield return new WaitForSeconds(time);
        Application.LoadLevel("GameMap");
    }

    void ButtonSoundClick()
    {
        MusicControll.musicControll.MakeSound(MusicControll.musicControll.ButtonClick);
        iTween.PunchScale(ButtonSound.gameObject, new Vector3(0.5f, 0.5f), 0.5f);
        if (isSoundOn)
        {
            isSoundOn = false;
            ButtonSound.GetComponent<Image>().sprite = SpriteSounds[1];
            MusicControll.musicControll.isSoundOn = false;
        }
        else
        {
            isSoundOn = true;
            ButtonSound.GetComponent<Image>().sprite = SpriteSounds[0];
            MusicControll.musicControll.isSoundOn = true;
        }
    }

    void ButtonMusicClick()
    {
        MusicControll.musicControll.MakeSound(MusicControll.musicControll.ButtonClick);
        iTween.PunchScale(ButtonMusic.gameObject, new Vector3(0.5f, 0.5f), 0.5f);
        if (isMusicOn)
        {
            isMusicOn = false;
            ButtonMusic.GetComponent<Image>().sprite = SpriteMusics[1];
            MusicControll.musicControll.isMusicOn = false;
            music.GetComponent<AudioSource>().Pause();
        }
        else
        {
            isMusicOn = true;
            ButtonMusic.GetComponent<Image>().sprite = SpriteMusics[0];
            MusicControll.musicControll.isMusicOn = true;
            music.GetComponent<AudioSource>().Play();
        }
    }

    void ButtonRateClick()
    {
        MusicControll.musicControll.MakeSound(MusicControll.musicControll.ButtonClick);
        iTween.PunchScale(ButtonRate.gameObject, new Vector3(0.5f, 0.5f), 0.5f);
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.fruit.fram.puzzle.panda");
    }
}
