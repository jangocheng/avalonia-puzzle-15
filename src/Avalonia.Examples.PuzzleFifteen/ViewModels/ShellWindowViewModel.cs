using System.Security.Cryptography;
using System.Windows.Input;
using Avalonia.Examples.PuzzleFifteen.GameEngine;
using Avalonia.Examples.PuzzleFifteen.Resources;
using Chimera.UI.ComponentModel;

namespace Avalonia.Examples.PuzzleFifteen.ViewModels
{
    internal sealed class ShellWindowViewModel : BindableObject
    {
        private readonly IBindableCommand _shuffleCommand;
        private readonly IBindableCommand _moveLeftCommand;
        private readonly IBindableCommand _moveRightCommand;
        private readonly IBindableCommand _moveUpCommand;
        private readonly IBindableCommand _moveDownCommand;

        private PuzzleState _puzzleState = Shuffle();
        private bool _puzzleCompleted;

        public ShellWindowViewModel()
        {
            _shuffleCommand = new BindableCommand(ShuffleCommandAction);
            _moveLeftCommand = new BindableCommand(MoveLeftCommandAction, MoveCommandPredicate);
            _moveRightCommand = new BindableCommand(MoveRightCommandAction, MoveCommandPredicate);
            _moveUpCommand = new BindableCommand(MoveUpCommandAction, MoveCommandPredicate);
            _moveDownCommand = new BindableCommand(MoveDownCommandAction, MoveCommandPredicate);
        }

        private static PuzzleState Shuffle()
        {
            var bytes = new byte[1000];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var movements = new PuzzleMovement[bytes.Length];

            for (var i = 0; i < bytes.Length; i++)
            {
                movements[i] = (PuzzleMovement)(bytes[i] / 64);
            }

            return PuzzleState.Completed.Move(movements);
        }

        private void ShuffleCommandAction(object parameter)
        {
            PuzzleState = Shuffle();

            _puzzleCompleted = false;

            RaisePropertyChanged(nameof(IsPuzzleCompleted));
        }

        private bool MoveCommandPredicate(object parameter)
        {
            return !_puzzleCompleted;
        }

        private void MoveLeftCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Move(PuzzleMovement.Left);
            CheckPuzzleState();
        }

        private void MoveRightCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Move(PuzzleMovement.Right);
            CheckPuzzleState();
        }

        private void MoveUpCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Move(PuzzleMovement.Up);
            CheckPuzzleState();
        }

        private void MoveDownCommandAction(object parameter)
        {
            PuzzleState = PuzzleState.Move(PuzzleMovement.Down);
            CheckPuzzleState();
        }

        private void CheckPuzzleState()
        {
            if (_puzzleState == PuzzleState.Completed)
            {
                _puzzleCompleted = true;

                RaisePropertyChanged(nameof(IsPuzzleCompleted));
            }
        }

        public PuzzleState PuzzleState
        {
            get => GetValue(ref _puzzleState);
            set => SetValue(ref _puzzleState, value);
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

        public bool IsPuzzleCompleted
        {
            get => GetValue(ref _puzzleCompleted);
        }

        public string Title
        {
            get => Strings.GetString("app.window.title");
        }

        public string MovementsLegend
        {
            get => Strings.GetString("puzzle.legend.movements");
        }

        public string ShuffleLegend
        {
            get => Strings.GetString("puzzle.legend.shuffle");
        }

        public string CompletionMessage
        {
            get => Strings.GetString("puzzle.message.completed");
        }
    }
}