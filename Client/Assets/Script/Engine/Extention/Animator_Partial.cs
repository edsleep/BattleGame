using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Animator_Extention
{
    public static bool IsAnimationFinished(this Animator self)
    {
        if(null == self)
            return false;

        return 1 <= self.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static float GetLength(this Animator self)
    {
        if (null == self)
            return 0;

        return self.GetCurrentAnimatorStateInfo(0).length;
    }

    public static float GetCurrentPlayTime(this Animator self)
    {
        if (null == self)
            return 0;

        return self.GetCurrentAnimatorStateInfo(0).normalizedTime * self.GetCurrentAnimatorStateInfo(0).length;
    }

    public static float GetCurrentPlayTimeNormalized(this Animator self)
    {
        if (null == self)
            return 0;

        return self.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
