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
using System.Management;
using System.Threading;

namespace UWHelper
{
    public partial class Form1 : Form
    {
        #region winAPI setup
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
        # endregion
        /*
         * Start normal code
         */

        // Set up some global variables
        public IntPtr handle;
        public int sizeX, sizeY, posX, posY;
        public bool topMost = false;
        public bool[] Borderless, Automatic;
        public int[] savedSizeX, savedSizeY, savedPosX, savedPosY;
        public UInt64[] DefaultStyle, ChangedStyle;
        public string[] ApplicationName;
        public string[] appliedSettings;
        public string[] ProcessName;
        string SelectedItem = "";
        string toBeSearched = " --- ";
        string pId = "";
        int pIdInt = 0;
        string SelectedApp =  "";
        string SelectedProcessName = "";
        int savedIndex;
        int tIndex;
        bool foundSave = false;
        int tProcId;
        Process[] processes = Process.GetProcesses();
        
        ManagementEventWatcher processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        ManagementEventWatcher processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

        public Form1()
        {
            InitializeComponent();

            processStartEvent.EventArrived += new EventArrivedEventHandler(processStartEvent_EventArrived);
            processStartEvent.Start();
            processStopEvent.EventArrived += new EventArrivedEventHandler(processStopEvent_EventArrived);
            processStopEvent.Start();

            //load settings
            Borderless = Properties.Settings.Default.Borderless;
            Automatic = Properties.Settings.Default.Automatic;
            savedSizeX = Properties.Settings.Default.sizeX;
            savedSizeY = Properties.Settings.Default.sizeY;
            savedPosX = Properties.Settings.Default.posX;
            savedPosY = Properties.Settings.Default.posY;
            DefaultStyle = Properties.Settings.Default.DefaultStyle;
            ChangedStyle = Properties.Settings.Default.ChangedStyle;
            ApplicationName = Properties.Settings.Default.ApplicationName;
            ProcessName = Properties.Settings.Default.ProcessName;
            tmrAutomatic.Enabled = false;
            tmrSetTopMost.Enabled = true;
        }

        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            int processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            for (int i = 0; i < ApplicationName.Length; i++)
            {
                if (Automatic[i])
                {
                    Process p;
                    try { p = Process.GetProcessById(processID); } catch { Console.WriteLine("ProcessID error"); return; }
                    //Console.WriteLine(p.MainWindowTitle);
                    if (p.ProcessName == ProcessName[i])
                    {
                        //Console.WriteLine("Index: " + i + " Processname " + p.ProcessName);
                        tProcId = p.Id;
                        tIndex = i;
                        #region Start Thread
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            int threadProcId = tProcId;
                            int threadIndex = tIndex;
                            Thread.Sleep(20000);
                            Process threadProc;
                            try { threadProc = Process.GetProcessById(threadProcId); } catch { return; }
                            //Console.WriteLine("Thread {0} tappName {1} pappName {2}", threadIndex, threadProc.MainWindowTitle, ApplicationName[threadIndex]);
                            try
                            {
                                if (threadProc.MainWindowTitle == ApplicationName[threadIndex]) {
                                    //Console.WriteLine("Thread {0} appname: {1}", threadIndex, threadProc.MainWindowTitle);
                                    IntPtr pHandle = threadProc.MainWindowHandle;
                                    UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                                    if ((UInt64)style != ChangedStyle[threadIndex])
                                    {
                                        //Console.WriteLine("applying style to " + threadProc.ProcessName);
                                        SetWindowLongPtr(pHandle, GWL_STYLE, (UIntPtr)ChangedStyle[threadIndex]);
                                    }
                                    SetWindowPos(pHandle, -2, savedPosX[threadIndex], savedPosY[threadIndex], savedSizeX[threadIndex], savedSizeY[threadIndex], 0x0010);
                                }
                            } catch {
                            }
                        }).Start();
                        #endregion End Thread
                    }
                }
            }
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();
        }

        #region Textboxes
        // handle integer textbox
        string txtsXOld;
        private void txtsX_TextChanged(object sender, EventArgs e)
        {
            if (txtsXOld == null) { txtsXOld = "1920"; }
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

        string txtsYOld;

        private void txtsY_TextChanged(object sender, EventArgs e)
        {
            if (txtsYOld == null) { txtsYOld = "1080"; }
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

        string txtpXOld;
        private void txtpX_TextChanged(object sender, EventArgs e)
        {
            if (txtpXOld == null) { txtpXOld = "0"; }
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

        string txtpYOld;
        private void txtpY_TextChanged(object sender, EventArgs e)
        {
            if (txtpYOld == null) { txtpYOld = "0"; }
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
#endregion

        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) { return; }

            // Convert list item text to pId and Application Name
            SelectedItem = lstProcesses.GetItemText(lstProcesses.SelectedItem);
            pId = SelectedItem.Substring(SelectedItem.IndexOf(toBeSearched) + toBeSearched.Length);
            SelectedApp = SelectedItem.Remove(SelectedItem.IndexOf(toBeSearched));
            Int32.TryParse(pId, out pIdInt);
            Process proc = Process.GetProcessById(pIdInt);
            SelectedProcessName = proc.ProcessName;
            handle = proc.MainWindowHandle;

            // Apply saved settings to the UI
            foundSave = false;
            if (ApplicationName == null) { return; }
            for (int i = 0; i < ApplicationName.Length; i++)
            {
                if (ApplicationName[i] == SelectedApp)
                {
                    foundSave = true;
                    savedIndex = i;
                    txtsX.Text = savedSizeX[i].ToString();
                    txtsY.Text = savedSizeY[i].ToString();
                    txtpX.Text = savedPosX[i].ToString();
                    txtpY.Text = savedPosY[i].ToString();
                    chkBorderless.Checked = Borderless[i];
                    chkAuto.Checked = Automatic[i];
                    break;
                }
            }
            //if (!foundSave) { MessageBox.Show("no saves"); }
        }

        // Refresh process list
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstProcesses.Items.Clear();
            processes = Process.GetProcesses();
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
                return;
            }

            if (!foundSave)
            {
                ApplicationName = ApplicationName.Concat(new string[] { SelectedApp }).ToArray();
                savedPosX = savedPosX.Concat(new int[] { posX }).ToArray();
                savedPosY = savedPosY.Concat(new int[] { posY }).ToArray();
                savedSizeX = savedSizeX.Concat(new int[] { sizeX }).ToArray();
                savedSizeY = savedSizeY.Concat(new int[] { sizeY }).ToArray();
                Borderless = Borderless.Concat(new bool[] { chkBorderless.Checked }).ToArray();
                Automatic = Automatic.Concat(new bool[] { chkAuto.Checked }).ToArray();
                DefaultStyle = DefaultStyle.Concat(new UInt64[] { (UInt64)GetWindowLongPtr(handle, GWL_STYLE) }).ToArray();
                ChangedStyle = ChangedStyle.Concat(new UInt64[] { (UInt64)GetWindowLongPtr(handle, GWL_STYLE) }).ToArray();
                ProcessName = ProcessName.Concat(new string[] { SelectedProcessName }).ToArray();
                foundSave = true;
            } else
            {
                for (int i = 0; i < ApplicationName.Length; i++)
                {
                    if (ApplicationName[i] == SelectedApp)
                    {
                        savedPosX[i] = posX;
                        savedPosY[i] = posY;
                        savedSizeX[i] = sizeX;
                        savedSizeY[i] = sizeY;
                        Borderless[i] = chkBorderless.Checked;
                        Automatic[i] = chkAuto.Checked;
                        break;
                    }
                }
            }

            if (chkBorderless.Checked)
            {

                UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                style = new UIntPtr(style.ToUInt64() & ~(UInt64)WS_OVERLAPPEDWINDOW);
                style = new UIntPtr(style.ToUInt64() | (UInt64)WS_POPUPWINDOW);
                SetWindowLongPtr(handle, GWL_STYLE, style);
                SetWindowPos(handle, -2, posX, posY, sizeX, sizeY, 0x0010);
                for (int i = 0; i < ApplicationName.Length; i++)
                {
                    if(ApplicationName[i] == SelectedApp)
                    {
                        ChangedStyle[i] = (UInt64)style;
                        break;
                    }
                }
            }
            else
            {
                //UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                //style = new UIntPtr(style.ToUInt64() & ~(UInt64)WS_POPUPWINDOW);
                //style = new UIntPtr(style.ToUInt64() | (UInt64)WS_OVERLAPPEDWINDOW);

                for (int i = 0; i < ApplicationName.Length; i++)
                {
                    if (ApplicationName[i] == SelectedApp)
                    {
                        //Console.WriteLine(DefaultStyle[i].ToString());
                        SetWindowLongPtr(handle, GWL_STYLE, (UIntPtr)DefaultStyle[i]);
                    }
                }
                SetWindowPos(handle, -2, posX, posY, sizeX, sizeY, 0x0010);
            }

            // Save settings
            Properties.Settings.Default.Borderless = Borderless;
            Properties.Settings.Default.Automatic = Automatic;
            Properties.Settings.Default.sizeX = savedSizeX;
            Properties.Settings.Default.sizeY = savedSizeY;
            Properties.Settings.Default.posX = savedPosX;
            Properties.Settings.Default.posY = savedPosY;
            Properties.Settings.Default.DefaultStyle = DefaultStyle;
            Properties.Settings.Default.ChangedStyle = ChangedStyle;
            Properties.Settings.Default.ApplicationName = ApplicationName;
            Properties.Settings.Default.ProcessName = ProcessName;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        bool[] atTop = new bool[0];
        private void tmrSetTopMost_Tick(object sender, EventArgs e)
        {
            if (atTop.Length < ApplicationName.Length)
            {
                atTop = new bool[ApplicationName.Length];
            }
            IntPtr fwHandle = GetForegroundWindow();
            for (int i = 0; i < ApplicationName.Length; i++) {
                Process[] tmProcs = Process.GetProcessesByName(ProcessName[i]);
                foreach (Process tP in tmProcs)
                {
                    UInt64 tmStyle = (UInt64)GetWindowLongPtr(tP.MainWindowHandle, GWL_STYLE);
                    if (tmStyle == ChangedStyle[i] && tmStyle != DefaultStyle[i])
                    {
                        IntPtr tmHandle = tP.MainWindowHandle;
                        if (fwHandle == tmHandle && fwHandle != null && tmHandle != null && !atTop[i])
                        {
                            SetWindowPos(tmHandle, -1, 0, 0, 0, 0, 0x0010 | 0x0001 | 0x0002);
                            atTop[i] = true;
                        }
                        else if (fwHandle != tmHandle && tmHandle != null && atTop[i])
                        {
                            SetWindowPos(tmHandle, -2, 0, 0, 0, 0, 0x0010 | 0x0001 | 0x0002);
                            atTop[i] = false;
                        }
                    }
                }

            }
        }

        // not used atm
        private void tmrAutomatic_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ApplicationName.Length; i++)
            {
                if (Automatic[i])
                {
                    Process p = Process.GetCurrentProcess();
                    //Console.WriteLine(p.MainWindowTitle);
                    if (p.MainWindowTitle == ApplicationName[i])
                    {
                        IntPtr pHandle = p.MainWindowHandle;
                        IntPtr fwHandle = GetForegroundWindow();
                        UIntPtr style = GetWindowLongPtr(handle, GWL_STYLE);
                        //Console.WriteLine("found process: " + p.ProcessName);
                        if ((UInt64)style != ChangedStyle[i])
                        {
                            SetWindowLongPtr(handle, GWL_STYLE, (UIntPtr)ChangedStyle[i]);
                        }


                        if (fwHandle == pHandle && pHandle != null && Borderless[i])
                        {
                            //Console.WriteLine("found borderless: " + ApplicationName[i]);
                            SetWindowPos(pHandle, -1, savedPosX[i], savedPosY[i], savedSizeX[i], savedSizeY[i], 0x0010);
                        }
                        else if (fwHandle != pHandle && pHandle != null || fwHandle == pHandle && pHandle != null && !Borderless[i])
                        {
                           // Console.WriteLine("found !borderless: " + ApplicationName[i]);
                            SetWindowPos(pHandle, -2, savedPosX[i], savedPosY[i], savedSizeX[i], savedSizeY[i], 0x0010);
                        }
                    }
                }
            }
        }
    }
}