using System;
using System.Collections.Generic;
using States.Interfaces;

namespace States
{
    public class StateMachine<TContext> where TContext : class
    {
        private readonly TContext _stateContext;
        private readonly Dictionary<IState<TContext>, List<ITransition<TContext>>> _transitionMap;
        private readonly List<ITransition<TContext>> _anyStateTransitionList;

        private IState<TContext> _currentState;

        private bool _isPaused;
        private bool _isActive;
        public bool IsActive => _isActive;
        public bool IsPaused => _isPaused;

        public StateMachine(TContext stateContext)
        {
            _stateContext = stateContext;
            _transitionMap = new Dictionary<IState<TContext>, List<ITransition<TContext>>>();
            _anyStateTransitionList = new List<ITransition<TContext>>();
            _isPaused = false;
            _isActive = false;
        }

        public void AddTransition(IState<TContext> fromState, IState<TContext> toState, Predicate<TContext> transitionCondition, Action<TContext> onTransitionCallback = null)
        {
            StateTransition<TContext> transition = new(toState, transitionCondition, onTransitionCallback);

            if (!_transitionMap.ContainsKey(fromState))
            {
                _transitionMap[fromState] = new List<ITransition<TContext>>();
            }

            _transitionMap[fromState].Add(transition);
        }

        public void AddAnyStateTransition(IState<TContext> toState, Predicate<TContext> transitionCondition, Action<TContext> onTransitionCallback = null)
        {
            StateTransition<TContext> transition = new(toState, transitionCondition, onTransitionCallback);
            _anyStateTransitionList.Add(transition);
        }

        public void Start(IState<TContext> initialState)
        {
            _isActive = true;
            _currentState = initialState;
            _currentState.StateFinished += OnStateFinished;
            _currentState.OnEnter(_stateContext);
        }

        public void Update()
        {
            if (!_isActive)
                return;

            if (_isPaused)
                return;

            _currentState.OnUpdate(_stateContext);
        }

        public void TryTransitioningToState(IState<TContext> targetState)
        {
            if (!_isActive)
                return;

            if (_isPaused)
                return;

            ITransition<TContext> validTransition = FindTransitionToState(targetState);

            if (validTransition != null)
            {
                TransitionToState(validTransition);
            }
        }

        private ITransition<TContext> FindTransitionToState(IState<TContext> targetState)
        {
            if (_transitionMap.TryGetValue(_currentState, out List<ITransition<TContext>> desiredTransition))
            {
                foreach (ITransition<TContext> transition in desiredTransition)
                {
                    if (transition.TargetState == targetState && transition.CanTransition(_stateContext))
                    {
                        return transition;
                    }
                }
            }

            foreach (ITransition<TContext> transition in _anyStateTransitionList)
            {
                if (transition.TargetState == targetState && transition.CanTransition(_stateContext))
                {
                    return transition;
                }
            }

            return null;
        }

        private void OnStateFinished()
        {
            if (!_isActive)
                return;

            if (_isPaused)
                return;

            ITransition<TContext> validTransition = CheckTransitions(_anyStateTransitionList);
            if (validTransition == null && _transitionMap.ContainsKey(_currentState))
            {
                validTransition = CheckTransitions(_transitionMap[_currentState]);
            }
            if (validTransition != null)
            {
                TransitionToState(validTransition);
            }
        }

        private ITransition<TContext> CheckTransitions(List<ITransition<TContext>> transitionList)
        {
            foreach (ITransition<TContext> transition in transitionList)
            {
                if (transition.TargetState == _currentState)
                    continue;

                if (transition.CanTransition(_stateContext))
                {
                    return transition;
                }
            }

            return null;
        }

        private void TransitionToState(ITransition<TContext> transition)
        {
            transition.OnTransition(_stateContext);
            TransitionToState(transition.TargetState);
        }

        private void TransitionToState(IState<TContext> targetState)
        {
            Pause();

            _currentState.StateFinished -= OnStateFinished;
            _currentState.OnExit(_stateContext);

            _currentState = targetState;
            _currentState.StateFinished += OnStateFinished;
            _currentState.OnEnter(_stateContext);

            Resume();
        }

        private void Pause()
        {
            _isPaused = true;
        }

        private void Resume()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            if (!_isActive)
                return;

            _isPaused = false;
            if (_currentState != null)
            {
                _currentState.StateFinished -= OnStateFinished;
                _currentState.OnExit(_stateContext);
            }
            _isActive = false;
            _currentState = null;
        }

        public void Clear()
        {
            Stop();
            _transitionMap.Clear();
            _anyStateTransitionList.Clear();
        }
    }
}