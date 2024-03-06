﻿using Spacebattle.Interfaces;

namespace Spacebattle
{
    public class SoftStopMainCommand : ICommand
    {
        private readonly CancellationTokenSource _cts;
        private readonly MainCommand _mainCommand;

        public SoftStopMainCommand(MainCommand mainCommand, CancellationTokenSource cts)
        {
            _mainCommand = mainCommand;
            _cts = cts;
        }

        public void Execute()
        {
            while (true)
            {
                if (_mainCommand.CommandCountInExecutor == 0)
                {
                    _cts.Cancel();
                    break;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
