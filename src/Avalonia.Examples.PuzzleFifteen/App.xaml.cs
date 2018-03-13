using Avalonia.Markup.Xaml;

namespace Avalonia.Examples.PuzzleFifteen
{
    internal sealed class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}