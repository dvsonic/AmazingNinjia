using UnityEngine;
using System.Collections;

public class DestoryController : MonoBehaviour {

	// Use this for initialization
    private Vector3 offset;
	void Start () {
        offset = Camera.main.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.transform.position - offset;
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "hero")
        {
            Camera.main.GetComponent<CameraShake>().Shake();
            Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (canvas)
                canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
            Application.LoadLevel("Result");
        }
        else
        {
            if (coll.gameObject.transform.parent)
                Destroy(coll.gameObject.transform.parent.gameObject);
            else
                Destroy(coll.gameObject);
        }
    }
}
