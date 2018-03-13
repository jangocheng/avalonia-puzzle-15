using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Examples.PuzzleFifteen.GameEngine;

namespace Avalonia.Examples.PuzzleFifteen.Controls
{
    internal sealed class PuzzleControl : TemplatedControl
    {
        public static readonly StyledProperty<PuzzleState> StateProperty =
            AvaloniaProperty.Register<PuzzleControl, PuzzleState>(nameof(State));

        private Canvas _canvas;

        static PuzzleControl()
        {
            StateProperty.Changed.AddClassHandler<PuzzleControl>(c => c.OnStatePropertyChanged);
        }

        private void OnStatePropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            ArrangePieces();
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _canvas = e.NameScope.Find("PART_Canvas") as Canvas;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);

            ArrangePieces();

            return finalSize;
        }

        private void ArrangePieces()
        {
            if (_canvas == null)
            {
                return;
            }

            _canvas.Children.Clear();

            var viewportSize = Math.Max(Math.Min(DesiredSize.Width - Padding.Left - Padding.Right, DesiredSize.Height - Padding.Top - Padding.Bottom), 0);

            if (viewportSize == 0)
            {
                return;
            }

            var viewportLeft = (DesiredSize.Width - viewportSize) / 2;
            var viewportTop = (DesiredSize.Height - viewportSize) / 2;
            var pieceSize = viewportSize / 4;

            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    var piece = State[x, y];

                    if (piece == PuzzlePiece.Space)
                    {
                        continue;
                    }

                    var item = new PuzzlePieceControl
                    {
                        Width = pieceSize,
                        Height = pieceSize,
                        DataContext = piece
                    };

                    _canvas.Children.Add(item);

                    item.SetValue(Canvas.LeftProperty, viewportLeft + x * pieceSize);
                    item.SetValue(Canvas.TopProperty, viewportTop + y * pieceSize);
                }
            }
        }

        public PuzzleState State
        {
            get => GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
    }
}