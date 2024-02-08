using StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TestBoss : Monster
{
    RandomBox<BossPattern> m_pt_short = new RandomBox<BossPattern>();
    RandomBox<BossPattern> m_pt_mid = new RandomBox<BossPattern>();
    RandomBox<BossPattern> m_pt_far = new RandomBox<BossPattern>();
    ConditionPicker<RandomBox<BossPattern>> m_ConditionPicker = new ConditionPicker<RandomBox<BossPattern>>();

    StateMachine<TestBoss> m_StateMachine = new StateMachine<TestBoss>();
    Animator m_Animator = null;

    enum BossPattern
    {
        NONE, 
        ATTACK_1, 
        ATTACK_2, 
        ATTACK_3,
        MOVE_TO_PLAYER,
        BASH,
        JUMP_ATTACK,
        END
    }

    float m_AccTime;

    // Start is called before the first frame update
    void Start()
    {
        m_AIController.Regist(BossPattern.ATTACK_1, () => { int a = 10; });
        m_AIController.Regist(BossPattern.ATTACK_2, () => { int a = 10; });
        m_AIController.Regist(BossPattern.ATTACK_3, () => { int a = 10; });
        m_AIController.Regist(BossPattern.MOVE_TO_PLAYER, () => { int a = 10; });
        m_AIController.Regist(BossPattern.JUMP_ATTACK, () => { int a = 10; });

        m_ConditionPicker.Add(m_pt_short, () => { return 10 < GetPlayerDistanceFloat(); });
        m_ConditionPicker.Add(m_pt_mid, () => { return 30 < GetPlayerDistanceFloat(); });
        m_ConditionPicker.Add(m_pt_far, () => { return true; });

        m_Animator = GetComponent<Animator>();

        m_AIController.ActionChangeable = false;
        m_StateMachine.ChangeState(this, new StateIdle());
    }

    // Update is called once per frame
    void Update()
    {
        if (0 == m_AIController.GetReserveCount())
        {
            m_AccTime += Time.deltaTime;
            if (m_AccTime > 3)
            {
                m_AIController.ReserveAction(m_ConditionPicker.GetItem().GetRandomItem());
                m_AccTime = 0;
            }
        }
        else
        {
            m_AccTime = 0;
        }
        m_AIController.Update();
        m_StateMachine.UpdateState(this);
    }

}