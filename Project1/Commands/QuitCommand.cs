using Project1.Interfaces;
using System;

namespace Project1.Commands
{
    public sealed class QuitCommand : ICommand
    {
        private readonly Action _quit;
        public QuitCommand(Action quit) => _quit = quit;
        public void Execute() => _quit?.Invoke();
    }
}

