using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using MxM;
using Cinemachine;
public class Mxmevent : ClimbCheck{ //继承类为攀爬判断
    public MxMAnimator mmAni;//mXm 动作组件
    public CharacterStats cha;//角色数值
    public MxMTrajectoryGenerator mmT;//角色控制
    [Header("Event list")]//所有的事件
    public MxMEventDefinition Jump;
    public MxMEventDefinition Slide;
    public MxMEventDefinition Climb2m;
    public MxMEventDefinition Climb1m;
    public MxMEventDefinition Attack1;
    public MxMEventDefinition Attack2;
    public MxMEventDefinition Dance1;
    public MxMEventDefinition Defend1;
    public MxMEventDefinition Attack3_pre;
    public MxMEventDefinition Attack3;
    //public MxMEventDefinition Climb05m;
    public MxMEventDefinition CrossOver;
    public MxMEventDefinition Sp1;
    public MxMEventDefinition Sp2;
    private MxMEventDefinition NextSp;
    public MxMEventDefinition bwd;
    public MxMEventDefinition fwd1;
    public MxMEventDefinition fwd2;
    public MxMEventDefinition pick;
    public MxMEventDefinition petting;
    [Header("Magic")]//魔法
    public GameObject magic1;
    public GameObject magic2;
    public GameObject healing1;
    public GameObject magic3;
    public GameObject magic3_place;
    public GameObject magic4;
    [Header("MagicianHand")]
    public GameObject Hand;
    [Header("other componets")]//其他组件
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject myBag;
    public DialogueManager dialogueManager;
    public Camera main;
    public GameObject Transport;
    public GameObject mainpos;
    public ClimbCheck climbCheck;
    public Rigidbody rrigidbody;
    public UnityEvent leftClick;
    public UnityEvent rightClick;
    private Vector3 playerMovementWorldSpace = Vector3.zero;
    private GameObject attackTarget;
    private float cooldowntime = 0.5f;
    const float rigidcooldown = 4f;
    private float rigidtime = rigidcooldown;
    private Tagswitch tagswitch;
    private NextPlayerMovement jumpEvent;
    private bool isLanding;
    public bool isroom;
    private float magic1_time;
    private float spSwitchtime;
    private float kineDelaytime;
    private float defendBUffTime = 0;
    private bool isArmed;
    public bool ispick;
    private bool start_magic;   //是否释放魔法
    private bool re_ReadHand;   //是否在手部搓出球球
    private bool attack3_ready; //两段式攻击
    private bool dontDestory;  //是否在生成之后下一个魔法诞生时候被摧毁（solo？）
    private Vector3 magic_pos = new Vector3(); //魔法释放位置
    private Vector3 magic_offset = new Vector3(); //魔法释放偏执
    private GameObject magic_effect;
    private GameObject readyToEf;
    //
    
    private bool isOpen = false;
    //private bool isCrouch;
    // Update is called once per frame
    private void Start()
    {
        myBag.SetActive(isOpen);
        cha = transform.GetComponent<CharacterStats>();
        tagswitch = transform.GetComponent<Tagswitch>();
        leftClick.AddListener(new UnityAction(ButtonLeftClick));
        rightClick.AddListener(new UnityAction(ButtonRightClick));
        NextSp = Sp1;
        spSwitchtime = 0f;
        isroom = false;
        start_magic = false;
        re_ReadHand = false;
        attack3_ready = false;
        dontDestory = false;
        kineDelaytime = 0f;
        jumpEvent = NextPlayerMovement.Nothing;
    }
    void Update()
    {
        OpenMyBag();//打开背包？
        magic11();//魔法管理
        UpdateRiG();//更新刚体
        CheckRigid();//检查刚体
        defendCheck();//检查防御力
        playerMovementWorldSpace = rrigidbody.velocity.normalized;
        cooldowntime -= Time.deltaTime;//共用冷却时间缩减
        spSwitchtime -= Time.deltaTime;// sp冷却时间缩减
        //isCrouch = transform.GetComponent<Tagswitch>().sflag;
        if (tagswitch.sflag == true) return;
        //Debug.Log(jumpEvent);
        if ((Input.GetButton("Jump") && cooldowntime <= 0) && jumpEvent == NextPlayerMovement.Nothing)
        {
            if (isArmed)
            {
                mmAni.BeginEvent(bwd);
                cooldowntime = 1.1f;
                return;
            }
            //Debug.Log("Enter jump!");
            jumpEvent = climbCheck.ClimbDetection(transform, playerMovementWorldSpace, 2f);
            if (transform.GetComponent<AudioSource>().isPlaying)
            {
                if (magic_effect != null) Destroy(magic_effect);
                transform.GetComponent<AudioSource>().Stop();

            }
            switch (jumpEvent)//事件切换
            {
                case NextPlayerMovement.climbHigh:
                    rrigidbody.useGravity = false;
                    rigidtime = rigidcooldown;
                    mmAni.BeginEvent(Climb2m);
                    cooldowntime = rigidcooldown;
                    break;
                case NextPlayerMovement.climbLow:
                    rrigidbody.useGravity = false;
                    rigidtime = 0.6f; //
                    mmAni.BeginEvent(Climb1m);
                    cooldowntime = 0.6f; //
                    break;
                case NextPlayerMovement.vault:
                    rrigidbody.useGravity = false;
                    rigidtime = rigidcooldown * 0.25f; //
                    mmAni.BeginEvent(CrossOver);
                    cooldowntime = rigidcooldown * 0.25f; //
                    break;
                case NextPlayerMovement.jump:
                    rrigidbody.useGravity = false;
                    rigidtime = 0.6f; //
                    mmAni.BeginEvent(Jump);
                    cooldowntime = 0.6f; //
                    break;
            }
        }
        else if (Input.GetButton("Slide") && cooldowntime <= 0)
        {
            mmAni.BeginEvent(Slide);
            cooldowntime = 1f;
        }
        else if (Input.GetButton("Attack1") && cooldowntime <= 0)
        {
            mmAni.BeginEvent(Attack1);
            re_ReadHand = true;
            //magic_pos = Hand.transform.position;
            start_magic = true;
            readyToEf = magic1;
            magic1_time = 1.5f;
            cooldowntime = 2f;
        }
        else if (Input.GetButton("Attack2") && cooldowntime <= 0)
        {
            lookatEnemy();
            mmAni.BeginEvent(Attack2);
            re_ReadHand = true;
            //magic_pos = Hand.transform.position;
            start_magic = true;
            readyToEf = magic2;
            magic1_time = 1.2f;
            //if (magic_effect != null) Destroy(magic_effect);
            //magic_effect = Instantiate(magic2, Hand.transform.position, Quaternion.identity);
            // magic_effect.GetComponent<Collider>().enabled = false;
            cooldowntime = 4f;
        }else if(Input.GetButton("Dance1") && cooldowntime <= 0)
        {
            if (transform.GetComponent<AudioSource>().isPlaying)
            {
                if (magic_effect != null) Destroy(magic_effect);
                transform.GetComponent<AudioSource>().Stop();

            }
            mmAni.BeginEvent(Dance1);
            readyToEf = healing1;
            transform.GetComponent<AudioSource>().Play();
            magic_effect = Instantiate(readyToEf, transform.position, Quaternion.identity);

            cooldowntime = 4f;
        }else if(Input.GetButton("Defend1") && cooldowntime <= 0)
        {
            mmAni.BeginEvent(Defend1);
            defendBUffTime = 10f;
            magic_pos = transform.position;
            start_magic = true;
            readyToEf = magic4;
            magic1_time = 1.8f;
            cooldowntime = 3.2f;
        }else if(Input.GetButton("Attack3") && cooldowntime <= 0)
        {
            lookatEnemy();
            mmAni.BeginEvent(Attack3_pre);
            magic_pos = transform.position;
            start_magic = true;
            attack3_ready = true;
            readyToEf = magic3_place;
           // magic_offset = new Vector3(0, -0.5f, 0);
            magic1_time = 0.6f;
            cooldowntime = 1.7f;
        }else if (attack3_ready && cooldowntime<= 0)
        {
            mmAni.BeginEvent(Attack3);
            re_ReadHand = true;
            start_magic = true;
            dontDestory = true;
            readyToEf = magic3;
            magic1_time = 2.7f;
            cooldowntime = 4f;
            attack3_ready = false;
        }else if(Input.GetMouseButtonDown(0) && cooldowntime <= 0)
        {
            leftClick.Invoke();
        }else if(Input.GetMouseButtonDown(1) && cooldowntime <= 0)
        {
            rightClick.Invoke();
        }else if (Input.GetButton("Pickup") && cooldowntime <= 0 && ispick)//todo: 改拾取为其他脚本调用
        {
            
            mmAni.BeginEvent(pick);
            ispick = false;
            cooldowntime = 1.5f;
        }else if(Input.GetButton("Camera") && cooldowntime <= 0)
        {
            if (main.GetComponent<CinemachineBrain>().isActiveAndEnabled)
            {
                main.GetComponent<CinemachineBrain>().enabled = false;
                main.transform.position = mainpos.transform.position;
                main.transform.rotation = mainpos.transform.rotation;
            }
            else
            {
                main.GetComponent<CinemachineBrain>().enabled = true;
            }
            cooldowntime = 2f;
            
        }else if(Input.GetButton("Petting") && cooldowntime <= 0)
        {
            mmAni.BeginEvent(petting);
            cooldowntime = 2f;
        }else if(Input.GetButton("Sit") && cooldowntime <= 0)
        {
            transform.position = Transport.transform.position;
            cooldowntime = 2f;
        }else if (Input.GetButton("H") && cooldowntime <= 0 && isroom)
        {
            if (Camera1.transform.GetComponent<CinemachineFreeLook>().isActiveAndEnabled)
            {
                Camera1.transform.GetComponent<CinemachineFreeLook>().enabled = false;
                Camera2.transform.GetComponent<CinemachineVirtualCamera>().enabled = true;
            }
            else
            {
                Camera1.transform.GetComponent<CinemachineFreeLook>().enabled = true;
                Camera2.transform.GetComponent<CinemachineVirtualCamera>().enabled = false;
            }
        }
        else if(Input.anyKey && cooldowntime <= 0 )
        {
            

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(jumpEvent == NextPlayerMovement.jump)
        {
            jumpEvent = NextPlayerMovement.Nothing;
        }
    }
    private void magic11()
    {
        isArmed = tagswitch.kflag;
        if (start_magic == true)
        {
            if(magic1_time >= 0)
            {
                magic1_time -= Time.deltaTime;
            }
            else
            {
                if(re_ReadHand) magic_pos = Hand.transform.position;
                if (magic_effect != null && dontDestory == false) Destroy(magic_effect);
                dontDestory = false;
                magic_effect = Instantiate(readyToEf, magic_pos, Quaternion.identity);
                if (readyToEf == magic1) magic1_check();
                if (magic_effect.GetComponent<ProjectileMoveScript>()!=null)
                {
                    if(attackTarget!=null)
                        magic_offset = (attackTarget.transform.position - magic_pos);
                    else
                        magic_offset = new Vector3(0, 0, 0);
                    magic_effect.GetComponent<ProjectileMoveScript>().SetOffset(magic_offset);
                    
                }
                start_magic = false;
                re_ReadHand = false;
            }
            
        }
    }
    private void CheckRigid()
    {
        if(kineDelaytime > 0)
            kineDelaytime -= Time.deltaTime;
        if((jumpEvent == NextPlayerMovement.Nothing && tagswitch.sflag == false)&& kineDelaytime <= 0)
        {
            rrigidbody.useGravity = true;
            rrigidbody.isKinematic = false;
        }else if(tagswitch.sflag == true)
        {
            kineDelaytime = 4f;
        }
    }
    void UpdateRiG()
    {
        switch (jumpEvent)
        {
            case NextPlayerMovement.climbHigh:
                if (rrigidbody.useGravity == false || rrigidbody.isKinematic == true)
                {
                    rigidtime -= Time.deltaTime;
                    if (rigidtime <= rigidcooldown - 0.6f && rigidtime > rigidcooldown - 3f)
                    {
                        rrigidbody.useGravity = true;
                        rrigidbody.isKinematic = true;
                    }
                    else if (rigidtime <= rigidcooldown - 3f && rigidtime > 0)
                    {
                        rrigidbody.isKinematic = false;
                        rrigidbody.useGravity = false;
                    }
                    else if (rigidtime <= 0)
                    {
                        rrigidbody.useGravity = true;
                        jumpEvent = NextPlayerMovement.Nothing;
                        //rrigidbody.isKinematic = false;
                    }
                }
                break;
            case NextPlayerMovement.climbLow:
                //Debug.Log("11111");
                if (rrigidbody.useGravity == false || rrigidbody.isKinematic == true)
                {
                    rigidtime -= Time.deltaTime;

                    if (rigidtime <= 0)
                    {
                        rrigidbody.useGravity = true;
                        jumpEvent = NextPlayerMovement.Nothing;
                        //rrigidbody.isKinematic = false;
                    }
                }
                break;
            case NextPlayerMovement.vault:
                if (rrigidbody.useGravity == false || rrigidbody.isKinematic == true)
                {
                    rigidtime -= Time.deltaTime;
                    if (rigidtime <= 0.6f && rigidtime > 0.2f)
                    {
                        rrigidbody.useGravity = true;
                        rrigidbody.isKinematic = true;
                    }
                    else if (rigidtime <= 0.2f && rigidtime > 0)
                    {
                        rrigidbody.isKinematic = false;
                        rrigidbody.useGravity = false;
                    }
                    else if (rigidtime <= 0)
                    {
                        rrigidbody.useGravity = true;
                        jumpEvent = NextPlayerMovement.Nothing;
                        //rrigidbody.isKinematic = false;
                    }
                }
                break;
            case NextPlayerMovement.jump:
                if (rrigidbody.useGravity == false || rrigidbody.isKinematic == true)
                {
                    rigidtime -= Time.deltaTime;

                    if (rigidtime <= 0)
                    {
                        rrigidbody.useGravity = true;
                        //rrigidbody.isKinematic = false;
                    }
                }
                break;
        }

    }
    void defendCheck()
    {
        if(defendBUffTime > 0)
        {
            defendBUffTime -= Time.deltaTime;
            cha.defence = 0.4f;
        }
        else
        {
            cha.defence = 0f;
        }
    }
    private void ButtonLeftClick()
    {
        //Debug.Log("Left Click");
        if (isArmed)
        {
            if (tagswitch.kflag) lookatEnemy();
            if (spSwitchtime > 0)
            {
                mmAni.BeginEvent(NextSp);
                if (NextSp == Sp2)
                {
                    NextSp = Sp1;
                    cooldowntime = 2f;

                }
                else
                {
                    NextSp = Sp2;
                    cooldowntime = 1.7f;
                }                
                spSwitchtime = 4f;
            }
            else
            {
                mmAni.BeginEvent(Sp1);
                NextSp = Sp2;
                cooldowntime = 1.7f;
                spSwitchtime = 4f;
            }
            attack_check();
        }
    }
    private void ButtonRightClick()
    {
        //Debug.Log("Right Click");
        if (isArmed)
        {
            if (spSwitchtime > 0)
            {
                mmAni.BeginEvent(fwd2);
                cooldowntime = 1.05f;
            }
            else
            {
                mmAni.BeginEvent(fwd1);
                cooldowntime = 1.3f;
            }
        }
    }
    private void magic1_check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, cha.AttackData.Attack_Range[1]);
        // Debug.Log(colliders.Length);
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.tag);
            if (collider.tag == "Enemy")
            {
                // 造成伤害
                var target = collider.gameObject;
                Vector3 direction = (target.transform.position - transform.position + Vector3.up).normalized + Vector3.up;
                target.GetComponent<NavMeshAgent>().isStopped = true;
                target.GetComponent<NavMeshAgent>().velocity = direction * 15;
                target.GetComponent<Animator>().SetTrigger("hit");
                target.GetComponent<CharacterStats>().TakeDamage(cha, 1, target.GetComponent<CharacterStats>());
                Debug.Log("击飞！");
            }
        }
    }
    private void attack_check()
    {
       // Debug.Log("ATTACK ENTER");
        Vector3 forward = transform.forward;
        Quaternion rotation = Quaternion.Euler(0, 120 * Mathf.Deg2Rad, 0);
        Vector3 leftVector = rotation * forward;
        Vector3 rightVector = Quaternion.Inverse(rotation) * forward;
        Collider[] colliders = Physics.OverlapSphere(transform.position, cha.AttackData.Attack_Range[0]);
       // Debug.Log(colliders.Length);
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.tag);
            if ((Vector3.Angle(transform.forward,collider.transform.position - transform.position)<=60)&& collider.tag == "Enemy")
            {
                // 造成伤害
                SwordHit(collider.gameObject);
                Debug.Log("造成伤害！");
            }
        }
    }
    void SwordHit(GameObject attackTarget)
    {
        if(attackTarget != null)
        {
            var TargetStatus = attackTarget.transform.GetComponent<CharacterStats>();
            TargetStatus.TakeDamage(cha, 0, TargetStatus);
            float randomValue = Random.value;
            if (randomValue < 0.5f)
            {
                attackTarget.transform.GetComponent<Animator>().SetTrigger("hit");
            }
        }
    }
    private void lookatEnemy()
    {
        Debug.Log("Enter lookat");
        Collider[] enemies;
        //LayerMask enemyMask = 3;
        Collider nearestEnemy;
        Vector3 nearestEnemyPos;
        enemies = Physics.OverlapSphere(transform.position, 10);
        //Debug.Log(enemies.Length);
        if (enemies.Length > 0)
        {
            nearestEnemy = null;
            nearestEnemyPos = Vector3.zero;

            foreach (Collider enemy in enemies)
            {
                if (enemy.tag != "Enemy") continue;
                Vector3 enemyPos = enemy.transform.position;
                if (nearestEnemy == null || Vector3.Distance(enemyPos, transform.position) <
                    Vector3.Distance(nearestEnemyPos, transform.position))
                {
                    nearestEnemy = enemy;
                    attackTarget = nearestEnemy.gameObject;
                    nearestEnemyPos = enemyPos;
                }
            }
            if (nearestEnemy == null) return;
            nearestEnemyPos.y = transform.position.y;
            transform.LookAt(nearestEnemyPos);  // 锁定最近敌人
        }
    }
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            myBag.SetActive(isOpen);
        }
    }

}
