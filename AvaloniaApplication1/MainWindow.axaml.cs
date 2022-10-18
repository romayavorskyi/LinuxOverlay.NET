using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Avalonia.Controls;
using Brushes = Avalonia.Media.Brushes;
using Point = Avalonia.Point;

namespace AvaloniaApplication1
{
    public partial class MainWindow : OverlayWindow
    {
        private List<Rectangle> _rects = new List<Rectangle>();
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public MainWindow(List<Rectangle> rects)
        {
            InitializeComponent();
            _rects = rects;
            this.Activated += OnActivated;
        }

        private void OnActivated(object? sender, EventArgs e)
        {
            // need to be done after window position is initialized, Activated looks like the only possible choice
            foreach (var rect in _rects)
            {
                var visualRect = new Avalonia.Controls.Shapes.Rectangle();
                visualRect.Height = rect.Height;
                visualRect.Width = rect.Width;
                visualRect.Fill = Brushes.Red;
                visualRect.Opacity = 0.3;
                // compensate rects absolute position
                visualRect.SetValue(Canvas.TopProperty, rect.Top - Position.Y);
                visualRect.SetValue(Canvas.LeftProperty, rect.Left - Position.X);
                RectContainer.Children.Add(visualRect);
            }        
        }

        protected override void OnMouseCoordinatesUpdated(int x, int y)
        {
            base.OnMouseCoordinatesUpdated(x, y);
            // visual rects use the 
            var relativePos = new Point(x - Position.X, y - Position.Y);
            if (RectContainer?.Children != null)
            {
                foreach (var visualRect in RectContainer?.Children)
                {
                    if (visualRect.Bounds.Contains(relativePos))
                    {
                        Debug.WriteLine("Hit!");
                    }
                }
            }
        }
    }
}