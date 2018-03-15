using System;
using System.Globalization;
using Avalonia.Examples.PuzzleFifteen.Resources;
using Avalonia.Examples.PuzzleFifteen.ViewModels;
using Avalonia.Examples.PuzzleFifteen.Views;

namespace Avalonia.Examples.PuzzleFifteen
{
    public static class Program
    {
        public static void Main()
        {
            Console.Title = string.Format(CultureInfo.InvariantCulture, Strings.GetString("app.console.title_template"), Strings.GetString("app.window.title"));
            AppBuilder.Configure<App>().UsePlatformDetect().Start<ShellWindowView>(() => new ShellWindowViewModel());
        }
    }
}