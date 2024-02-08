using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected ObjectAIController m_AIController = new ObjectAIController();
    // 플레이어와의 거리를 계산하는 함수
    protected Vector2 GetPlayerDistanceVec2()
    {
        if(null == Player.m_self)
            return Vector2.zero;

        return Player.m_self.transform.position - transform.position;
    }

    protected float GetPlayerDistanceFloat()
    {
        if (null == Player.m_self)
            return 0;

        return Vector3.Distance(Player.m_self.transform.position, transform.position);
    }











}
