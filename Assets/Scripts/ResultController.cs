using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultController : MonoBehaviour {

	// Use this for initialization
    public Text tfScore;
    public Text tfScoreBest;
    public Text tfReplay;
    public Text tfQuit;
    public Text tfShare;
    public Text tfEnd;
    public Text tfRecord;
    public GameObject Record;
    public Button btnHome;
    public Button btnRank;
    public Button btnDiscuss;
    public Button btnRestart;
    public Button btnHomeIOS;
    public const string BEST_SCORE = "BEST_SCORE";

    void Awake()
    {
        if(tfReplay)
            tfReplay.text = GameData.getLanguage().SearchForChildByTag("replay").Text;
        if(tfQuit)
            tfQuit.text = GameData.getLanguage().SearchForChildByTag("quit").Text;
        if (tfEnd)
            tfEnd.text = GameData.getLanguage().SearchForChildByTag("end").Text;
        if (tfShare)
            tfShare.text = GameData.getLanguage().SearchForChildByTag("share").Text;
        if(tfRecord)
            tfRecord.text = GameData.getLanguage().SearchForChildByTag("record").Text;
#if UNITY_ANDROID
        Destroy(btnRank.gameObject);
        Destroy(btnDiscuss.gameObject);
        Destroy(btnHomeIOS.gameObject);
#else
       Destroy(btnHome.gameObject);
#endif
    }
	void Start () {
        tfScore.text = GameData.getLanguage().SearchForChildByTag("score").Text+"\n"+GameData.score.ToString();
        int best = PlayerPrefs.GetInt(BEST_SCORE);
        if (GameData.score >= best)
        {
            PlayerPrefs.SetInt(BEST_SCORE, GameData.score);
            best = GameData.score;
            Record.SetActive(true);
        }
        else
        {
            Record.SetActive(false);
        }
        tfScoreBest.text = GameData.getLanguage().SearchForChildByTag("bestscore").Text+"\n"+best.ToString();

        SocialManager.GetInstance().ReportScore("20003", GameData.score);

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    public void Restart()
    {
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        GameData.score = 0;
        Application.LoadLevel("Main");
    }

    public void Quit()
    {
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        Application.Quit();
    }

    public void Login()
    {
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        Application.LoadLevel("Start");
    }

    public void Rank()
    {
        SocialManager.GetInstance().ShowLeaderboard();
    }

    public void ToDiscuss()
    {
#if UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/app/id1011674889?mt=8");
#endif
    }
}
