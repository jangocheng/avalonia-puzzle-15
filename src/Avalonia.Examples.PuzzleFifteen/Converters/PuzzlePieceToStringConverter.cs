using System.Globalization;
using Avalonia.Examples.PuzzleFifteen.Framework;
using Avalonia.Examples.PuzzleFifteen.GameEngine;

namespace Avalonia.Examples.PuzzleFifteen.Converters
{
    internal sealed class PuzzlePieceToStringConverter : ValueConverter<PuzzlePiece, string>
    {
        protected override string Convert(PuzzlePiece value, object parameter, CultureInfo culture)
        {
            return ((byte)value).ToString("00", CultureInfo.InvariantCulture);
        }
    }
}