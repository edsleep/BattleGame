using UnityEngine;
using StateSystem;
using UnityEngine.AI;
using System;


public partial class Player : MonoBehaviour
{
    static public Player m_self;

    public float m_MoveSpeed = 10;
    public float m_JumpPower = 10;
    public float m_DashDistance = 10;
    public float m_DashDurationTime = 0.2f;
    public GameObject ZanSang;

    bool m_IsCancleTiming = false;

    private bool m_IsGorund = false;
    private Vector2 m_InputDir = Vector2.right;

    public Vector2 m_collider_bottom = Vector2.right;


    //플레이어
    Rigidbody2D m_Rigibody = null;
    Collider2D m_Collider2D = null;
    Animator m_Animator = null;
    SpriteRenderer m_SpriteRenderer = null;

    StateMachine<Player> m_StateMachine = new StateMachine<Player>();
    InputInfo m_inputInfo = new InputInfo();


    public class InputInfo
    {
        public Vector2 inputDir;
        public StateType []stateTypes = new StateType[(int)ActionType.ACTION_MAX];

        public StateType GetState(ActionType type)
        {
            return stateTypes[(int)type];
        }

        public bool GetDown(ActionType type)
        {
            return stateTypes[(int)type] == StateType.DOWN;
        }

        public bool GetPress(ActionType type)
        {
            return stateTypes[(int)type] == StateType.PRESS || stateTypes[(int)type] == StateType.DOWN;
        }

        public bool GetUp(ActionType type)
        {
            return stateTypes[(int)type] == StateType.UP;
        }

        public enum ActionType
        {
            NONE,
            MOVE,
            ATTACK,
            DASH,
            JUMP,
            ACTION_MAX
        }
        public enum StateType
        {
            NONE,
            DOWN,
            PRESS,
            UP
        }
    }

    void Start()
    {
        m_Rigibody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine.SetStateChangeCallback(() => { m_IsCancleTiming = false; });
        m_StateMachine.ChangeState(this, new StateIdle());
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider2D = GetComponent<Collider2D>();
        m_self = this;
    }

    void Update()
    {
        RefreshInputInfo();
        m_InputDir = m_inputInfo.inputDir;
        m_StateMachine.UpdateState(this);


        Vector2 pos = ((Vector2)transform.position + m_collider_bottom);
        RaycastHit2D[] asdf = Physics2D.RaycastAll(pos, Vector2.down, 1);
        Debug.DrawLine(pos, pos  + Vector2.down, Color.red);
        foreach (var dd in asdf)
        {
            if (0 != dd.transform.tag.CompareTo("Ground"))
            {
                Vector3 player_pos = transform.position;
                float a = transform.position.x - dd.transform.position.x;
                if (0 <= a)
                {
                    transform.position = player_pos + new Vector3(0.05f, 0, 0);
                }
                else
                {
                    transform.position = player_pos + new Vector3(-0.05f, 0, 0);
                }
            }
        }


        if (Input.GetKey(KeyCode.Q))
        {
            CreateSpriteTrail();
        }
    }

    void RefreshInputInfo()
    {
        for (int i = 0; i < (int)InputInfo.ActionType.ACTION_MAX; ++i)
            m_inputInfo.stateTypes[i] = InputInfo.StateType.NONE;


        var oldInput = m_inputInfo.inputDir;
        m_inputInfo.inputDir = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;
        if(0 != m_inputInfo.inputDir.x)
        {
            if (0 == oldInput.x)
            {
                m_inputInfo.stateTypes[(int)InputInfo.ActionType.MOVE] = InputInfo.StateType.DOWN;
            }
            else
                m_inputInfo.stateTypes[(int)InputInfo.ActionType.MOVE] = InputInfo.StateType.PRESS;
        }
        else
        {
            if (0 != oldInput.x)
                m_inputInfo.stateTypes[(int)InputInfo.ActionType.MOVE] = InputInfo.StateType.UP;
            else
                m_inputInfo.stateTypes[(int)InputInfo.ActionType.MOVE] = InputInfo.StateType.NONE;
        }

         if (Input.GetKey(KeyCode.Z))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.JUMP] = InputInfo.StateType.PRESS;
        if (Input.GetKey(KeyCode.X))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.DASH] = InputInfo.StateType.PRESS;
        if (Input.GetKey(KeyCode.C))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.ATTACK] = InputInfo.StateType.PRESS;

        if (Input.GetKeyDown(KeyCode.Z))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.JUMP] = InputInfo.StateType.DOWN;
        if (Input.GetKeyDown(KeyCode.X))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.DASH] = InputInfo.StateType.DOWN;
        if (Input.GetKeyDown(KeyCode.C))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.ATTACK] = InputInfo.StateType.DOWN;

        if (Input.GetKeyUp(KeyCode.Z))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.JUMP] = InputInfo.StateType.UP;
        if (Input.GetKeyUp(KeyCode.X))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.DASH] = InputInfo.StateType.UP;
        if (Input.GetKeyUp(KeyCode.C))
            m_inputInfo.stateTypes[(int)InputInfo.ActionType.ATTACK] = InputInfo.StateType.UP;
    }

    void SetXDir(float dir)
    {
        if (dir == 0)
            return;
        m_SpriteRenderer.flipX = 0 > dir;
    }

    bool IsNoneInputDir()
    {
        return m_InputDir.x == 0 && m_InputDir.y == 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            m_IsGorund = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            m_IsGorund = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            m_IsGorund = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            m_IsGorund = false;
    }

    void UpdateTrail()
    {

    }

    void TurnOnSpriteTrail(float frequency)
    {

    }

    void CreateSpriteTrail()
    {
        var inst = Instantiate(ZanSang);
        var st_script = inst.GetComponent<SpriteTrail>();
        st_script.SetSpriteTrail(m_SpriteRenderer);
    }


    void OnAnimationEvent(int i)
    {
        if (i == 0)
            m_IsCancleTiming = false;

        if (i == 1)
            m_IsCancleTiming = true;
    }
}
