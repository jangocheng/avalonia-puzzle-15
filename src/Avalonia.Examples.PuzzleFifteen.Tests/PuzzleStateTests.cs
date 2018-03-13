using Avalonia.Examples.PuzzleFifteen.GameEngine;
using Xunit;

namespace Avalonia.Examples.PuzzleFifteen.Tests
{
    public sealed class PuzzleStateTests
    {
        [Fact]
        public void Completed()
        {
            var state = PuzzleState.Completed;

            Assert.Equal(PuzzlePiece.Piece01, state[0, 0]);
            Assert.Equal(PuzzlePiece.Piece02, state[1, 0]);
            Assert.Equal(PuzzlePiece.Piece03, state[2, 0]);
            Assert.Equal(PuzzlePiece.Piece04, state[3, 0]);
            Assert.Equal(PuzzlePiece.Piece05, state[0, 1]);
            Assert.Equal(PuzzlePiece.Piece06, state[1, 1]);
            Assert.Equal(PuzzlePiece.Piece07, state[2, 1]);
            Assert.Equal(PuzzlePiece.Piece08, state[3, 1]);
            Assert.Equal(PuzzlePiece.Piece09, state[0, 2]);
            Assert.Equal(PuzzlePiece.Piece10, state[1, 2]);
            Assert.Equal(PuzzlePiece.Piece11, state[2, 2]);
            Assert.Equal(PuzzlePiece.Piece12, state[3, 2]);
            Assert.Equal(PuzzlePiece.Piece13, state[0, 3]);
            Assert.Equal(PuzzlePiece.Piece14, state[1, 3]);
            Assert.Equal(PuzzlePiece.Piece15, state[2, 3]);
            Assert.Equal(PuzzlePiece.Space, state[3, 3]);
        }

        [Fact]
        public void Movements()
        {
            var state = PuzzleState.Completed;

            state = state.Move(PuzzleMovement.Right);

            Assert.Equal(PuzzlePiece.Piece15, state[3, 3]);
            Assert.Equal(PuzzlePiece.Space, state[2, 3]);

            state = state.Move(PuzzleMovement.Down);

            Assert.Equal(PuzzlePiece.Piece11, state[2, 3]);
            Assert.Equal(PuzzlePiece.Space, state[2, 2]);

            state = state.Move(PuzzleMovement.Left);

            Assert.Equal(PuzzlePiece.Piece12, state[2, 2]);
            Assert.Equal(PuzzlePiece.Space, state[3, 2]);

            state = state.Move(PuzzleMovement.Up);

            Assert.Equal(PuzzlePiece.Piece15, state[3, 2]);
            Assert.Equal(PuzzlePiece.Space, state[3, 3]);
        }

        [Fact]
        public void ItemByPiece()
        {
            var state = PuzzleState.Completed;

            Assert.Equal((3, 3), state[PuzzlePiece.Space]);
        }

        [Fact]
        public void OperatorEquality()
        {
            var state1 = default(PuzzleState);
            var state2 = PuzzleState.Completed;

            Assert.False(state1 == state2);
        }

        [Fact]
        public void OperatorInequality()
        {
            var state1 = default(PuzzleState);
            var state2 = PuzzleState.Completed;

            Assert.True(state1 != state2);
        }
    }
}