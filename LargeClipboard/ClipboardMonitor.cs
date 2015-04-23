﻿/*
 * Created by SharpDevelop.
 * User: mdzwonczyk
 * Date: 4/21/2015
 * Time: 11:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace LargeClipboard
{
	/// <summary>
	/// Description of ClipboardAssist.
	/// </summary>
	// Must inherit Control, not Component, in order to have Handle
	[DefaultEvent("ClipboardChanged")]
	public partial class ClipboardMonitor : Control  {
    	IntPtr nextClipboardViewer;
    	MainForm mainForm = new MainForm(true);
		
    	public ClipboardMonitor() {
        	BackColor = Color.Red;
        	Visible = false;
       	 	nextClipboardViewer = (IntPtr) SetClipboardViewer((int) this.Handle);
    	}

	    /// <summary>
	    /// Clipboard contents changed.
	    /// </summary>
	    public event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;
	
	    protected override void Dispose(bool disposing) {
	        ChangeClipboardChain(this.Handle, nextClipboardViewer);
	    }
	
	    [DllImport("User32.dll")]
	    protected static extern int SetClipboardViewer(int hWndNewViewer);
	
	    [DllImport("User32.dll", CharSet = CharSet.Auto)]
	    public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
	
	    [DllImport("user32.dll", CharSet = CharSet.Auto)]
	    public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
	
	    protected override void WndProc(ref System.Windows.Forms.Message m) {
	        // defined in winuser.h
	        const int WM_DRAWCLIPBOARD = 0x308;
	        const int WM_CHANGECBCHAIN = 0x030D;
	
	        switch (m.Msg) {
	            case WM_DRAWCLIPBOARD:
	                OnClipboardChanged();
	                SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
	                break;
	
	            case WM_CHANGECBCHAIN:
	                if (m.WParam == nextClipboardViewer)
	                    nextClipboardViewer = m.LParam;
	                else
	                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
	                break;
	
	            default:
	                base.WndProc(ref m);
	                break;
	        }
	    }
	
	    void OnClipboardChanged() {
	        try {
	            IDataObject iData = Clipboard.GetDataObject();
	            if (ClipboardChanged != null) {
	                ClipboardChanged(this, new ClipboardChangedEventArgs(iData));
	            }
	            
	            string copy = Clipboard.GetText();
	            Clipboard.SetText(copy);
	            
	            MainForm.setClipboardText(copy);
	        } catch (Exception e) {
	           	MessageBox.Show(e.ToString());
	        }
	    }
	}
	
	public class ClipboardChangedEventArgs : EventArgs {
	    public readonly IDataObject DataObject;
	
	    public ClipboardChangedEventArgs(IDataObject dataObject) {
	        DataObject = dataObject;
	    }
	}
}
