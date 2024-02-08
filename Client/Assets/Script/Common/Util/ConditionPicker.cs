using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionPicker<T>
{
    List<Tuple<Func<bool>,T>> m_Container = new List<Tuple<Func<bool>, T>> ();
    public void Add(T in_item, Func<bool> in_Codition)
    {
        m_Container.Add(new Tuple<Func<bool>, T>(in_Codition, in_item));
    }

    public bool RemoveItem(T in_item, Func<bool> in_Codition, bool in_IsRemoveAll = false)
    {
        var targetTuple = new Tuple<Func<bool>, T>(in_Codition, in_item);
        bool isSuccess = false;
        foreach (var item in m_Container)
        {
            if (item == targetTuple)
            {
                m_Container.Remove(item);
                isSuccess = true;
                if (false == in_IsRemoveAll)
                    break;
            }
        }
        return isSuccess;
    }

    public T GetItem()
    {
        foreach (var item in m_Container)
        {
            if(item.Item1.Invoke())
                return item.Item2;  
        }
        return default(T);
    }

    public bool TryGetItem(out T out_item)
    {
        out_item = default(T);  
        foreach (var item in m_Container)
        {
            if (item.Item1.Invoke())
            {
                out_item = item.Item2;
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        m_Container.Clear();
    }

}
