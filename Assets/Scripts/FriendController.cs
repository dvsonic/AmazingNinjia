using UnityEngine;
using System.Collections;

public class FriendController : MonoBehaviour {

    public float speed;
    public float checkDistant;//范围内有人才跑
    public GameObject ninjia;
    private bool escaped;
	// Use this for initialization
	void Start () {
        ninjia = GameObject.Find("Ninjia");
        escaped = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!escaped)
        {
            if (ninjia.transform.position.x > transform.position.x)
            {
                GameData.score += 20;
                
                if (GameData.hp < GameData.MAX_HP)
                {
                    GameData.hp++;
                    EventManager.getInstance().trigger(Event_Name.REFRESH_STAR);
                    ninjia.SendMessage("SetHeadState", 3);
                }
                else
                    ninjia.SendMessage("SetHeadState", 2);
                escaped = true;
            }
        }
	}

    void FixedUpdate()
    {
        if(transform.position.x - ninjia.transform.position.x < checkDistant)
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    public void OnHit()
    {
        GetComponent<Animator>().SetBool("isDead", true);
        speed = 0;
        GetComponent<TargetState>().IsDead = true;
    }

    public void OnCrash()
    {
        GetComponent<Animator>().SetBool("isCrash", true);
        speed = 0;
    }
}
