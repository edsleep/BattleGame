using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* StateMachine
 * 기본적으로 State는 OnUpdateState안에서 교체하는것을 원칙으로 한다.
 */



namespace StateSystem
{
    public class StateMachine<T>
    {
        State<T> m_CurrentState = null;
        Action m_ChangeCallback = null;

        public void SetStateChangeCallback(Action change_callback)
        { 
            m_ChangeCallback = change_callback;
        }

        public void ChangeState(T obj, State<T> newState)
        {
            m_CurrentState?.OnExit(obj);
            m_CurrentState = newState;

            if (null != m_ChangeCallback)
                m_ChangeCallback();

            newState.OnEnter(obj);
        }

        public void UpdateState(T obj)
        {
            if (null == m_CurrentState)
                return;

            State<T> state = m_CurrentState.OnUpdateState(obj);
            if (null != state)
                ChangeState(obj, state);

            if(null != m_CurrentState)
                m_CurrentState.OnUpdate(obj);
        }
    }

    public abstract class State<T>
    {
        internal virtual void OnEnter(T obj) { }

        internal virtual State<T> OnUpdateState(T obj) { return null; }

        internal virtual void OnUpdate(T obj) { }

        internal virtual void OnExit(T obj) { }
    }
}