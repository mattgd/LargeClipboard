/*
 * Author: mattgd
 * Date: 4/21/2015
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LargeClipboard
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form {
		
		[DllImport("User32.dll")]
  		protected static extern int SetClipboardViewer(int hWndNewViewer);
  		
  		[DllImport("User32.dll", CharSet=CharSet.Auto)]
  		public static extern bool ChangeClipboardChain(IntPtr hWndRemove,
  		                                               IntPtr hWndNewNext);
  		
  		[DllImport("user32.dll", CharSet=CharSet.Auto)]
 		public static extern int SendMessage(IntPtr hwnd, int wMsg, 
  		                                     IntPtr wParam,
  		                                     IntPtr lParam);
  		
  		IntPtr nextClipboardViewer;
  		GlobalHotkey ghk;
  		
  		// DLL libraries used to manage hotkeys
		[DllImport("user32.dll")] 
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		
		const int ALT = 0x0001;
        const int CTRL = 0x0002;
        const int WM_HOTKEY_MSG_ID = 0x0312;
		
		public MainForm(Boolean open) {
			InitializeComponent();
			nextClipboardViewer = (IntPtr)SetClipboardViewer((int) this.Handle);
			ghk = new GlobalHotkey(CTRL + ALT, Keys.V, this);
		}
		
  		protected override void WndProc(ref Message m) {
		  	// defined in winuser.h
		  	const int WM_DRAWCLIPBOARD = 0x308;
		  	const int WM_CHANGECBCHAIN = 0x030D;
		 	
		  	switch(m.Msg) {
		    	case WM_DRAWCLIPBOARD:
		      		DisplayClipboardData();
		      		SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
		    		break;
		 
		    	case WM_CHANGECBCHAIN:
		    		if (m.WParam == nextClipboardViewer) {
		    			nextClipboardViewer = m.LParam;
		    		} else {
		    			SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
		    		}
		    		break;
		    	default:
		      		base.WndProc(ref m);
		    		break;
		  	}
		  	if (m.Msg == WM_HOTKEY_MSG_ID) {
				HandleHotkey();
			}  
            base.WndProc(ref m);
		}
        
        void HandleHotkey() {
        	MessageBox.Show("Hotkey pressed!");
        }
		
  		void DisplayClipboardData() {
  			try {
  				IDataObject iData = new DataObject();
	  			iData = Clipboard.GetDataObject();
	  			
	  			RichTextBox clipboard;
	  			if (clipboard1.Text == "") {
	  				clipboard = clipboard1;
	  			} else {
	  				clipboard = clipboard2;
	  			}
	  			
	  			if (iData.GetDataPresent(DataFormats.Rtf)) {
	  				clipboard.Text = (string) iData.GetData(DataFormats.Rtf);
	  			} else if (iData.GetDataPresent(DataFormats.Text)) {
	  				clipboard.Text = (string) iData.GetData(DataFormats.Text);
	  			} else {
	  				clipboard.Text = "";
	  			}
  			} catch(Exception e) {
  				MessageBox.Show(e.ToString());
  			}
  		}
	}
}