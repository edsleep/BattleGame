using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ObjectAIController
{
    Dictionary<int, Action> m_dic_action = new Dictionary<int, Action>();
    Queue<int> m_queue_action = new Queue<int>();
    public bool ActionChangeable { get; set; } = false;

    public bool Update()
    {
        if (m_dic_action.Count <= 0)
            return false;

        if (false == ActionChangeable || m_queue_action.Count <= 0)
            return true;

        m_dic_action[m_queue_action.Dequeue()]?.Invoke();
        return true;
    }

    public bool Regist(int in_Key, Action action)
    {
        if (m_dic_action.ContainsKey(in_Key))
            return false;
        m_dic_action.Add(in_Key, action);       
        return true;
    }

    public bool Regist<T>(T in_KeyEnum, Action action) where T : Enum
    {
        int key = Util.Enum32ToInt(in_KeyEnum);
        return Regist(key, action);
    }

    // 기존에 등록된 행동을 덮어씌웁니다.
    // 만약 등록되어 있지 않다면 false를 반환합니다.
    public bool SetAction(int in_Key, Action action)
    {
        if (false == m_dic_action.ContainsKey(in_Key))
            return false;

        m_dic_action[in_Key] = action; 
        return true;
    }

    public bool SetAction<T>(T in_KeyEnum, Action action) where T : Enum
    {
        int key = Util.Enum32ToInt(in_KeyEnum);
        return SetAction(key, action);
    }

    public void DeleteAllAction()
    {
        m_dic_action.Clear();
    }

    public void ClearActionQueue()
    {
        m_queue_action.Clear();
    }

    public int GetReserveCount()
    {
        return m_queue_action.Count;
    }


    public bool ReserveAction(int in_Key)
    {
        if (false == m_dic_action.ContainsKey(in_Key))
            return false;

        m_queue_action.Enqueue(in_Key);
        return true;
    }


    public bool ReserveAction<T>(T in_KeyEnum) where T : Enum
    {
        int key = Util.Enum32ToInt(in_KeyEnum);
        return ReserveAction(key);
    }

    public bool ForceChangeAction(int in_Key)
    {
        if (false == m_dic_action.ContainsKey(in_Key))
            return false;

        m_dic_action[in_Key]?.Invoke();
        return true;
    }

    public bool ForceChangeAction<T>(T in_KeyEnum) where T : Enum
    {
        int key = Util.Enum32ToInt(in_KeyEnum);
        return ForceChangeAction(key);
    }
}

