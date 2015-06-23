using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class StartController : MonoBehaviour {

	// Use this for initialization
    public Text tfStart;
    public Transform Block;
    public Slider slider;
    void Awake()
    {
        if (tfStart)
        {
            tfStart.text = GameData.getLanguage().SearchForChildByTag("start").Text;
        }
    }
    void Start () {
        SocialManager.GetInstance().Start();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (slider != null && async != null)
        {
            slider.value = async.progress;
            if (async.progress == 1.0f)
                slider.enabled = false;
        }
	}

    void FixedUpdate()
    {
        if (Block)
            Block.transform.position -= new Vector3(Time.deltaTime * 1, 0, 0);
    }

    private AsyncOperation async;
    public void StartGame()
    {
       async = Application.LoadLevelAsync("Main");
       if (slider)
       {
           slider.gameObject.SetActive(true);
           slider.value = 0;
       }
    }
}
