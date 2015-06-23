using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {

	// Use this for initialization
    private Transform left;
    private Transform middle;
    private Transform right;
    private float leftExtents;
    private float rightExtents;
	void Start () {
        left = transform.FindChild("ImageLeft");
        middle = transform.FindChild("ImageMiddle");
        right = transform.FindChild("ImageRight");
	}
	
	// Update is called once per frame
	void Update () {
        if (!_isStop)
        {
            leftExtents = left.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            rightExtents = right.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            Vector3 cameraPos = Camera.main.transform.position;
            float height = Camera.main.orthographicSize * 2;
            float width = height * Camera.main.aspect;
            float cameraLeft = cameraPos.x - width / 2;
            if (left.position.x + leftExtents < cameraLeft)//左边图超出相机边界
            {
                left.gameObject.transform.position = new Vector3(right.position.x + rightExtents + leftExtents, left.transform.position.y, left.transform.position.z);
                Transform temp = left;
                left = middle;
                middle = right;
                right = temp;
            }
        }
	}

    private bool _isStop;
    public void StopScroll()
    {
        _isStop = true;
        Transform edge = transform.FindChild("ImageEdge");
        if (edge)
        {
            edge.gameObject.SetActive(true);
            float edgeExtends = edge.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            edge.gameObject.transform.position = new Vector3(middle.position.x + rightExtents + edgeExtends, middle.transform.position.y, middle.transform.position.z);
            right.gameObject.SetActive(false);
        }
    }
}
