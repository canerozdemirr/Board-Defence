using System;
using System.Collections.Generic;
using Commands.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Commands
{
    [Serializable]
    public class CommandExecutor
    {
        private readonly Queue<ICommand> _commandQueue = new();
        
        private int _totalCommandsExecuted;
        private int _currentCommandIndex;

        private readonly Action OnQueueFinished;

        public CommandExecutor(Action onQueueFinished)
        {
            OnQueueFinished = onQueueFinished;
        }
        
        public void Enqueue(ICommand command)
        {
            _commandQueue.Enqueue(command);
        }

        public async UniTask ExecuteCommands()
        {
            _currentCommandIndex = 0;
            _totalCommandsExecuted = _commandQueue.Count;

            try
            {
                while (_commandQueue.Count > 0)
                {
                    ICommand command = _commandQueue.Dequeue();

                    try
                    {
                        await command.Execute();
                        _currentCommandIndex++;
                    }
                    catch (Exception commandException)
                    {
                        Debug.LogError($"Command execution failed at index {_currentCommandIndex}: {commandException.Message}");
                        Debug.LogException(commandException);
                        _currentCommandIndex++;
                    }
                }

                Debug.Log($"Successfully executed {_currentCommandIndex}/{_totalCommandsExecuted} commands");
            }
            catch (Exception generalException)
            {
                Debug.LogError($"Critical error in command execution pipeline: {generalException.Message}");
                Debug.LogException(generalException);
                _commandQueue.Clear();
                throw;
            }
            finally
            {
                _currentCommandIndex = 0;
                _totalCommandsExecuted = 0;
                OnQueueFinished?.Invoke();
            }
        }
    }
}