using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.Examples.PuzzleFifteen.Views
{
    internal sealed class ShellWindowView : Window
    {
        public ShellWindowView()
        {
            Initialize();
        }

        private void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}