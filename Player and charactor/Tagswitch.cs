using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;
using UnityEngine.Animations;

public class Tagswitch : MonoBehaviour//本类实现对Mxm的需求tag的获取
{
    // Start is called before the first frame update
    public MxMAnimator ani;
    public MxMTrajectoryGenerator mmT;
    public bool cflag = false;
    public bool kflag = false;
    public bool sflag = false;
    private float switchtime = 0.5f;
    private float cooldown = 0.5f;
    public Rigidbody rigidbody;
    public GameObject sword;
    public GameObject Sitpos;
    public DialogueManager dialogueManager;
    public bool readytosit;
    public float sittime;
    public MxMEventDefinition CrouchTrans;
    public MxMEventDefinition KanataTrans;
    public MxMEventDefinition Startsit;
    public MxMEventDefinition Endsit;
    public MxMEventDefinition Unequip;
    private void Start()
    {
        sittime = 0f;
        readytosit = false;
        //rigidbody = transform.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (sittime > 0) sitcool();
        switchtime -= Time.deltaTime;
        if ((Input.GetButton("Crouch") && cflag == false)&& switchtime <= 0)
        {
            switchtime = cooldown;
            Debug.Log("enter crouch");
            ani.SetRequiredTag("Crouch");
            kflag = false;
            cflag = true;
            sflag = false;
        }
        else if((Input.GetButton("Crouch") && cflag == true) && switchtime <= 0)
        {
            switchtime = cooldown;
            Debug.Log("out! crouch");
            ani.ClearRequiredTags();
            kflag = false;
            cflag = false;
            sflag = false;
            ani.BeginEvent(CrouchTrans);
        }else if((Input.GetButton("Kanata") && kflag == false) && switchtime <= 0)
        {
            switchtime = cooldown;
            Debug.Log("enter kanata");
            ani.SetRequiredTag("Kanata");
            ani.BeginEvent(KanataTrans);
            List < ConstraintSource >  clist = new List<ConstraintSource>() { };
            sword.transform.GetComponent<ParentConstraint>().GetSources(clist);
            Transform targetTrans0 = clist[0].sourceTransform;
            ConstraintSource constraintSource0 = new ConstraintSource() { sourceTransform = targetTrans0, weight = 0 };
            Transform targetTrans1 = clist[1].sourceTransform;
            ConstraintSource constraintSource1 = new ConstraintSource() { sourceTransform = targetTrans1, weight = 1 };
            sword.transform.GetComponent<ParentConstraint>().SetSources(new List<ConstraintSource>() { constraintSource0, constraintSource1 });
            kflag = true;
            cflag = false;
            sflag = false;
        }
        else if ((Input.GetButton("Kanata") && kflag == true) && switchtime <= 0)
        {
            switchtime = cooldown;
            Debug.Log("out! kanata");
            ani.ClearRequiredTags();
            ani.BeginEvent(Unequip);
            List<ConstraintSource> clist = new List<ConstraintSource>() { };
            sword.transform.GetComponent<ParentConstraint>().GetSources(clist);
            Transform targetTrans0 = clist[0].sourceTransform;
            ConstraintSource constraintSource0 = new ConstraintSource() { sourceTransform = targetTrans0, weight = 1 };
            Transform targetTrans1 = clist[1].sourceTransform;
            ConstraintSource constraintSource1 = new ConstraintSource() { sourceTransform = targetTrans1, weight = 0 };
            sword.transform.GetComponent<ParentConstraint>().SetSources(new List<ConstraintSource>() { constraintSource0, constraintSource1 });
            kflag = false;
            cflag = false;
            sflag = false;
        }else if((readytosit && sflag == false) && switchtime <= 0)
        {
            readytosit = false;
            sflag = true;
            switchtime = cooldown*2;
            Debug.Log("enter! sit");
            rigidbody.isKinematic = true;
            TranSport(Sitpos);
            ani.BeginEvent(Startsit);
            ani.SetRequiredTag("Sit");
            kflag = false;
            cflag = false;
            //sittime = 5f;
        }
        else if ((readytosit && sflag == true) && switchtime <= 0)
        {
            readytosit = false;
            switchtime = cooldown*2;
            Debug.Log("out! sit");
            ani.ClearRequiredTags();
            ani.BeginEvent(Endsit);
            sflag = false;
            kflag = false;
            cflag = false;
        }
    }
    void TranSport(GameObject target)
    {
        Transform targetTransform = target.transform;
        Vector3 targetPos = targetTransform.position + targetTransform.forward * 0.6f;
        Quaternion lookRotation = Quaternion.LookRotation(targetTransform.forward * -1);
        transform.position = targetPos;
        transform.rotation = lookRotation;
    }
    void sitcool()
    {
        sittime -= Time.deltaTime;
        if(sittime <= 0)
        {
            dialogueManager.dialogbuttonInit(5003);
            
        }
    }
}
