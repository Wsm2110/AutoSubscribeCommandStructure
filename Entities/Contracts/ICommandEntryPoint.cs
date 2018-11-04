using Entities.Contracts;
using System;

namespace TestRunnerArchitecture
{
    public interface ICommandEntryPoint
    {
        IEventAggregator EventAggregator { get; set; }
        ICommandEntryPoint ParseArguments();
        ICommandEntryPoint Execute();    
        void OnError(Action<ConsoleError, string> p);
        string[] GetModules();
        ICommandEntryPoint FindCommand(string v);
    }
}