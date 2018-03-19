using System;
using System.Collections.Generic;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Examples.PuzzleFifteen.GameEngine;
using Avalonia.Interactivity;

namespace Avalonia.Examples.PuzzleFifteen.Controls
{
    internal sealed class PuzzleControl : TemplatedControl
    {
        public static readonly StyledProperty<PuzzleState> StateProperty = AvaloniaProperty.Register<PuzzleControl, PuzzleState>(nameof(State));

        private IReadOnlyList<IControl> _pieceControls;

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            var canvas = e.NameScope.Get<Canvas>("PART_Canvas");
            var pieceControlEasing = LinearEasing.For<double>();
            var pieceControlPropertyTransitionsX = new PropertyTransition(Canvas.LeftProperty, TimeSpan.FromSeconds(0.075), pieceControlEasing);
            var pieceControlPropertyTransitionsY = new PropertyTransition(Canvas.TopProperty, TimeSpan.FromSeconds(0.075), pieceControlEasing);

            foreach (var piece in (PuzzlePiece[])Enum.GetValues(typeof(PuzzlePiece)))
            {
                if (piece != PuzzlePiece.Space)
                {
                    var pieceControl = new PuzzlePieceControl();

                    pieceControl.DataContext = piece;

                    // Setting default values for Canvas properties since property transition doesn't work with NaN values

                    pieceControl.SetValue(Canvas.LeftProperty, 0.0);
                    pieceControl.SetValue(Canvas.TopProperty, 0.0);
                    pieceControl.PropertyTransitions.Add(pieceControlPropertyTransitionsX);
                    pieceControl.PropertyTransitions.Add(pieceControlPropertyTransitionsY);
                    pieceControl.Tapped += OnPieceControlTapped;

                    canvas.Children.Add(pieceControl);
                }
            }

            _pieceControls = canvas.Children;
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);

            ArrangePieceControls();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == StateProperty)
            {
                ArrangePieceControls();
            }
        }

        private void ArrangePieceControls()
        {
            if (_pieceControls != null)
            {
                var viewportSize = Math.Min(Width - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);
                var viewportLeft = (Width - viewportSize) / 2;
                var viewportTop = (Height - viewportSize) / 2;
                var pieceSize = viewportSize / 4;

                for (var i = 0; i < _pieceControls.Count; i++)
                {
                    var pieceControl = (Control)_pieceControls[i];
                    var pieceSlot = State[(PuzzlePiece)pieceControl.DataContext];

                    pieceControl.Width = pieceSize;
                    pieceControl.Height = pieceSize;
                    pieceControl.SetValue(Canvas.LeftProperty, viewportLeft + pieceSlot.X * pieceSize);
                    pieceControl.SetValue(Canvas.TopProperty, viewportTop + pieceSlot.Y * pieceSize);
                }
            }
        }

        private void OnPieceControlTapped(object sender, RoutedEventArgs e)
        {
            var pieceSlot = State[(PuzzlePiece)((IControl)sender).DataContext];
            var spaceSlot = State[PuzzlePiece.Space];
            var difference = (X: pieceSlot.X - spaceSlot.X, Y: pieceSlot.Y - spaceSlot.Y);

            if (difference.Equals((+1, +0)))
            {
                State = State.Apply(PuzzleMovement.Left);
            }
            if (difference.Equals((-1, +0)))
            {
                State = State.Apply(PuzzleMovement.Right);
            }
            if (difference.Equals((+0, +1)))
            {
                State = State.Apply(PuzzleMovement.Up);
            }
            if (difference.Equals((+0, -1)))
            {
                State = State.Apply(PuzzleMovement.Down);
            }
        }

        public PuzzleState State
        {
            get => GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
    }
}