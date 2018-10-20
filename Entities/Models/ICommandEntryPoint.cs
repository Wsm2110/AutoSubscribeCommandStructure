using Entities.Contracts;
using System;

namespace TestRunnerArchitecture
{
    public interface ICommandEntryPoint
    {
        ICommandEntryPoint ParseArguments<T>();
        ICommandEntryPoint Execute();    
        void OnError(Action<ConsoleError, string> p);
        string[] GetModules();
        ICommandEntryPoint FindCommand(string v);
    }
}