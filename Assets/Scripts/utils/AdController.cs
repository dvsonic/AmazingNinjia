using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdController : MonoBehaviour
{

    // Use this for initialization
    public string adConfig;
    public ADPlatfrom platform;
    public static bool admob_show;
    private static bool isShow;
    private ADConfig selfAD;
    private bool isLoad;
    void Awake()
    {
        if (!isShow)
        {
            isLoad = false;
            startLoad = Time.time;
            StartCoroutine(LoadConfig());
            isShow = true;
        }
        else
        {
            isLoad = true;
            Quit();
        }
    }

    private List<ADConfig> adList;
    IEnumerator LoadConfig()
    {
        adList = new List<ADConfig>();
        Debug.Log("Start:" + Time.time);
        WWW w = new WWW(adConfig);
        yield return w;
        Debug.Log("Finish:" + Time.time);
        string config = w.text;
        if (string.IsNullOrEmpty(w.text))
        {
            Debug.Log("load config error");
            string adconfig = Resources.Load("config/adconfig").ToString();
            PhaseConfig(adconfig);
        }
        else
            PhaseConfig(w.text);
    }

    private void PhaseConfig(string config)
    {
        isLoad = true;
        string[] ary = config.Split('\n');
        List<ADConfig> selfList = new List<ADConfig>();
        foreach (string str in ary)
        {
            string[] ary2 = str.Split('\t');
            if (ary2[0] == platform.ToString() && ary2.Length == 5)
            {
                ADConfig ad = new ADConfig();
                ad.platfrom = platform;
                ad.image = ary2[1];
                ad.url = ary2[2];
                ad.start_show = int.Parse(ary2[3]);
                ad.admob_show = int.Parse(ary2[4]);
                if (ad.url.IndexOf("http") >= 0 && ad.start_show == 1)//内置广告
                    selfList.Add(ad);
                else
                    admob_show = ad.admob_show == 1;
            }
        }
        if (selfList.Count > 0)
        {
            selfAD = selfList[Random.Range(0, selfList.Count)];
            if (selfAD.image.IndexOf("http") >= 0)//网络资源
            {
                StartCoroutine(LoadImage(selfAD.image));
            }
            else
            {
                Sprite img = Resources.Load("image/ad/" + selfAD.image.Split('.')[0], typeof(Sprite)) as Sprite;
                if (img)
                    this.GetComponent<Image>().sprite = img;
                else
                    Debug.Log("image/ad/" + selfAD.image.Split('.')[0] + "load Error");
            }
        }
    }

    IEnumerator LoadImage(string url)
    {
        WWW w = new WWW(url);
        yield return w;
        if (w.isDone)
        {
            Texture2D t = w.texture;
            Sprite s = null;
            if (t != null)
                s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
            if (s != null)
                this.GetComponent<Image>().sprite = s;
        }
    }

    public void Quit()
    {
        if (transform.parent)
            transform.parent.gameObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    private float startLoad;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoad && Time.time - startLoad > 3)//三秒没加载完就用本地配置
        {
            PhaseConfig(Resources.Load("config/adconfig").ToString());
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
#else
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
#endif
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (selfAD.url != null)
                {
                    Application.OpenURL(selfAD.url);
                    Quit();
                }
            }
        }
    }
}
public enum ADPlatfrom { ios, ios_admob, ad, ad_admob };
public struct ADConfig
{
    public ADPlatfrom platfrom;
    public string image;
    public string url;
    public int start_show;//内置广告开关
    public int admob_show;//admob开关
}
