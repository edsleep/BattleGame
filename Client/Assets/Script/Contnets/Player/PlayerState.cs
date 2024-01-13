using UnityEngine;
using StateSystem;
using EasingLerp;
using Unity.VisualScripting;

public partial class Player : MonoBehaviour
{

    public class DashInfo
    {
        public DashInfo(float time, float duration, float distance, EaseType type)
        {
            this.time = time;
            this.duration = duration;
            this.distance = distance;
            this.type = type;
        }
        public float time;
        public float duration;
        public float distance;
        public EaseType type;
    }

    static internal void Dash(Player obj, ref float time, float duration, float distance, EaseType type = EaseType.Lerp)
    {
        float oldtime = time;
        time+= Time.deltaTime;

        float prev_ratio = (oldtime / duration);
        float next_ratio = (time / duration);
        float moveDistance = Easing.Ease(type, 0, distance, next_ratio) - Easing.Ease(type, 0, distance, prev_ratio);

        var pos = obj.transform.position;
        pos.x += moveDistance * Mathf.Sign(obj.m_InputDir.x);
        obj.transform.position = pos;
    }

    static internal void Dash(Player obj, DashInfo info)
    {
        float oldtime = info.time;
        info.time += Time.deltaTime;


        float prev_ratio = (oldtime / info.duration);
        float next_ratio = (info.time / info.duration);
        float moveDistance = Easing.Ease(info.type, 0, info.distance, next_ratio) - Easing.Ease(info.type, 0, info.distance, prev_ratio);

        var pos = obj.transform.position;
        pos.x += moveDistance * Mathf.Sign(obj.m_InputDir.x);
        obj.transform.position = pos;
    }

    public abstract class CommonStateOnGround : State<Player>
    {
        internal override State<Player> OnUpdateState(Player obj)
        {
            if (obj.m_inputInfo.GetPress(InputInfo.ActionType.JUMP))
                return new StateAir(true);
            if (obj.m_inputInfo.GetDown(InputInfo.ActionType.ATTACK))
                return new StateAttack_Ground_Combo();
            if (obj.m_inputInfo.GetDown(InputInfo.ActionType.DASH) && false == obj.IsNoneInputDir())
                return new StateDash();

            return null;
        }
    }

    public class StateIdle : CommonStateOnGround
    {
        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.Play("Idle");
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (obj.m_inputInfo.GetPress(InputInfo.ActionType.MOVE))
                return new StateIdleToRun();

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

        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.PlayInFixedTime("IdleToRun", 0, m_anim_start_time);
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (false == obj.m_inputInfo.GetPress(InputInfo.ActionType.MOVE))
                return new StateRunToIdle(obj.m_Animator.GetLength() - obj.m_Animator.GetCurrentPlayTime());
            if (obj.m_Animator.IsAnimationFinished())
                return new StateMove();

            return base.OnUpdateState(obj);
        }


        internal override void OnUpdate(Player obj)
        {
            Vector3 pos = Vector3.zero;
            pos.x = obj.m_MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            obj.m_Rigibody.AddForce(pos);

            //obj.transform.position += pos;
            obj.SetXDir(pos.x);
        }
    }

    public class StateMove : CommonStateOnGround
    {
        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.Play("Run");
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (false == obj.m_inputInfo.GetPress(InputInfo.ActionType.MOVE))
                return new StateRunToIdle();

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(Player obj)
        {
            Vector3 pos = Vector3.zero;
            pos.x = obj.m_MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            obj.m_Rigibody.AddForce(pos);

            //obj.transform.position += pos;
            obj.SetXDir(pos.x);
        }
    }

    public class StateRunToIdle : State<Player>
    {
        public StateRunToIdle() { }
        public StateRunToIdle(float anim_start_time)
        {
            m_anim_start_time = anim_start_time;
        }

        private float m_anim_start_time = 0;

        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.PlayInFixedTime("RunToIdle", 0, m_anim_start_time);
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (obj.m_inputInfo.GetPress(InputInfo.ActionType.MOVE))
                return new StateIdleToRun((1 - obj.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * obj.m_Animator.GetCurrentAnimatorStateInfo(0).length);
            if (obj.m_Animator.IsAnimationFinished())
                return new StateIdle();

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(Player obj)
        {
            Vector3 pos = Vector3.zero;
            pos.x = obj.m_MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            obj.m_Rigibody.AddForce(pos);

            //obj.transform.position += pos;
            obj.SetXDir(pos.x);
        }
    }

    public class StateAir : State<Player>
    {
        bool m_isJump = false;

        Vector2 m_start_velo = Vector2.zero;    

        public StateAir() { }
        public StateAir(bool isJump = false)
        {
            m_isJump = isJump;
        }

        public StateAir(bool isJump , Vector2 in_velo)
        {
            m_isJump = isJump;
            m_start_velo = in_velo; 
        }

        internal override void OnEnter(Player obj)
        {
            var velo = obj.m_Rigibody.velocity;
            velo.y = 0;
            if (m_isJump)
            {
                velo.y = obj.m_JumpPower;
            }
            obj.m_Rigibody.velocity = velo + m_start_velo;

            if (0 <= obj.m_Rigibody.velocity.y)
                obj.m_Animator.Play("AirUp");
            else
                obj.m_Animator.Play("AirDown");
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (obj.m_inputInfo.GetDown(InputInfo.ActionType.DASH) && false == obj.IsNoneInputDir())
                return new StateDash();

            if (obj.m_IsGorund && 0 >= obj.m_Rigibody.velocity.y)
                return new StateIdle();

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(Player obj)
        {
            if (0 <= obj.m_Rigibody.velocity.y)
                obj.m_Animator.Play("AirUp");
            else
                obj.m_Animator.Play("AirDown");

            Vector3 pos = Vector3.zero;
            pos.x = obj.m_MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            obj.transform.position += pos;
            obj.SetXDir(pos.x);
        }
    }


    public class StateDash : State<Player>
    {
        private float m_DashTime = 0;
        private Vector2 m_DashDir = Vector2.zero;

        private int m_trail_counter = 0;
        private const float TRAIL_FREQUENCY = 0.015f;

        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.Play("Dash");
            m_DashDir = obj.m_InputDir;
            obj.m_Rigibody.velocity = new Vector2(0, 0);
            obj.m_Rigibody.gravityScale = 0;
        }

        internal override State<Player> OnUpdateState(Player obj)
        {
            if (obj.m_DashDurationTime <= m_DashTime)
            {
                if (obj.m_IsGorund)
                {
                    if (0 < obj.m_InputDir.x * m_DashDir.x)
                        return new StateMove();
                    else
                        return new StateRunToIdle();
                }
                else
                    return new StateAir();
            }
            else
            {
                if (obj.m_inputInfo.GetPress(InputInfo.ActionType.JUMP))
                    return new StateAir(true, m_DashDir * 20);
            }

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(Player obj)
        {
            UpdateTrail(obj);
            float oldDashTime = m_DashTime;
            float maxDashTime = obj.m_DashDurationTime;
            m_DashTime += Time.deltaTime;
            m_DashTime = Mathf.Clamp(m_DashTime, 0, maxDashTime + 0.01f);

            float prevratio = oldDashTime / maxDashTime;
            float ratio = m_DashTime / maxDashTime;
            float delta = ratio - prevratio;

            var tempVec = (m_DashDir * obj.m_DashDistance * delta);
            Vector3 tran = new Vector3(tempVec.x, tempVec.y);

            obj.transform.position += tran;
        }

        internal override void OnExit(Player obj)
        {
            base.OnExit(obj);
            obj.m_Rigibody.gravityScale = 5;
        }

        void UpdateTrail(Player obj)
        {
            if (m_trail_counter * TRAIL_FREQUENCY <= m_DashTime)
            {
                obj.CreateSpriteTrail();
                ++m_trail_counter;
            }
        }
    }


    public class StateAttack_Ground_Combo : State<Player>
    {
        public StateAttack_Ground_Combo(int ComboCount = 1)
        {
            m_ComboCount = ComboCount;
        }

        InputInfo.ActionType m_PreInputAction = InputInfo.ActionType.NONE;
        int m_ComboCount = 1;

        internal override void OnEnter(Player obj)
        {
            obj.m_Animator.PlayInFixedTime($"AttackGroundCombo{m_ComboCount}", 0, 0);
        }


        internal override State<Player> OnUpdateState(Player obj)
        {
            if (InputInfo.ActionType.NONE == m_PreInputAction)
            {
                if (obj.m_inputInfo.GetDown(InputInfo.ActionType.ATTACK) && m_ComboCount < 3)
                    m_PreInputAction = InputInfo.ActionType.ATTACK;
                else if (obj.m_inputInfo.GetDown(InputInfo.ActionType.DASH))
                    m_PreInputAction = InputInfo.ActionType.DASH;
                else if (obj.m_inputInfo.GetDown(InputInfo.ActionType.MOVE))
                    m_PreInputAction = InputInfo.ActionType.MOVE;
                else if (obj.m_inputInfo.GetDown(InputInfo.ActionType.JUMP))
                    m_PreInputAction = InputInfo.ActionType.JUMP;

                if (obj.m_IsCancleTiming && InputInfo.ActionType.NONE != m_PreInputAction)
                {
                    obj.CreateSpriteTrail();
                    return ChangeState_Internal(obj);
                }
            }

            if (false == obj.m_Animator.IsAnimationFinished())
                return null;

            if (InputInfo.ActionType.NONE == m_PreInputAction)
                obj.m_StateMachine.ChangeState(obj, new StateIdle());
            else
                return ChangeState_Internal(obj);

            return base.OnUpdateState(obj);
        }

        internal override void OnUpdate(Player obj)
        {

        }

        State<Player> ChangeState_Internal(Player obj)
        {
            switch (m_PreInputAction)
            {
                case InputInfo.ActionType.MOVE:
                    return new StateMove();
                case InputInfo.ActionType.ATTACK:
                    return new StateAttack_Ground_Combo(m_ComboCount +1);
                case InputInfo.ActionType.DASH:
                    return new StateDash();
                case InputInfo.ActionType.JUMP:
                    return new StateAir();
            }
            return null;
        }
    }
}