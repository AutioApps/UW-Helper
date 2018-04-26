using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UWHelper
{
    public partial class Form1 : Form
    {

        /*
         * Set up winAPI invokes
         */
        public static UIntPtr SetWindowLongPtr(IntPtr hWnd, Int32 nIndex, UIntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            else
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern UIntPtr SetWindowLongPtr32(IntPtr hWnd, Int32 nIndex, UIntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        private static extern UIntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, UIntPtr dwNewLong);

        public static UIntPtr GetWindowLongPtr(IntPtr hWnd, Int32 nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLongPtr32(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr64(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        private static extern UIntPtr GetWindowLongPtr32(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
        private static extern UIntPtr GetWindowLongPtr64(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        // Set up constants
        public static int GWL_EXSTYLE = -20;
        public static int GWL_STYLE = -16;
        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_MINIMIZE = 0x20000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_DISABLED = 0x08000000;
        public const uint WS_CLIPSIBLINGS = 0x04000000;
        public const uint WS_CLIPCHILDREN = 0x02000000;
        public const uint WS_MAXIMIZE = 0x01000000;
        public const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const uint WS_BORDER = 0x00800000;
        public const uint WS_DLGFRAME = 0x00400000;
        public const uint WS_VSCROLL = 0x00200000;
        public const uint WS_HSCROLL = 0x00100000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_GROUP = 0x00020000;
        public const uint WS_TABSTOP = 0x00010000;

        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_EX_APPWINDOW = 0x00040000;
        public const uint WS_TILED = WS_OVERLAPPED;
        public const uint WS_ICONIC = WS_MINIMIZE;
        public const uint WS_SIZEBOX = WS_THICKFRAME;
        public const uint WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

        public const uint WS_OVERLAPPEDWINDOW =
            (WS_OVERLAPPED |
              WS_CAPTION |
              WS_SYSMENU |
              WS_THICKFRAME |
              WS_MINIMIZEBOX |
              WS_MAXIMIZEBOX);

        public const uint WS_POPUPWINDOW =
            (WS_POPUP |
              WS_BORDER |
              WS_SYSMENU);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /*
         * Start normal code
         */

        // Set up some global variables
        public IntPtr handle;
        public int sizeX, sizeY, posX, posY;
        public bool topMost = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        // handle integer texbox
        string txtsXOld = "1920";
        private void txtsX_TextChanged(object sender, EventArgs e)
        {
            long a;
            if (txtsX.Text == "")
            {
                txtsX.Text = "0";
            }
            if (!long.TryParse(txtsX.Text, out a))
            {
                // If not int clear textbox text or Undo() last operation
                txtsX.Text = txtsXOld;
                txtsX.SelectionStart = txtsX.Text.Length;
                txtsX.SelectionLength = 0;
            }
            txtsXOld = txtsX.Text;
        }

        string txtsYOld = "1080";
        private void txtsY_TextChanged(object sender, EventArgs e)
        {
            long a;
            if (txtsY.Text == "")
            {
                txtsY.Text = "0";
            }
            if (!long.TryParse(txtsY.Text, out a))
            {
                // If not int clear textbox text or Undo() last operation
                txtsY.Text = txtsYOld;
                txtsY.SelectionStart = txtsY.Text.Length;
                txtsY.SelectionLength = 0;
            }
            txtsYOld = txtsY.Text;
        }

        string txtpXOld = "0";
        private void txtpX_TextChanged(object sender, EventArgs e)
        {
            long a;
            if (txtpX.Text == "")
            {
                txtpX.Text = "0";
            }
            if (!long.TryParse(txtpX.Text, out a))
            {
                // If not int clear textbox text or Undo() last operation
                txtpX.Text = txtpXOld;
                txtpX.SelectionStart = txtpX.Text.Length;
                txtpX.SelectionLength = 0;
            }
            txtpXOld = txtpX.Text;
        }

        string txtpYOld = "0";
        private void txtpY_TextChanged(object sender, EventArgs e)
        {
            long a;
            if (txtpY.Text == "")
            {
                txtpY.Text = "0";
            }
            if (!long.TryParse(txtpY.Text, out a))
            {
                // If not int clear textbox text or Undo() last operation
                txtpY.Text = txtpYOld;
                txtpY.SelectionStart = txtpY.Text.Length;
                txtpY.SelectionLength = 0;
            }
            txtpYOld = txtpY.Text;
        }

        // Refresh process list
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstProcesses.Items.Clear();
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    lstProcesses.Items.Add(p.MainWindowTitle + " --- " + p.Id);
                }
            }
        }

        private void btnBringToFront_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) { return; }
            string SelectedItem = lstProcesses.GetItemText(lstProcesses.SelectedItem);
            string toBeSearched = " --- ";
            string pId = SelectedItem.Substring(SelectedItem.IndexOf(toBeSearched) + toBeSearched.Length);
            int pIdInt = 0;
            Int32.TryParse(pId, out pIdInt);
            Process proc = Process.GetProcessById(pIdInt);
            handle = proc.MainWindowHandle;
            // convert textboxes to int
            try
            {
                Int32.TryParse(txtpX.Text, out posX);
                Int32.TryParse(txtpY.Text, out posY);
                Int32.TryParse(txtsX.Text, out sizeX);
                Int32.TryParse(txtsY.Text, out sizeY);
            } catch
            {
                MessageBox.Show("Please only use numbers in size X, size Y, pos X and pos Y.");
            }
            
            if (chkBorderless.Checked)
            {

                UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                style = new UIntPtr(style.ToUInt64() & ~(UInt64)WS_OVERLAPPEDWINDOW);
                style = new UIntPtr(style.ToUInt64() | (UInt64)WS_POPUPWINDOW);
                SetWindowLongPtr(handle, GWL_STYLE, style);
                SetWindowPos(handle, -2, posX, posY, sizeX, sizeY, 0x0010);
                tmrSetTopMost.Enabled = true;
            }
            else
            {
                UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                style = new UIntPtr(style.ToUInt64() & ~(UInt64)WS_POPUPWINDOW);
                style = new UIntPtr(style.ToUInt64() | (UInt64)WS_OVERLAPPEDWINDOW);
                SetWindowLongPtr(handle, GWL_STYLE, style);
                SetWindowPos(handle, -2, posX, posY, sizeX, sizeY, 0x0010);
                tmrSetTopMost.Enabled = false;
            }
        }

        private void tmrSetTopMost_Tick(object sender, EventArgs e)
        {
            IntPtr fwHandle = GetForegroundWindow();
            if (fwHandle == handle && fwHandle != null && handle != null && !topMost)
            {
                SetWindowPos(handle, -1, posX, posY, sizeX, sizeY, 0);
                topMost = true;
            } else if (fwHandle != handle && handle != null && topMost) {
                SetWindowPos(handle, -2, posX, posY, sizeX, sizeY, 0x0010);
                topMost = false;
            }
        }
    }
}