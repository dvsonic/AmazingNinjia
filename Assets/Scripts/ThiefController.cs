using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ThiefController : MonoBehaviour {

	// Use this for initialization
    public enum NinjiaState { FALL_INIT,ON_GROUND,FIRST_JUMP,SECOND_JUMP,DEAD,ATTACK,SECOND_ATTACK,CRASH,LOSE};
    public NinjiaState state;
    public float speed;
    public float jumpSpeed;
    public float jumpSpeed2;
    public BlockFactory[] factoryList;
    public GameObject endlessBlock;
    public GameObject guide;
    public AudioSource run;
    public AudioSource jump;
    public AudioSource dead;
    public AudioSource bonus;
    public AudioSource attack;
    public AudioSource hit;
    public AudioSource crash;
    public float attackDistant;

    private float lastAttackTime;

	void Start () {
        state = NinjiaState.FALL_INIT;
        _isStart = false;
        _hitList = new List<GameObject>();
        if (run)
            run.Play();
    }

    public void Reset()
    {
        Start();
        rigidbody2D.isKinematic = false;
        transform.position = new Vector3(-1.67f, -0.68f);
        GetComponent<TargetState>().IsDead = false;
        GetComponent<Animator>().SetBool("isDead", false);
        GetComponent<Animator>().SetBool("isJump", false);
        GetComponent<Animator>().SetBool("isCrash", false);
        GetComponent<Animator>().SetBool("isLose", false);
        GetComponent<Animator>().SetInteger("attackType", 0);  
        GetComponent<Animator>().SetTrigger("Reset");
        endlessBlock.SendMessage("Reset");
        ResetBG();
        if (guide)
        {
            GameObject.Destroy(guide);
            guide = null;
        }
        if (hurtFlash)
            hurtFlash.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                switch (state)
                {
                    case NinjiaState.ON_GROUND:
                    case NinjiaState.FIRST_JUMP:
                    case NinjiaState.FALL_INIT:
                        Jump();
                        if (!_isStart)
                            GameStart();
                        break;
                }
            }
            else
            {
                if (state == NinjiaState.ON_GROUND || state == NinjiaState.FALL_INIT)
                {
                    if (lastAttackTime > 0 && Time.time - lastAttackTime < 2)
                        SetState(NinjiaState.SECOND_ATTACK);
                    else
                        SetState(NinjiaState.ATTACK);
                    if (!_isStart)
                        GameStart();
                }
                else if((state == NinjiaState.FIRST_JUMP || state == NinjiaState.SECOND_JUMP) && rigidbody2D.velocity.y < 0)//下落的时候可以攻击
                {
                    SetState(NinjiaState.ATTACK);
                }
            }
        }

#else
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
        {
            if (Input.touches[0].position.x < Screen.width / 2)
            {
                switch (state)
                {
                    case NinjiaState.ON_GROUND:
                    case NinjiaState.FIRST_JUMP:
                    case NinjiaState.FALL_INIT:
                        Jump();
                        if (!_isStart)
                            GameStart();
                        break;
                }
            }
            else
            {
                if (state == NinjiaState.ON_GROUND || state == NinjiaState.FALL_INIT)
                {
                    if (lastAttackTime > 0 && Time.time - lastAttackTime < 2)
                        SetState(NinjiaState.SECOND_ATTACK);
                    else
                        SetState(NinjiaState.ATTACK);
                    if (!_isStart)
                        GameStart();
                }
                else if((state == NinjiaState.FIRST_JUMP || state == NinjiaState.SECOND_JUMP) && rigidbody2D.velocity.y < 0)//下落的时候可以攻击
                {
                    SetState(NinjiaState.ATTACK);
                }
            }
        }
#endif
        if (isGod())
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
        }
        else
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void GameStart()
    {
        _isStart = true;
        if (endlessBlock)
            endlessBlock.SendMessage("StopScroll",SendMessageOptions.DontRequireReceiver);
        if (guide)
            guide.GetComponent<Animator>().SetBool("isHide", true);
        foreach (BlockFactory bf in factoryList)
        {
            bf.CreatBlock();
        }
    }

    public void SetState(NinjiaState state)
    {
        this.state = state;
        switch (state)
        {
            case NinjiaState.ATTACK:
                lastAttackTime = Time.time;
                GetComponent<Animator>().SetInteger("attackType",1);
                if (attack)
                    attack.Play();
                break;
            case NinjiaState.SECOND_ATTACK:
                lastAttackTime = 0;
                GetComponent<Animator>().SetInteger("attackType", 2);
                if (attack)
                    attack.Play();
                break;
            case NinjiaState.FIRST_JUMP:
            case NinjiaState.SECOND_JUMP:
                GetComponent<Animator>().SetBool("isJump", true);
                if (run)
                    run.Stop();
                if (jump)
                    jump.Play(); 
                break;
            case NinjiaState.ON_GROUND:
                GetComponent<Animator>().SetBool("isJump", false);
                GetComponent<Animator>().SetInteger("attackType", 0);
                if (run && !run.isPlaying)
                    run.Play();
                break;
            case NinjiaState.DEAD:
                if (run)
                    run.Stop();
                if (dead)
                    dead.Play();
                GetComponent<Animator>().SetBool("isDead", true);
                break;
            case NinjiaState.CRASH:
                GetComponent<Animator>().SetBool("isCrash", true);
                if (run)
                    run.Stop();
                break;
            case NinjiaState.LOSE:
                GetComponent<Animator>().SetBool("isLose", true);
                if (run)
                    run.Stop();
                break;
        }
    }

    void FixedUpdate()
    {
        if (!_isStart)
            endlessBlock.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        else
        {
            if (state != NinjiaState.FALL_INIT && state != NinjiaState.DEAD && state != NinjiaState.CRASH && state != NinjiaState.LOSE)
            {
                gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0);
                updateBG();
            }
            else
            {
                if (cloud)
                    cloud.position += new Vector3(-1 * Time.deltaTime, 0);
            }
        }

    }
    public Transform bgFar;
    public Transform bgNear;
    public Transform bgFront;
    public Transform cloud;
    private void updateBG()
    {
        if (bgFar)
            bgFar.position -= new Vector3(0.1f * speed * Time.deltaTime, 0, 0);
        if (bgNear)
            bgNear.position -= new Vector3(0.2f * speed * Time.deltaTime, 0, 0);
        if (bgFront)
            bgFront.position -= new Vector3(0.6f * speed * Time.deltaTime, 0);
        if(cloud)
            cloud.position += new Vector3((speed-1) * Time.deltaTime, 0);
    }

    private void ResetBG()
    {
        if (bgFar)
        {
            bgFar.position = new Vector3(0, bgFar.position.y, bgFar.position.z);
            bgFar.SendMessage("Reset");
        }
        if (bgNear)
        {
            bgNear.position = new Vector3(0, bgNear.position.y, bgNear.position.z);
            bgNear.SendMessage("Reset");
        }
        if (bgFront)
        {
            bgFront.position = new Vector3(0, bgFront.position.y, bgFront.position.z);
            bgFront.SendMessage("Reset");
        }
        if (cloud)
        {
            cloud.position = new Vector3(0, cloud.position.y, cloud.position.z);
            cloud.SendMessage("Reset");
        }
    }

    private bool _isStart;
    private void Jump()
    {      
        if (state == NinjiaState.ON_GROUND || state == NinjiaState.FALL_INIT)
        {
            gameObject.rigidbody2D.velocity = new Vector2(0, jumpSpeed);
            SetState(NinjiaState.FIRST_JUMP);
        }
        else if (state == NinjiaState.FIRST_JUMP)
        {
            gameObject.rigidbody2D.velocity = new Vector2(0, jumpSpeed2);
            SetState(NinjiaState.SECOND_JUMP);
        }
    }
    private List<GameObject> _hitList;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!_isStart)
            return;
        if (state == NinjiaState.LOSE || state == NinjiaState.CRASH || state == NinjiaState.DEAD)
            return;
        if(endlessBlock != null && coll.transform.parent == endlessBlock.transform)
        {
            SetState(NinjiaState.ON_GROUND);
        }
        if(coll.gameObject.tag == "BottomBlock" || coll.gameObject.tag == "Dangerous")
        {
            Vector2 collNormal = coll.contacts[0].normal;
            if (Mathf.Abs(collNormal.x-0.0f)<0.1f && Mathf.Abs(collNormal.y-1.0f)<0.1f )
            {
                AdjustSpeed(coll.gameObject);
                SetState(NinjiaState.ON_GROUND);
                if (_hitList.IndexOf(coll.gameObject) < 0)
                {
                    foreach (BlockFactory bf in factoryList)
                    {
                        bf.CreatBlock();
                    }
                    _hitList.Add(coll.gameObject);
                }

            }
            else
            {
                Debug.Log(coll.contacts[0].normal);
                OnDead();
            }

        }
    }

    private void AdjustSpeed(GameObject obj)
    {
        if (_hitList.Count>0 && _hitList[_hitList.Count - 1].tag == "Dangerous" && obj.tag != "Dangerous")
            speed *= 1.1f;
    }

     void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "BottomBlock")
        {
            if (run)
                run.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Friend" && !isGod())
        {
            OnCrash();
            coll.gameObject.SendMessage("OnCrash");
        }
        else if (coll.gameObject.tag == "Enemy" && !isGod())
        {
            //OnCrash();
            OnHit();
        }

    }

    private float godTime = 0;
    public GameObject hurtFlash;
    public void OnHit()
    {
        GameData.hp--;
        EventManager.getInstance().trigger(Event_Name.REFRESH_STAR);
        if (GameData.hp <= 0)
            OnDead();
        else
            godTime = Time.time;
        if (hurtFlash)
        {
            hurtFlash.SetActive(false);
            hurtFlash.SetActive(true);
        }
        SetHeadState(4);

    }

    public bool isGod()
    {
        if (godTime > 0 && Time.time - godTime < 2)
            return true;
        else
            return false;
    }

    public GameObject target;
    public void OnAttack()
    {
        UpdateCurrentTarget();
        if (target && (target.transform.position.x - transform.position.x) < attackDistant)
        {
            if (hit)
                hit.Play();
            target.SendMessage("OnHit");
            if (target.tag == "Friend")
            {
                SetState(NinjiaState.LOSE);
                StartCoroutine(GameEnd());
            }
        }
    }

    //更新当前对象
    private void UpdateCurrentTarget()
    {
        target = null;
        GameObject[] friendList = GameObject.FindGameObjectsWithTag("Friend");
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] total = new GameObject[friendList.Length+enemyList.Length];
        friendList.CopyTo(total, 0);
        enemyList.CopyTo(total, friendList.Length);
        float min = float.MaxValue;
        for (int i = 0; i < total.Length; i++)
        {
            if (total[i] != null && !total[i].GetComponent<TargetState>().IsDead)
            {
                float dis = total[i].transform.position.x - transform.position.x;
                if (dis > 0 && dis < min)
                {
                    target = total[i];
                    min = dis;
                }
            }
        }
    }

    public GameObject headState;
    public void SetHeadState(int type)
    {
        if (headState)
        {
            headState.GetComponent<Animator>().SetInteger("type", type);
            StartCoroutine(ClearHeadState());
        }
    }

    IEnumerator ClearHeadState()
    {
        yield return new WaitForSeconds(0.3f);
        headState.GetComponent<Animator>().SetInteger("type", 0);
    }

    public bool CanBeHit()
    {
        return (state == NinjiaState.ON_GROUND) && !isGod() ;
    }

    public bool CanBeKnock()
    {
        return (state == NinjiaState.FIRST_JUMP || state == NinjiaState.SECOND_JUMP) && !isGod();
    }

    public void OnDead()
    {
        rigidbody2D.isKinematic = true;
        GetComponent<TargetState>().IsDead = true;
        SetState(NinjiaState.DEAD);
        Camera.main.GetComponent<CameraShake>().Shake();
        StartCoroutine(GameEnd());
    }

    public void OnCrash()
    {
        if (crash)
            crash.Play();
        SetState(NinjiaState.CRASH);
        Camera.main.GetComponent<CameraShake>().Shake();
        StartCoroutine(GameEnd());
    }

    private IEnumerator GameEnd()
    {
        GameData.hp = 0;
        EventManager.getInstance().trigger(Event_Name.REFRESH_STAR);
        yield return new WaitForSeconds(1.0f);
        GameScene.GotoScene(3);
    }
}
