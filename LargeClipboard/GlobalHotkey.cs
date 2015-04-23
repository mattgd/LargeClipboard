/*
 * Created by SharpDevelop.
 * User: mdzwonczyk
 * Date: 4/23/2015
 * Time: 12:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LargeClipboard
{
	/// <summary>
	/// Description of GlobalHotkey.
	/// </summary>
	public class GlobalHotkey
	{
		private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id;
 
        public GlobalHotkey(int modifier, Keys key, Form form) {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }
 
        public bool Register() {
            return RegisterHotKey(hWnd, id, modifier, key);
        }
 
        public bool Unregiser() {
            return UnregisterHotKey(hWnd, id);
        }
 
        public override int GetHashCode() {
            return modifier ^ key ^ hWnd.ToInt32();
        }
 
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
 
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	}
}
