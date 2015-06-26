using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{

    public Text tfScore;
    public GameObject PausePanel;
    public GameObject PauseUI;
    public Text tfCountDown;
    public Text tfGuide;
    public GameObject star;

    public GameObject startCanvas;
    public GameObject mainCanvas;
    public GameObject endCanvas;
    // Use this for initialization
    void Awake()
    {
        GameScene.init(startCanvas, mainCanvas, endCanvas);
        if (tfGuide)
            tfGuide.text = GameData.getLanguage("guide");
    }
    void Start()
    {
        GameData.score = 0;
        GameData.hp = GameData.MAX_HP;
        star.SendMessage("Init");

    }

    public GameObject ninjia;
    public GameObject factory;
    void Reset()
    {
        Debug.Log("Reset");
        Start();
        if (ninjia)
            ninjia.SendMessage("Reset");
        if (factory)
            factory.SendMessage("Reset");
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (tfScore)
            tfScore.text = GameData.score.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (tfCountDown.enabled)
        {
            int passTime = Mathf.FloorToInt(Time.realtimeSinceStartup - realTime);
            int left = 3 - passTime;
            if (left > 0)
                tfCountDown.text = left.ToString();
            else
            {
                Time.timeScale = 1;
                PausePanel.SetActive(false);
                tfCountDown.enabled = false;
            }
        }
    }

    public void PauseOrResume()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PauseUI.SetActive(true);
        tfCountDown.enabled = false;
        realTime = 0;
    }

    private float realTime;
    void Resume()
    {
        realTime = Time.realtimeSinceStartup;
        tfCountDown.enabled = true;
        PauseUI.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        GameData.score = 0;
        GameScene.GotoScene(1);
    }

    /*void OnEnable()
    {
        gameObject.SendMessage("SetBannerVisible", true, SendMessageOptions.DontRequireReceiver);
    }

    void OnDisable()
    {
        gameObject.GetComponent<ADMob>().SetBannerVisible(false);
    }*/
}
