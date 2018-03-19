using System.Security.Cryptography;

namespace Avalonia.Examples.PuzzleFifteen.GameEngine
{
    internal static class PuzzleFactory
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public static PuzzleState CreateShuffled()
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
    }
}