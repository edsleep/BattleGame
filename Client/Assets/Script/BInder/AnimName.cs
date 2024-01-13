using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimName
{
    public class Player
    {
        public enum Anim
        {
            Idle, Attack
        }

        static string GetAnimStr(Player.Anim _anim)
        {
            switch (_anim)
            {
                case Anim.Idle: return "Idle";
                case Anim.Attack: return "Attack";
            }
            return "";
        }
    }
}
