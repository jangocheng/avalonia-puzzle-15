﻿using System;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Examples.PuzzleFifteen.GameEngine;
using Avalonia.Interactivity;

namespace Avalonia.Examples.PuzzleFifteen.Controls
{
    internal sealed class PuzzleControl : TemplatedControl
    {
        public static readonly StyledProperty<PuzzleState> StateProperty =
            AvaloniaProperty.Register<PuzzleControl, PuzzleState>(nameof(State));

        private AvaloniaList<IControl> _pieceControls;

        static PuzzleControl()
        {
            StateProperty.Changed.AddClassHandler<PuzzleControl>(c => c.OnStatePropertyChanged);
        }

        private void OnStatePropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            ArrangePieceControls();
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            var canvas = e.NameScope.Find("PART_Canvas") as Canvas;

            if (canvas == null)
            {
                return;
            }

            foreach (var piece in Enum.GetValues(typeof(PuzzlePiece)))
            {
                if (object.Equals(piece, PuzzlePiece.Space))
                {
                    continue;
                }

                var pieceControl = new PuzzlePieceControl
                {
                    DataContext = piece
                };

                pieceControl.Tapped += OnPieceControlTapped;

                canvas.Children.Add(pieceControl);
            }

            _pieceControls = canvas.Children;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);

            ArrangePieceControls();

            return finalSize;
        }

        private void ArrangePieceControls()
        {
            if (_pieceControls == null)
            {
                return;
            }

            var viewportSize = Math.Min(DesiredSize.Width - Padding.Left - Padding.Right, DesiredSize.Height - Padding.Top - Padding.Bottom);
            var viewportLeft = (DesiredSize.Width - viewportSize) / 2;
            var viewportTop = (DesiredSize.Height - viewportSize) / 2;
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

        private void OnPieceControlTapped(object sender, RoutedEventArgs e)
        {
            var pieceSlot = State[(PuzzlePiece)((Control)sender).DataContext];
            var spaceSlot = State[PuzzlePiece.Space];

            if (pieceSlot.Y == spaceSlot.Y)
            {
                if (pieceSlot.X == spaceSlot.X + 1)
                {
                    State = State.Move(PuzzleMovement.Left);
                }
                else if (pieceSlot.X == spaceSlot.X - 1)
                {
                    State = State.Move(PuzzleMovement.Right);
                }
            }
            else if (pieceSlot.X == spaceSlot.X)
            {
                if (pieceSlot.Y == spaceSlot.Y + 1)
                {
                    State = State.Move(PuzzleMovement.Up);
                }
                else if (pieceSlot.Y == spaceSlot.Y - 1)
                {
                    State = State.Move(PuzzleMovement.Down);
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