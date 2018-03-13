namespace Avalonia.Examples.PuzzleFifteen.GameEngine
{
    /// <summary>Represents puzzle movement.</summary>
    public enum PuzzleMovement : byte
    {
        /// <summary>Move an accessible puzzle piece left.</summary>
        Left = 0x00,

        /// <summary>Move an accessible puzzle piece right.</summary>
        Right = 0x01,

        /// <summary>Move an accessible puzzle piece up.</summary>
        Up = 0x02,

        /// <summary>Move an accessible puzzle piece down.</summary>
        Down = 0x03
    }
}