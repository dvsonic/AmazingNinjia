using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarController : MonoBehaviour {

    private List<GameObject> starList;
    private int curValue;
    private int maxValue;
	// Use this for initialization
	void Start () {
	}

    void Init()
    {
        starList = new List<GameObject>();
        curValue = GameData.hp;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject star = transform.GetChild(i).gameObject;
            if (curValue <= i)
                star.GetComponent<Animator>().SetBool("isSub", true);

            starList.Add(transform.GetChild(i).gameObject);
        }
        maxValue = transform.childCount;
        EventManager.getInstance().addEventListener(Event_Name.REFRESH_STAR, UpdateValue);
    }

    // Update is called once per frame
    void Update()
    {

	}

    public void UpdateValue()
    {
        int value = GameData.hp;
        if (value > curValue && value<= maxValue)//加血
        {
            transform.GetChild(value - 1).gameObject.GetComponent<Animator>().SetBool("isAdd", true);
            transform.GetChild(value - 1).gameObject.GetComponent<Animator>().SetBool("isSub", false);
            curValue = value;
        }
        else if (value < curValue && value >= 0)//减血
        {
            for (int i = value; i < curValue; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<Animator>().SetBool("isSub", true);
                transform.GetChild(i).gameObject.GetComponent<Animator>().SetBool("isAdd", false);
            }
            curValue = value;
        }
    }
}
