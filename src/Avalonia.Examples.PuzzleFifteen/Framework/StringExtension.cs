using System;
using Avalonia.Examples.PuzzleFifteen.Resources;
using Portable.Xaml.Markup;

namespace Avalonia.Examples.PuzzleFifteen.Framework
{
    internal sealed class StringExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Name != null ? Strings.GetString(Name.ToLowerInvariant()) : null;
        }

        public string Name
        {
            get;
            set;
        }
    }
}