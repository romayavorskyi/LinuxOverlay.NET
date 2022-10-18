using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                List<Rectangle> rects = new List<Rectangle>
                {
                    new Rectangle(new Point(200,200), new Size(50, 50)),
                    new Rectangle(new Point(300,300), new Size(50, 50)),
                    new Rectangle(new Point(400,400), new Size(50, 50)),
                    new Rectangle(new Point(500,500), new Size(50, 50)),
                    new Rectangle(new Point(600,600), new Size(50, 50)),

                };
                desktop.MainWindow = new MainWindow(rects);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}