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
        private readonly IBindableCommand _moveLeftCommand;
        private readonly IBindableCommand _moveRightCommand;
        private readonly IBindableCommand _moveUpCommand;
        private readonly IBindableCommand _moveDownCommand;

        private PuzzleState _puzzleState = CreateShuffled();
        private int _puzzleSteps;
        private bool _puzzleCompleted;

        public ShellWindowViewModel()
        {
            _shuffleCommand = new BindableCommand(ShuffleCommandAction);
            _moveLeftCommand = new BindableCommand(MoveLeftCommandAction, MoveCommandPredicate);
            _moveRightCommand = new BindableCommand(MoveRightCommandAction, MoveCommandPredicate);
            _moveUpCommand = new BindableCommand(MoveUpCommandAction, MoveCommandPredicate);
            _moveDownCommand = new BindableCommand(MoveDownCommandAction, MoveCommandPredicate);
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

        private bool MoveCommandPredicate(object parameter)
        {
            return !_puzzleCompleted;
        }

        private void MoveLeftCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Apply(PuzzleMovement.Left);
        }

        private void MoveRightCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Apply(PuzzleMovement.Right);
        }

        private void MoveUpCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Apply(PuzzleMovement.Up);
        }

        private void MoveDownCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Apply(PuzzleMovement.Down);
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

        public ICommand MoveLeftCommand
        {
            get => _moveLeftCommand;
        }

        public ICommand MoveRightCommand
        {
            get => _moveRightCommand;
        }

        public ICommand MoveUpCommand
        {
            get => _moveUpCommand;
        }

        public ICommand MoveDownCommand
        {
            get => _moveDownCommand;
        }

        public string Title
        {
            get => Strings.GetString("app.window.title");
        }

        public string Legend
        {
            get => Strings.GetString("puzzle.legend");
        }

        public string CompletionMessage
        {
            get => Strings.GetString("puzzle.completion_message");
        }
    }
}