using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaApplication1;

public class OverlayWindow : Window
{
    const string libX11 = "libX11.so.6";
    const string libXFixes = "libXfixes.so.3";

    [DllImport(libX11)]
    public static extern IntPtr XOpenDisplay(IntPtr display);

    [DllImport(libXFixes)]
    public static extern IntPtr XFixesCreateRegion(IntPtr dpy, IntPtr ignore, int count);
        
    [DllImport(libXFixes)]
    public static extern IntPtr XFixesSetWindowShapeRegion (IntPtr dpy, IntPtr win, int kind, int x, int y, IntPtr region);

    [DllImport(libXFixes)]
    public static extern void XFixesDestroyRegion(IntPtr dpy, IntPtr region);
        
    [DllImport(libX11)]
    public static extern int XSync(IntPtr display, bool discard);
    
    
    [DllImport(libX11)]
    public static extern bool XQueryPointer(IntPtr display, IntPtr window, ref IntPtr window_return, ref IntPtr child_return,
        ref int root_x, ref int root_y, ref int win_x, ref int win_y, ref uint mask);
    
    [DllImport(libX11)]
    public static extern IntPtr XRootWindow(IntPtr display, int screenNumber);
    
    private static IntPtr Display = XOpenDisplay(IntPtr.Zero);

    public OverlayWindow()
    {
        Task.Run(PollMouseCoordinates);

        Debug.WriteLine($"Primary display height: {Screens.Primary.Bounds.Height}, width: {Screens.Primary.Bounds.Width}");

        
        TransparencyLevelHint = WindowTransparencyLevel.Transparent;
        Background = Brushes.Transparent;
        Topmost = true;
        SystemDecorations = SystemDecorations.None;
            
        var r = XFixesCreateRegion(Display, IntPtr.Zero, 0);
        XFixesSetWindowShapeRegion(Display, PlatformImpl.Handle.Handle, 2, 0, 0, r);
        XFixesDestroyRegion(Display, r);
        XSync(Display, true);
    }

    protected virtual void OnMouseCoordinatesUpdated(int x, int y)
    {
        
    }
    
    // We need this method to calculate the hover - click-through breaks the avalonia events
    private void PollMouseCoordinates()
    {
        IntPtr windowReturn = IntPtr.Zero, childReturn = IntPtr.Zero;
        int rootX = 0, rootY = 0, winX = 0, winY = 0;
        uint mask = 0;
        var rootWindow = XRootWindow(Display, 0);
        
        while (true)
        {
            XQueryPointer(Display, rootWindow, ref windowReturn, ref childReturn, ref rootX, ref rootY,
                ref winX, ref winY, ref mask);
            Debug.WriteLine($"Mouse cursor position: {rootX}, {rootY}");
            Debug.WriteLine($"Window position: {Position.X}, {Position.Y}");
            OnMouseCoordinatesUpdated(rootX, rootY);
            Thread.Sleep(2000);
        }
    }
}