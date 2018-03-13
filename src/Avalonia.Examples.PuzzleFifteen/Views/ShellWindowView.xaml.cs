using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.Examples.PuzzleFifteen.Views
{
    internal sealed class ShellWindowView : Window
    {
        public ShellWindowView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}