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

            for (var i = 0; i < matrix.Length - 1; i++)
            {
                matrix[i] = (byte)(i + 1);
            }

            return new PuzzleState(matrix);
        }

        private static (int X, int Y) FindPiece(byte[] matrix, PuzzlePiece piece)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                if (matrix[i] == (byte)piece)
                {
                    return (i % 4, i / 4);
                }
            }

            throw new InvalidOperationException(Strings.GetString("puzzle.piece_not_found"));
        }

        private static void Apply(byte[] matrix, PuzzleMovement movement, (int X, int Y) spaceSlot)
        {
            switch (movement)
            {
                case PuzzleMovement.Left:
                    {
                        if (spaceSlot.X != 3)
                        {
                            matrix[spaceSlot.Y * 4 + spaceSlot.X + 0] = matrix[spaceSlot.Y * 4 + spaceSlot.X + 1];
                            matrix[spaceSlot.Y * 4 + spaceSlot.X + 1] = (byte)PuzzlePiece.Space;
                        }
                    }
                    break;
                case PuzzleMovement.Right:
                    {
                        if (spaceSlot.X != 0)
                        {
                            matrix[spaceSlot.Y * 4 + spaceSlot.X + 0] = matrix[spaceSlot.Y * 4 + spaceSlot.X - 1];
                            matrix[spaceSlot.Y * 4 + spaceSlot.X - 1] = (byte)PuzzlePiece.Space;
                        }
                    }
                    break;
                case PuzzleMovement.Up:
                    {
                        if (spaceSlot.Y != 3)
                        {
                            matrix[(spaceSlot.Y + 0) * 4 + spaceSlot.X] = matrix[(spaceSlot.Y + 1) * 4 + spaceSlot.X];
                            matrix[(spaceSlot.Y + 1) * 4 + spaceSlot.X] = (byte)PuzzlePiece.Space;
                        }
                    }
                    break;
                case PuzzleMovement.Down:
                    {
                        if (spaceSlot.Y != 0)
                        {
                            matrix[(spaceSlot.Y + 0) * 4 + spaceSlot.X] = matrix[(spaceSlot.Y - 1) * 4 + spaceSlot.X];
                            matrix[(spaceSlot.Y - 1) * 4 + spaceSlot.X] = (byte)PuzzlePiece.Space;
                        }
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
                Apply(matrix, movements[i], FindPiece(matrix, PuzzlePiece.Space));
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