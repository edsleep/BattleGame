using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FollowCam : MonoBehaviour
{
    public Vector2 m_Offset = Vector2.zero;
    public Vector3 m_CurrentPosition = Vector3.zero;
    public GameObject m_TargetObject;

    private Vector3 TestOld;
    [Header("0 ~ 1")]
    public float m_FollowSpeed;


    public Vector2 m_target_cul = Vector2.zero;
    public Vector3 m_velo = Vector2.zero;

    void Start()
    {
        //StartCoroutine(LateFixedUpdate());  
    }


    //IEnumerator LateFixedUpdate()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForFixedUpdate();
    //        if (m_TargetObject == null)
    //            continue;

    //        var targetPos = m_TargetObject.transform.position;
    //        var lerp_pos = Vector2.Lerp(m_CurrentPosition, targetPos, m_FollowSpeed * Time.fixedDeltaTime);
    //        float halfX = m_target_cul.x / 2.0f;
    //        float halfY = m_target_cul.y / 2.0f;
    //        if (halfX < Mathf.Abs(m_CurrentPosition.x - targetPos.x))
    //        {
    //            if (m_CurrentPosition.x < targetPos.x)
    //                m_CurrentPosition.x = targetPos.x - halfX;
    //            else
    //                m_CurrentPosition.x = targetPos.x + halfX;
    //        }
    //        else
    //        {
    //            m_CurrentPosition.x = lerp_pos.x;
    //        }

    //        if (halfY < Mathf.Abs(m_CurrentPosition.y - targetPos.y))
    //        {
    //            if (m_CurrentPosition.y < targetPos.y)
    //                m_CurrentPosition.y = targetPos.y - halfX;
    //            else
    //                m_CurrentPosition.y = targetPos.y + halfX;
    //        }
    //        else
    //        {
    //            m_CurrentPosition.y = lerp_pos.y;
    //        }
    //        m_CurrentPosition.z = -10;
    //        transform.position = m_CurrentPosition + new Vector3(m_Offset.x, m_Offset.y, 0);
    //    }
    //}


    private void LateUpdate()
    {

        m_CurrentPosition.z = m_TargetObject.transform.position.z;
        m_CurrentPosition = Vector3.SmoothDamp(m_CurrentPosition, m_TargetObject.transform.position, ref m_velo, m_FollowSpeed);
        m_CurrentPosition.z = -10;

        transform.position = m_CurrentPosition + new Vector3(m_Offset.x, m_Offset.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(m_target_cul.x, m_target_cul.y, 1));
    }
}
