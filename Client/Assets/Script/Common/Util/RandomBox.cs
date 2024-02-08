using System;
using System.Collections.Generic;

// 확률을 지정하여 여러개를 넣은 뒤 하나를 뽑고싶을때 사용
public class RandomBox<T>
{
    static Random RANDOM = new Random();

    long m_RateSum = 0;
    List<Tuple<long, T>> m_Objects = new List<Tuple<long, T>>();

    public bool AddItem(T in_Target,long in_Rate)
    {
        m_Objects.Add(new Tuple<long, T>(in_Rate, in_Target));
        m_RateSum += in_Rate;
        return true;
    }

    public bool RemoveItem(T in_Target, long in_Rate, bool in_IsRemoveAll = false)
    {
        var targetTuple = new Tuple<long, T>(in_Rate, in_Target);
        bool isSuccess = false;
        foreach (var item in m_Objects) 
        {
            if (item == targetTuple)
            {
                m_RateSum -= in_Rate;
                m_Objects.Remove(item);
                isSuccess = true;
                if (false == in_IsRemoveAll)
                    break;
            }
        }
        return isSuccess;
    }

    public void Clear()
    { 
        m_Objects.Clear();
        m_RateSum = 0;
    }

    public bool TryGetRandomItem(out T out_item)
    {
        long randomNum = RANDOM.Next(m_RateSum);
        long sum = 0;
        out_item = default(T);  

        foreach (var item in m_Objects)
        {
            sum += item.Item1;
            if (randomNum < sum)
            {
                out_item = item.Item2;
                return true;
            }
        }
        return false;
    }

    public T GetRandomItem()
    {
        long randomNum = RANDOM.Next(m_RateSum);
        long sum = 0;

        foreach (var item in m_Objects)
        {
            sum += item.Item1;
            if (randomNum < sum)
                return item.Item2;
        }
        return default;
    }

}
