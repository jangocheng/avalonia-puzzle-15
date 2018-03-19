using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Input;
using Avalonia.Examples.PuzzleFifteen.GameEngine;
using Avalonia.Examples.PuzzleFifteen.Resources;
using Chimera.UI.ComponentModel;

namespace Avalonia.Examples.PuzzleFifteen.ViewModels
{
    internal sealed class ShellWindowViewModel : BindableObject
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private readonly IBindableCommand _shuffleCommand;
        private readonly IBindableCommand _moveCommand;

        private PuzzleState _puzzleState = CreateShuffled();
        private int _puzzleSteps;
        private bool _puzzleCompleted;

        public ShellWindowViewModel()
        {
            _shuffleCommand = new BindableCommand(ShuffleCommandAction);
            _moveCommand = new BindableCommand(MoveCommandAction, MoveCommandPredicate);
        }

        private static PuzzleState CreateShuffled()
        {
            var bytes = new byte[1000];

            _rng.GetBytes(bytes);

            var movements = new PuzzleMovement[bytes.Length];

            for (var i = 0; i < bytes.Length; i++)
            {
                movements[i] = (PuzzleMovement)(bytes[i] / 64);
            }

            return PuzzleState.Completed.Apply(movements);
        }

        private void ShuffleCommandAction(object parameter)
        {
            _puzzleState = CreateShuffled();
            _puzzleSteps = 0;
            _puzzleCompleted = false;

            RaisePropertyChanged(nameof(PuzzleState));
            RaisePropertyChanged(nameof(PuzzleStepsInfo));
            RaisePropertyChanged(nameof(IsPuzzleCompleted));
        }

        private void MoveCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Apply(Enum.Parse<PuzzleMovement>((string)parameter));
        }

        private bool MoveCommandPredicate(object parameter)
        {
            return !_puzzleCompleted;
        }

        private void OnPuzzleStateUpdated()
        {
            _puzzleSteps++;
            _puzzleCompleted = _puzzleState == PuzzleState.Completed;

            RaisePropertyChanged(nameof(PuzzleStepsInfo));

            if (_puzzleCompleted)
            {
                RaisePropertyChanged(nameof(IsPuzzleCompleted));
            }
        }

        public PuzzleState PuzzleState
        {
            get => GetValue(ref _puzzleState);
            set => SetValue(ref _puzzleState, value, OnPuzzleStateUpdated);
        }

        public string PuzzleStepsInfo
        {
            get => string.Format(CultureInfo.InvariantCulture, Strings.GetString("puzzle.moves_template"), _puzzleSteps);
        }

        public bool IsPuzzleCompleted
        {
            get => _puzzleCompleted;
        }

        public ICommand ShuffleCommand
        {
            get => _shuffleCommand;
        }

        public ICommand MoveCommand
        {
            get => _moveCommand;
        }
    }
}