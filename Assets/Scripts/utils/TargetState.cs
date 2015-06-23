using UnityEngine;
using System.Collections;

public class TargetState : MonoBehaviour {

    private bool isDead;
	// Use this for initialization
	void Start () {
        isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsDead
    {
        get { return isDead;}
        set { isDead = value; }
    }
}
