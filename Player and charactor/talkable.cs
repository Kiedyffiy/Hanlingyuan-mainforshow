using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class talkable : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private bool isTalk;
    public bool okidle;
    public float checktime = 10f;
    private float checknowtime;
    void Start()
    {
        anim = GetComponent<Animator>();
        checknowtime = checktime;
    }

    // Update is called once per frame
    void Update()
    {
        checknowtime -= Time.deltaTime;
        if(checknowtime < 0 && isTalk == false && okidle)
        {
            isOtherIdle();
            checknowtime = checktime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector3 direction = collision.gameObject.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(forward, direction);
            //Debug.Log(angle);
            //if ()
        } 
    }
    private void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            anim.SetBool("talking", true);
            isTalk = true;
            Vector3 direction = other.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(forward, direction);
            if (Vector3.Cross(forward, direction).y < 0)
            {
                angle *= -1;
            }
            //Debug.Log(angle);
            if (angle<=-45f && angle > -135f)
            {
                anim.SetTrigger("rotatel90");
            }else if(angle <= -135f)
            {
                anim.SetTrigger("rotatel180");
            }else if(angle >=45f && angle < 135f)
            {
                anim.SetTrigger("rotater90");
            }else if(angle >= 135f)
            {
                anim.SetTrigger("rotater180");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTalk = false;
            anim.SetBool("talking", false);
        }
    }
    private void isOtherIdle()
    {
        if(Random.value < 0.5f)
        {
            if(Random.value < 0.5f)
            {
                anim.SetTrigger("idle2");
            }
            else
            {
                anim.SetTrigger("idle3");
            }
        }
    }
}
