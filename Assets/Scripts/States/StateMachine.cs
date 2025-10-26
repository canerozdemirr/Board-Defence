using System;
using System.Collections.Generic;
using States.Interfaces;
using UnityEngine;

namespace States
{
    public class StateMachine<TContext> where TContext : class
    {
        private readonly TContext _context;
        private readonly List<IState<TContext>> _stateSequence;
        
        private IState<TContext> _currentState;
        private int _currentIndex;

        private bool _isPaused;
        private bool _isActive;
        
        public StateMachine(TContext context)
        {
            _context = context;
            _stateSequence = new List<IState<TContext>>();
            _currentIndex = -1;
            _isPaused = false;
            _isActive = false;
        }
        
        public void Start()
        {
            if (_stateSequence.Count == 0)
            {
                Debug.LogWarning("StateMachine: No states to run.");
                return;
            }
            
            _isActive = true;
            _currentIndex = 0;
            _currentState = _stateSequence[_currentIndex];
            _currentState.StateFinished += MoveToNextState;
            _currentState.OnEnter(_context);
        }
        
        public void Update()
        {
            if (_isPaused)
                return;

            _currentState.OnUpdate(_context);
        }
        
        public void MoveToNextState()
        {
            _currentState.OnExit(_context);
            _currentIndex++;
            if (_currentIndex >= _stateSequence.Count)
            {
                Debug.Log("StateMachine: Reached the end of the state sequence.");
                _currentIndex = 0;
                return;
            }

            _currentState = _stateSequence[_currentIndex];
            _currentState.OnEnter(_context);
        }
        
        public void AddState(IState<TContext> state)
        { 
            _stateSequence.Add(state);
        }
        
        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        private void Stop()
        {
            Pause();
            _currentState.OnExit(_context);
            _currentState.StateFinished -= MoveToNextState;
            _isActive = false;
            _currentIndex = -1;
            _currentState = null;
        }
        
        public void Clear()
        {
            Stop();
            _stateSequence.Clear();
            _currentIndex = -1;
        }
    }
}