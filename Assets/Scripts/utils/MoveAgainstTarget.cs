using UnityEngine;
using System.Collections;

public class MoveAgainstTarget : MonoBehaviour {

	// Use this for initialization
    public int speed;
    private Vector3 offset;
	void Start () {
        offset = transform.position - Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
