using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    public enum EnemyState {IDLE,HIT,KNOCK,DEAD }
    public EnemyState state;
    public float attackDistant;
    public ThiefController ninjia;
    private bool canHit;
    private bool canKnock;
    public AudioSource attack;
    public AudioSource die;
	// Use this for initialization
	void Start () {
        canHit = canKnock = true;
        if (null == ninjia)
            ninjia = GameObject.Find("Ninjia").GetComponent<ThiefController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canHit && transform.position.x - ninjia.transform.position.x>0 && transform.position.x - ninjia.transform.position.x < attackDistant && ninjia.CanBeHit())
        {
            SetState(EnemyState.HIT);
            canHit = false;
        }
        if (canKnock && ninjia.transform.position.x - transform.position.x > attackDistant / 2 && ninjia.transform.position.x - transform.position.x < attackDistant && ninjia.CanBeKnock())
        {
            SetState(EnemyState.KNOCK);
            canKnock = false;
        }
	}

    void FixedUpdate()
    {

    }

    public void SetState(EnemyState state)
    {
        this.state = state;
        switch (state)
        {
            case EnemyState.IDLE:
                GetComponent<Animator>().SetBool("isHit", false);
                GetComponent<Animator>().SetBool("isKnock", false);
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case EnemyState.HIT:
                GetComponent<Animator>().SetBool("isHit", true);
                if (attack)
                    attack.Play();
                break;
            case EnemyState.KNOCK:
                GetComponent<Animator>().SetBool("isKnock", true);
                transform.localScale = new Vector3(-1, 1, 1);
                if (attack)
                    attack.Play();
                break;
            case EnemyState.DEAD:
                GetComponent<Animator>().SetBool("isDead", true);
                GetComponent<TargetState>().IsDead = true;
                if (die)
                    die.Play();
                break;
        }
    }

    public void OnAttack()
    {
        ninjia.OnHit();
    }

    public void OnHit()
    {
        SetState(EnemyState.DEAD);
        GameData.score += 10;
        ninjia.SendMessage("SetHeadState", 1);
    }

    public void OnDead()
    {
        Destroy(gameObject);
    }


}
