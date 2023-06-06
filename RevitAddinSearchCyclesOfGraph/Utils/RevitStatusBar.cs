using System.Runtime.InteropServices;

namespace RevitAddinSearchCyclesOfGraph.Utils;

public class RevitStatusBar
{
    [DllImport("user32.dll",
        SetLastError = true)]
    static extern IntPtr FindWindowEx(
        IntPtr hwndParent,
        IntPtr hwndChildAfter,
        string lpszClass,
        string lpszWindow);

    [DllImport("user32.dll",
        SetLastError = true,
        CharSet = CharSet.Auto)]
    static extern int SetWindowText(
        IntPtr hWnd,
        string lpString);

    private IntPtr m_StatusBar = IntPtr.Zero;

    protected RevitStatusBar()
    {
        // Find the status bar, so we can add messages.
        IntPtr revitHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        if (revitHandle != IntPtr.Zero)
            m_StatusBar = FindWindowEx(revitHandle, IntPtr.Zero, "msctls_statusbar32", "");
    }

    /// <summary>
    /// Set the value of the status bar, if there is a valid handle to it.
    /// </summary>
    /// <param name="msg">The message.</param>
    public void Set(string msg)
    {
        if (m_StatusBar != IntPtr.Zero)
        {
            SetWindowText(m_StatusBar, msg);
        }
    }

    /// <summary>
    /// Create a new RevitStatusBar.
    /// </summary>
    /// <returns>The RevitStatusBar.</returns>
    public static RevitStatusBar Create()
    {
        return new RevitStatusBar();
    }
}