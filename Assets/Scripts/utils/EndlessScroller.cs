using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessScroller : MonoBehaviour {

    private List<Transform> itemList;
    private Transform edge;
    private Transform rightItem;
    private float size;
    private bool isStop;
    private Vector3 _initPositon;
	// Use this for initialization
    void Awake()
    {
        _initPositon = transform.position;
    }
	void Start () {
        itemList = new List<Transform>();
        size = transform.GetChild(1).position.x - transform.GetChild(0).position.x;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
                edge = transform.GetChild(i);
            else
            {
                rightItem = transform.GetChild(i);
                itemList.Add(rightItem);
            }
        }

	}

    // Update is called once per frame
    void Update()
    {
        if (!isStop)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            float height = Camera.main.orthographicSize * 2;
            float width = height * Camera.main.aspect;
            float cameraLeft = cameraPos.x - width / 2;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].position.x + size < cameraLeft)
                {
                    int lastIndex = i - 1;
                    if (lastIndex < 0)
                        lastIndex = itemList.Count - 1;
                    itemList[i].position = itemList[lastIndex].position + new Vector3(size,0,0);
                    rightItem = itemList[i];
                }
            }
        }
	}

    public void StopScroll()
    {
        isStop = true;
        if (edge)
        {
            edge.gameObject.SetActive(true);
            edge.gameObject.transform.position = rightItem.position + new Vector3(size,0,0);
        }
    }

    public void Reset()
    {
        isStop = false;
        if (edge)
            edge.gameObject.SetActive(false);
        transform.position = _initPositon;
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].localPosition = new Vector3(size * i, 0, 0);
            rightItem = itemList[i];
        }
    }
}
