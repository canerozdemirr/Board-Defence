using System;
using States.Interfaces;

namespace States
{
    public abstract class BaseState<TContext> : IState<TContext> where TContext : class
    {
        public event Action StateFinished;

        public virtual void OnEnter(TContext context)
        {
            
        }

        public virtual void OnUpdate(TContext context)
        {
            
        }
        
        public virtual void OnExit(TContext context)
        {
            
        }
        
        protected void FinishState()
        {
            StateFinished?.Invoke();
        }
    }
}