using System;
using System.Globalization;
using Avalonia.Markup;
using Portable.Xaml.Markup;

namespace Avalonia.Examples.PuzzleFifteen.Framework
{
    internal abstract class ValueConverter<TSource, TTarget> : MarkupExtension, IValueConverter
    {
        protected virtual TTarget Convert(TSource value, object parameter, CultureInfo culture)
        {
            return default;
        }

        protected virtual TSource ConvertBack(TTarget value, object parameter, CultureInfo culture)
        {
            return default;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TSource)value, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TTarget)value, parameter, culture);
        }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}