using UnityEngine;
using StateSystem;
using EasingLerp;
using Unity.VisualScripting;

public partial class TestBoss
{
    public abstract class CommonStateOnGround : State<TestBoss>
    {
        internal override State<TestBoss> OnUpdateState(TestBoss obj)
        {
            return null;
        }
    }

    public class StateIdle : CommonStateOnGround
    {
        internal override void OnEnter(TestBoss obj)
        {

        }

        internal override State<TestBoss> OnUpdateState(TestBoss obj)
        {
            return base.OnUpdateState(obj);
        }
    }

    public class StateIdleToRun : CommonStateOnGround
    {
        public StateIdleToRun() { }

        public StateIdleToRun(float anim_start_time)
        {
            m_anim_start_time = anim_start_time;
        }

        private float m_anim_start_time = 0;

        internal override void OnEnter(TestBoss obj)
        {

        }

        internal override State<TestBoss> OnUpdateState(TestBoss obj)
        {
            return base.OnUpdateState(obj);
        }


        internal override void OnUpdate(TestBoss obj)
        {

        }
    }

    public class StateMove : CommonStateOnGround
    {
        internal override void OnEnter(TestBoss obj)
        {

        }

        internal override State<TestBoss> OnUpdateState(TestBoss obj)
        {

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(TestBoss obj)
        {

        }
    }
}