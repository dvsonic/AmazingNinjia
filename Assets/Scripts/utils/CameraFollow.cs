using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    private GameObject bg;
    private Vector3 _offset;
    // Use this for initialization
    void Start()
    {
        bg = GameObject.Find("BGStill");
        _offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x + _offset.x, transform.position.y, transform.position.z);
            if(bg)
                bg.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
