using System;
using States.Interfaces;

namespace States
{
    public class StateTransition<TContext> : ITransition<TContext> where TContext : class
    {
        private readonly Predicate<TContext> _condition;
        private readonly Action<TContext> _onTransitionCallback;

        public IState<TContext> TargetState { get; }

        public StateTransition(IState<TContext> targetState, Predicate<TContext> condition, Action<TContext> onTransitionCallback = null)
        {
            TargetState = targetState;
            _condition = condition;
            _onTransitionCallback = onTransitionCallback;
        }

        public bool CanTransition(TContext context)
        {
            return _condition(context);
        }

        public void OnTransition(TContext context)
        {
            _onTransitionCallback?.Invoke(context);
        }
    }
}
