using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{
    //���ݼ��������ҿ��ܵ���һ���ж���Ĭ��Ϊjump
    public enum NextPlayerMovement
    {
        jump,   //��Ծ
        climbLow,    //��λ����
        climbHigh,   //��λ����
        vault,        //��Խ
        Nothing
    }
    public NextPlayerMovement nextMovement = NextPlayerMovement.jump;

    [Header("��λ���������޸߶ȣ����ڴ�ֵ�������������ڴ�ֵʹ�õ�λ��������")]
    public float lowClimbHeight = 0.5f;

    [Header("��λ���������޸߶ȣ����ڴ�ֵʹ�ø�λ��������")]
    public float hightClimbHeight = 1.6f;

    [Header("�����ļ�����")]
    public float checkDistance = 1f;

    [Header("�����ĽǶ����ƣ����ǰ�����ϰ��﷨��֮��ļнǴ��ڴ�ֵʱ��������")]
    public float climbAngle = 45f;

    [Header("��ҿɽ��ܵķ�Խ��������Ϯ�߶�")]
    public float bodyHeight = 1f;

    [Header("��ҿ��Է�Խ�ĺ��")]
    public float valutDistance = 0.2f;


    float climbDistance;  //������ʵ�ʾ��룬����ǽ��С���������Ż�����
    Vector3 ledge;    //ǽ�ڱ�Ե
    Vector3 climbHitNormal;   //ǽ�ڷ���
    public Vector3 Ledge { get => ledge; }
    public Vector3 ClimbHitNormal { get => climbHitNormal; }


    private void Start()
    {
        climbDistance = Mathf.Cos(climbAngle) * checkDistance;
    }

    /// <summary>
    /// ���ǰ�����ϰ���ڽ�ɫ��������jump�����ڵ���
    /// </summary>
    /// <param name="playerTransform">��ҵ�transform��Ϣ���ɵ����ߴ���</param>
    /// <param name="inputDirection">��ҵ�ǰ�ķ���������Ϣ</param>
    /// <param name="offset">����ƶ��ٶȴ���������ƫ�������ٶ�Խ����ܹ����ϸ�Զ��ǽ�ڣ�</param>
    /// <returns>����ֵ����ҽ��������˶�״̬</returns>
    public NextPlayerMovement ClimbDetection(Transform playerTransform, Vector3 inputDirection, float offset)
    {
        float climbOffset = Mathf.Cos(climbAngle) * offset;
        //����λ,���߶���lowClimbHeight����������checkDistance
        if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimbHeight, playerTransform.forward, out RaycastHit obsHit, checkDistance + offset))
        {
            climbHitNormal = obsHit.normal;
            Debug.Log("��λ���ͨ��" + obsHit.normal);
            Debug.Log("playerTransform.forward" + playerTransform.forward);
            Debug.Log(Vector3.Angle(-climbHitNormal, playerTransform.forward));
            //Debug.Log(inputDirection);
            //(��ҳ���������뷽��)�ǶȲ���Ҫ����ǽ�淨�߽Ƕȴ������ƽǶȣ��������Ծ
            if (Vector3.Angle(-climbHitNormal, playerTransform.forward) > climbAngle)//|| Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle
            {
                Debug.Log("�ǶȲ���");
                return NextPlayerMovement.jump;
            }

            //��ǽ�ڷ��߷����ټ��һ�ε�λ����������climbDistance
            if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimbHeight, -climbHitNormal, out RaycastHit firstWallHit, climbDistance + climbOffset))
            {
                Debug.Log("��λ���߷�����ͨ��" + firstWallHit.normal);
                //�������һ����λbodyHeight���ټ��һ��
                if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight), -climbHitNormal, out RaycastHit secondWallHit, climbDistance + climbOffset))
                {

                    Debug.Log("����һ����λ���߷�����ͨ��");
                    //Debug.Log(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight * 2));
                    //�������������λbodyHeight���ټ��һ��
                    if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight * 2), -climbHitNormal, out RaycastHit thirdWallHit, climbDistance + climbOffset))
                    {
                        Debug.Log("����������λ���߷�����ͨ��");
                        //�������������λbodyHeight���ټ��һ�Σ��Ծɼ�⵽�ϰ��������Ծ
                        if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight * 3), -climbHitNormal, climbDistance + offset))
                        {
                            Debug.Log("̫����");
                            return NextPlayerMovement.jump;
                        }

                        //��������λû�м�⵽�ϰ������ӵڶ�����λ����һ����λ��Ϊֹ�����¼�⣬��ײ�㼴Ϊǽ�ߣ����ִ�и�λ����
                        else if (Physics.Raycast(thirdWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                        {
                            Debug.Log("�ڵڶ�����λ�Ϸ���⵽��Ե");
                            ledge = ledgeHit.point;
                            return NextPlayerMovement.climbHigh;
                        }
                    }

                    //�ڶ�����λû�м�⵽�ϰ������ӵ�һ����λ����һ����λ��Ϊֹ�����¼�⣬��ײ�㼴Ϊǽ�ߣ����ִ�е�λ����
                    else if (Physics.Raycast(secondWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                    {
                        
                        Debug.Log("�ڵ�һ����λ�Ϸ���⵽��Ե");
                        ledge = ledgeHit.point;
                        //Debug.Log(ledge);
                        //Debug.Log(playerTransform.position.y);
                        if (ledge.y - playerTransform.position.y > hightClimbHeight)
                        {
                            //Debug.Log("������");
                            return NextPlayerMovement.climbHigh;
                        }
                        //���ڵ�λ�����߶ȣ�����Ƿ���Է�Խ����⵽����㹻����ʹ�õ�λ����
                        else if (Physics.Raycast(secondWallHit.point + Vector3.up * bodyHeight - climbHitNormal * valutDistance, Vector3.down, bodyHeight))
                        {
                            return NextPlayerMovement.climbLow;
                        }
                        else
                        {
                            return NextPlayerMovement.vault;
                        }
                    }
                }
                else if (Physics.Raycast(firstWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                {
                    Debug.Log("�ڵ�λ�Ϸ���⵽��Ե");
                    ledge = ledgeHit.point;
                    //���ڵ�λ�����߶ȣ�����Ƿ���Է�Խ����⵽����㹻����ʹ�õ�λ����
                    if (Physics.Raycast(firstWallHit.point + Vector3.up * bodyHeight - climbHitNormal * valutDistance, Vector3.down, out ledgeHit, bodyHeight))
                    {
                        return NextPlayerMovement.climbLow;
                    }
                    else
                    {
                        return NextPlayerMovement.vault;
                    }
                }
            }
        }
        Debug.Log("ɶҲ���ǣ�����С����");
        return NextPlayerMovement.jump;
    }
}
