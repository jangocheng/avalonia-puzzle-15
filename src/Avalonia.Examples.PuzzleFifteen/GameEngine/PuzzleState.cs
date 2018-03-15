using System;
using Avalonia.Examples.PuzzleFifteen.Resources;

namespace Avalonia.Examples.PuzzleFifteen.GameEngine
{
    /// <summary>Represents puzzle state.</summary>
    public readonly struct PuzzleState
    {
        private static readonly PuzzleState _completed = CreateCompleted();

        private readonly byte[] _matrix;

        private PuzzleState(byte[] matrix)
        {
            _matrix = matrix;
        }

        private static PuzzleState CreateCompleted()
        {
            var matrix = new byte[0x10];

            for (var i = 0; i < 0x0F; i++)
            {
                matrix[i] = (byte)(i + 1);
            }

            return new PuzzleState(matrix);
        }

        private static (int X, int Y) FindPiece(byte[] matrix, PuzzlePiece piece)
        {
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (matrix[y * 4 + x] == (byte)piece)
                    {
                        return (x, y);
                    }
                }
            }

            throw new InvalidOperationException(Strings.GetString("puzzle.piece_not_found"));
        }

        private void Apply(byte[] matrix, PuzzleMovement movement, int spaceX, int spaceY)
        {
            switch (movement)
            {
                case PuzzleMovement.Left:
                    {
                        if (spaceX == 3)
                        {
                            return;
                        }

                        matrix[spaceY * 4 + spaceX + 0] = matrix[spaceY * 4 + spaceX + 1];
                        matrix[spaceY * 4 + spaceX + 1] = (byte)PuzzlePiece.Space;
                    }
                    break;
                case PuzzleMovement.Right:
                    {
                        if (spaceX == 0)
                        {
                            return;
                        }

                        matrix[spaceY * 4 + spaceX + 0] = matrix[spaceY * 4 + spaceX - 1];
                        matrix[spaceY * 4 + spaceX - 1] = (byte)PuzzlePiece.Space;
                    }
                    break;
                case PuzzleMovement.Up:
                    {
                        if (spaceY == 3)
                        {
                            return;
                        }

                        matrix[(spaceY + 0) * 4 + spaceX] = matrix[(spaceY + 1) * 4 + spaceX];
                        matrix[(spaceY + 1) * 4 + spaceX] = (byte)PuzzlePiece.Space;
                    }
                    break;
                case PuzzleMovement.Down:
                    {
                        if (spaceY == 0)
                        {
                            return;
                        }

                        matrix[(spaceY + 0) * 4 + spaceX] = matrix[(spaceY - 1) * 4 + spaceX];
                        matrix[(spaceY - 1) * 4 + spaceX] = (byte)PuzzlePiece.Space;
                    }
                    break;
            }
        }

        private bool Equals(PuzzleState other)
        {
            if ((_matrix == null) && (other._matrix == null))
            {
                return true;
            }
            if ((_matrix == null) && (other._matrix != null) ||
                (_matrix != null) && (other._matrix == null))
            {
                return false;
            }
            for (var i = 0; i < _matrix.Length; i++)
            {
                if (_matrix[i] != other._matrix[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Gets the position of the specified piece.</summary>
        /// <param name="piece">The puzzle piece to get the position for.</param>
        /// <returns>The tuple with horizontal and vertical indexes of the specified piece.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="piece" /> has invalid value.</exception>
        public (int X, int Y) this[PuzzlePiece piece]
        {
            get => FindPiece(_matrix, piece);
        }

        /// <summary>Apply the specified movements to the current state.</summary>
        /// <param name="movements">The puzzle movements to execute.</param>
        /// <returns>The puzzle state after movements.</returns>
        public PuzzleState Apply(params PuzzleMovement[] movements)
        {
            var matrix = new byte[_matrix.Length];

            _matrix.CopyTo(matrix, 0);

            for (var i = 0; i < movements.Length; i++)
            {
                var (spaceX, spaceY) = FindPiece(matrix, PuzzlePiece.Space);

                Apply(matrix, movements[i], spaceX, spaceY);
            }

            return new PuzzleState(matrix);
        }

        /// <summary>Indicates whether the current <see cref="PuzzleState" /> is equal to the specified object.</summary>
        /// <param name="obj">The object to compare with the current <see cref="PuzzleState" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="PuzzleState" /> is equal to the specified object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is PuzzleState other) && Equals(other);
        }

        /// <summary>Returns the hash code for the current <see cref="PuzzleState" />.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = (int)2166136261;

                for (var i = 0; i < _matrix.Length; i++)
                {
                    result = (result * 16777619) ^ ((i + 1) * _matrix[i].GetHashCode());
                }

                return result;
            }
        }

        /// <summary>Indicates whether the left <see cref="PuzzleState" /> is equal to the right <see cref="PuzzleState" />.</summary>
        /// <param name="obj1">The left <see cref="PuzzleState" /> operand.</param>
        /// <param name="obj2">The right <see cref="PuzzleState" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="PuzzleState" /> is equal to the right <see cref="PuzzleState" />; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(PuzzleState obj1, PuzzleState obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>Indicates whether the left <see cref="PuzzleState" /> is not equal to the right <see cref="PuzzleState" />.</summary>
        /// <param name="obj1">The left <see cref="PuzzleState" /> operand.</param>
        /// <param name="obj2">The right <see cref="PuzzleState" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="PuzzleState" /> is not equal to the right <see cref="PuzzleState" />; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(PuzzleState obj1, PuzzleState obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <summary>Gets the completed puzzle state.</summary>
        public static PuzzleState Completed
        {
            get => _completed;
        }
    }
}