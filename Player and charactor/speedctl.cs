using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;


public class speedctl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject character;
    public float ws;
    public float wp;
    public float wd;
    public float rs;
    public float rp;
    public float rd;
    public float ss;
    public float sp;
    public float sd;
    private MxMAnimator mmAni;
    private MxMTrajectoryGenerator mmT;

    void Start()
    {
        mmAni = character.GetComponentInChildren<MxMAnimator>();
        mmT = character.GetComponentInChildren<MxMTrajectoryGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Sprint"))
        {
            mmT.MaxSpeed = ss;
            mmT.PositionBias = sp;
            mmT.DirectionBias = sd;
        }
        else if(Input.GetButton("Walk"))
        {
            mmT.MaxSpeed = ws;
            mmT.PositionBias = wp;
            mmT.DirectionBias = wd;
        }
        else if(Input.GetButtonDown("Run"))
        {
            mmT.MaxSpeed = rs;
            mmT.PositionBias = rp;
            mmT.DirectionBias = rd;
        }else if (Input.GetButtonDown("Crouch"))
        {
            mmT.MaxSpeed = ws;
            mmT.PositionBias = wp;
            mmT.DirectionBias = wd;
        }
    }
}
