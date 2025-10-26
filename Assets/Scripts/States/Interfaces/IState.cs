using System;

namespace States.Interfaces
{
    public interface IState<in TContext> where TContext : class
    {
        event Action StateFinished;
        void OnEnter(TContext context);
        void OnUpdate(TContext context);
        void OnExit(TContext context);
    }
}