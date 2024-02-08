using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유틸함수들 다 여따가 때려박자
public static class Util
{
    struct Shell<T> where T : Enum
    {
        public int IntValue;
        public T Enum;
    }

    public static int Enum32ToInt<T>(T e) where T : Enum
    {
        Shell<T> s;
        s.Enum = e;

        unsafe
        {
            int* pi = &s.IntValue;
            pi += 1;
            return *pi;
        }
    }

    public static T IntToEnums32<T>(int value) where T : Enum
    {
        var s = new Shell<T>();

        unsafe
        {
            int* pi = &s.IntValue;
            pi += 1;
            *pi = value;
        }

        return s.Enum;
    }


}



