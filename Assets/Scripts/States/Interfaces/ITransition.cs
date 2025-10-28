using System;

namespace States.Interfaces
{
    public interface ITransition<in TContext> where TContext : class
    {
        IState<TContext> TargetState { get; }
        bool CanTransition(TContext context);
        void OnTransition(TContext context);
    }
}
