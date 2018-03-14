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

            Assert.Equal((0, 0), state[PuzzlePiece.Piece01]);
            Assert.Equal((1, 0), state[PuzzlePiece.Piece02]);
            Assert.Equal((2, 0), state[PuzzlePiece.Piece03]);
            Assert.Equal((3, 0), state[PuzzlePiece.Piece04]);
            Assert.Equal((0, 1), state[PuzzlePiece.Piece05]);
            Assert.Equal((1, 1), state[PuzzlePiece.Piece06]);
            Assert.Equal((2, 1), state[PuzzlePiece.Piece07]);
            Assert.Equal((3, 1), state[PuzzlePiece.Piece08]);
            Assert.Equal((0, 2), state[PuzzlePiece.Piece09]);
            Assert.Equal((1, 2), state[PuzzlePiece.Piece10]);
            Assert.Equal((2, 2), state[PuzzlePiece.Piece11]);
            Assert.Equal((3, 2), state[PuzzlePiece.Piece12]);
            Assert.Equal((0, 3), state[PuzzlePiece.Piece13]);
            Assert.Equal((1, 3), state[PuzzlePiece.Piece14]);
            Assert.Equal((2, 3), state[PuzzlePiece.Piece15]);
            Assert.Equal((3, 3), state[PuzzlePiece.Space]);
        }

        [Fact]
        public void Movements()
        {
            var state = PuzzleState.Completed;

            state = state.Move(PuzzleMovement.Right);

            Assert.Equal((3, 3), state[PuzzlePiece.Piece15]);
            Assert.Equal((2, 3), state[PuzzlePiece.Space]);

            state = state.Move(PuzzleMovement.Down);

            Assert.Equal((2, 3), state[PuzzlePiece.Piece11]);
            Assert.Equal((2, 2), state[PuzzlePiece.Space]);

            state = state.Move(PuzzleMovement.Left);

            Assert.Equal((2, 2), state[PuzzlePiece.Piece12]);
            Assert.Equal((3, 2), state[PuzzlePiece.Space]);

            state = state.Move(PuzzleMovement.Up);

            Assert.Equal((3, 2), state[PuzzlePiece.Piece15]);
            Assert.Equal((3, 3), state[PuzzlePiece.Space]);
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