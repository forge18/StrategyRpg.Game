using System;
using System.Collections.Generic;

namespace Infrastructure.Commands
{
    public class CommandService : ICommandService
    {
        private Lazy<Dictionary<string, ICommandQueue>> _commandQueues =
            new Lazy<Dictionary<string, ICommandQueue>>(() => new Dictionary<string, ICommandQueue>());
        private Lazy<Dictionary<string, IUndoRedoCommandQueue>> _undoRedoCommandQueues =
            new Lazy<Dictionary<string, IUndoRedoCommandQueue>>(() => new Dictionary<string, IUndoRedoCommandQueue>());

        public void ProcessQueues()
        {
            foreach (KeyValuePair<string, ICommandQueue> commandQueue in _commandQueues.Value)
            {
                commandQueue.Value.Process();
            }

            foreach (KeyValuePair<string, IUndoRedoCommandQueue> undoRedoCommandQueue in _undoRedoCommandQueues.Value)
            {
                undoRedoCommandQueue.Value.Process();
            }
        }

        public ICommandQueue CreateQueue(string queueName)
        {
            if (_commandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.CreateQueue: Queue with name {queueName} already exists.");
            }

            ICommandQueue queue = new CommandQueue();
            _commandQueues.Value.Add(queueName, queue);
            return queue;
        }

        public IUndoRedoCommandQueue CreateUndoRedoQueue(string queueName)
        {
            if (_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.CreateUndoRedoQueue: Queue with name {queueName} already exists.");
            }

            IUndoRedoCommandQueue queue = new UndoRedoCommandQueue();
            _undoRedoCommandQueues.Value.Add(queueName, queue);
            return queue;
        }

        public ICommandQueue GetQueue(string queueName)
        {
            if (!_commandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.GetQueue: Queue with name {queueName} does not exist.");
            }

            return _commandQueues.Value[queueName];
        }

        public IUndoRedoCommandQueue GetUndoRedoQueue(string queueName)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.GetUndoRedoQueue: Queue with name {queueName} does not exist.");
            }

            return _undoRedoCommandQueues.Value[queueName];
        }

        public void RemoveQueue(string queueName)
        {
            if (!_commandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.RemoveQueue: Queue with name {queueName} does not exist.");
            }

            _commandQueues.Value.Remove(queueName);
        }

        public void RemoveUndoRedoQueue(string queueName)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.RemoveUndoRedoQueue: Queue with name {queueName} does not exist.");
            }

            _undoRedoCommandQueues.Value.Remove(queueName);
        }

        public bool HasQueue(string queueName)
        {
            return _commandQueues.Value.ContainsKey(queueName);
        }

        public bool HasUndoRedoQueue(string queueName)
        {
            return _undoRedoCommandQueues.Value.ContainsKey(queueName);
        }

        public void AddCommandToQueue(string queueName, ICommand command)
        {
            if (!_commandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.AddCommandToQueue: Queue with name {queueName} does not exist.");
            }

            _commandQueues.Value[queueName].Store(command);
        }

        public void AddCommandToUndoRedoQueue(string queueName, IUndoRedoCommand command)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.AddCommandToUndoRedoQueue: Queue with name {queueName} does not exist.");
            }

            _undoRedoCommandQueues.Value[queueName].Store(command);
        }

        public void GetNextFromQueue(string queueName)
        {
            if (!_commandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.GetNextFromQueue: Queue with name {queueName} does not exist.");
            }

            _commandQueues.Value[queueName].Process();
        }

        public void GetNextFromUndoRedoQueue(string queueName)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.GetNextFromUndoRedoQueue: Queue with name {queueName} does not exist.");
            }

            _undoRedoCommandQueues.Value[queueName].Process();
        }

        public void Undo(string queueName)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.Undo: Queue with name {queueName} does not exist.");
            }

            _undoRedoCommandQueues.Value[queueName].Undo();
        }

        public void Redo(string queueName)
        {
            if (!_undoRedoCommandQueues.Value.ContainsKey(queueName))
            {
                throw new Exception($"CommandService.Redo: Queue with name {queueName} does not exist.");
            }

            _undoRedoCommandQueues.Value[queueName].Process();
        }
    }
}