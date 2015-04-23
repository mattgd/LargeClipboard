/*
 * Author: mattgd
 * Date: 4/21/2015
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LargeClipboard
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form {
		public MainForm(Boolean open) {
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			if (!open) new ClipboardMonitor();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public static void setClipboardText(String text) {
			if (clipboard1.Text == null) {
				clipboard1.Text = text;
			} else {
				clipboard2.Text = text;
			}
		}
	}
}