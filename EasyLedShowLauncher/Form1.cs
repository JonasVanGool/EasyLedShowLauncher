using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Toub.Sound.Midi;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EasyLedShowLauncher
{
    public partial class EasyLedShowLauncher : Form
    {
        [DllImport("winmm.dll")]
        protected static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        protected static extern int midiOutGetDevCaps(int deviceID,
            ref MidiOutCaps caps, int sizeOfMidiOutCaps);

        private const int SW_MAXIMIZE = 3;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        Thread processDataThread;
        Thread launchThread;
        SerialPort dmxDataPort;
        Stopwatch launchStopWatch;
        bool blockProcessData = false;
        bool allowProcessData = true;
        bool allowLaunch = true;
        SavedSettings workingSettings;

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetProgressBarCallback(int value);
        delegate void SetReceiveLabelCallback(Color value);


        public struct MidiOutCaps
        {
            #region MidiOutCaps Members

            /// <summary>
            /// Manufacturer identifier of the device driver for the Midi output 
            /// device. 
            /// </summary>
            public short mid;

            /// <summary>
            /// Product identifier of the Midi output device. 
            /// </summary>
            public short pid;

            /// <summary>
            /// Version number of the device driver for the Midi output device. The 
            /// high-order byte is the major version number, and the low-order byte 
            /// is the minor version number. 
            /// </summary>
            public int driverVersion;

            /// <summary>
            /// Product name.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string name;

            /// <summary>
            /// Flags describing the type of the Midi output device. 
            /// </summary>
            public short technology;

            /// <summary>
            /// Number of voices supported by an internal synthesizer device. If 
            /// the device is a port, this member is not meaningful and is set 
            /// to 0. 
            /// </summary>
            public short voices;

            /// <summary>
            /// Maximum number of simultaneous notes that can be played by an 
            /// internal synthesizer device. If the device is a port, this member 
            /// is not meaningful and is set to 0. 
            /// </summary>
            public short notes;

            /// <summary>
            /// Channels that an internal synthesizer device responds to, where the 
            /// least significant bit refers to channel 0 and the most significant 
            /// bit to channel 15. Port devices that transmit on all channels set 
            /// this member to 0xFFFF. 
            /// </summary>
            public short channelMask;

            /// <summary>
            /// Optional functionality supported by the device. 
            /// </summary>
            public int support;

            #endregion
        }

        public class SavedSettings
        {
            public string comPortName { get; set; }
            public string midiDeviceName { get; set; }
            public int midiDeviceIdx { get; set; }
            public string launchDelay { get; set; }
            public string jinxFile { get; set; }
            public string jinxProgram { get; set; }
        }

        public EasyLedShowLauncher(string filePath = null)
        {
            //filePath = @"C:\Users\2425\Desktop\launch1.els";
            workingSettings = new SavedSettings();
            if (filePath != null)
            {
                workingSettings = loadSettings(filePath);
            }

            InitializeComponent();

            InitializeComports(workingSettings);

            InitializeMidiDevices(workingSettings);

            InitializeLaunchDelay(workingSettings);

            InitializeJINXFile(workingSettings);

            InitializeJINXProgram(workingSettings);

            dmxDataPort = new SerialPort();

            this.FormClosing += EasyLedShowLauncher_FormClosing;

            //Auto Launch
            if (filePath != null)
            {
                StartLaunch();
            }
        }


        void CloseData()
        {
            // Stop process thread
            blockProcessData = false;
            allowProcessData = false;

            if (processDataThread != null)
            {
                while (processDataThread.IsAlive) { }
            }


            // Stop launch thread
            allowLaunch = false;
            if (launchThread != null)
            {
                while (launchThread.IsAlive){}
            }

            // Close midi device
            MidiPlayer.CloseMidi();

            // Close serial prot
            CloseComPort();
        }

        void EasyLedShowLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseData();
        }

        // Initialize functions
        private void InitializeComports(SavedSettings settings)
        {
            int selectedindex = 0;
            foreach (String comPortName in SerialPort.GetPortNames())
            {
                cmbComports.Items.Add(comPortName);
                if (settings.comPortName != null && comPortName == settings.comPortName)
                    selectedindex = cmbComports.Items.Count - 1;
            }

            if (settings.comPortName != null)
            {
                if (selectedindex == 0)
                {
                    MessageBox.Show("Comport from file not available");
                }
                else
                {
                    cmbComports.SelectedIndex = selectedindex;
                }
            }
        }

        private void InitializeMidiDevices(SavedSettings settings)
        {
            int selectedindex = 0;
            MidiOutCaps caps = new MidiOutCaps();
            for (int c = 0; c < midiOutGetNumDevs(); c++)
            {
                int result = midiOutGetDevCaps(c, ref caps, Marshal.SizeOf(caps));
                cmbMidiDevices.Items.Add(caps.name);
                if (settings.midiDeviceName != null && caps.name == settings.midiDeviceName)
                    selectedindex = cmbMidiDevices.Items.Count - 1;
            }
            if (settings.midiDeviceName != null)
            {
                if (selectedindex == 0)
                {
                    MessageBox.Show("Midi device from file not available");
                }
                else
                {
                    cmbMidiDevices.SelectedIndex = selectedindex;
                }
            }
        }

        private void InitializeLaunchDelay(SavedSettings settings)
        {
            if (settings.launchDelay != null)
            {
                txtLaunchDelay.Text = settings.launchDelay;
            }
        }

        private void InitializeJINXFile(SavedSettings settings)
        {
            if (settings.jinxFile != null)
            {
                txtJinxFilePath.Text = settings.jinxFile;
            }
        }

        private void InitializeJINXProgram(SavedSettings settings)
        {
            if (settings.jinxProgram != null)
            {
                txtJinxProgram.Text = settings.jinxProgram;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Open JINX! File";
            fileDialog.Filter = "JNX files|*.jnx";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtJinxFilePath.Text = fileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavedSettings saveSettings = new SavedSettings();
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "els files (*.els)|*.els|All files (*.*)|*.*";
            fileDialog.AddExtension = true;
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                //serialize
                using (StreamWriter stream = new StreamWriter(fileDialog.FileName))
                {
  
                    // Set variables
                    saveSettings.comPortName = (string)cmbComports.SelectedItem;
                    saveSettings.midiDeviceName = (string)cmbMidiDevices.SelectedItem;
                    saveSettings.launchDelay = txtLaunchDelay.Text;
                    saveSettings.jinxFile = txtJinxFilePath.Text;
                    saveSettings.jinxProgram = txtJinxProgram.Text;
                    saveSettings.midiDeviceIdx = GetMidiDeviceIndex(saveSettings.midiDeviceName);
                    String json = JsonConvert.SerializeObject(saveSettings);
                    // write variables
                    stream.Write(json);

                    stream.Close();
                }
            }
        }

        private SavedSettings loadSettings(String filePath)
        {
            SavedSettings tempSettings = new SavedSettings();
            using (StreamReader stream = new StreamReader(filePath))
            {
                string temp = stream.ReadLine();
                tempSettings = JsonConvert.DeserializeObject<SavedSettings>(temp);
                return tempSettings;
            }
        }

        private bool launchTest(){
            // Check comport
            GetGUIData();
            try
            {
                OpenComPort();
                CloseComPort();
            }
            catch (Exception e1)
            {
                MessageBox.Show("DMX Comport: \n" + e1.Message);
                return false;
            }

            // Check midi data
            try
            {
                MidiPlayer.OpenMidi(workingSettings.midiDeviceIdx);
                MidiPlayer.CloseMidi();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Midi Device: \n" + e1.Message);
                return false;
            }

            // Check lanch delay
            try
            {
                double.Parse(txtLaunchDelay.Text);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Launch delay: \n" + e1.Message);
                return false;
            }

            // Check jinx file
            if (!File.Exists(txtJinxFilePath.Text))
            {
                MessageBox.Show("JINX! File: \n" + "Jinx file doesn't exist! [" + txtJinxFilePath.Text + "]");
                return false;
            }

            // Check jinx program file
            if (!File.Exists(txtJinxProgram.Text))
            {
                MessageBox.Show("JINX! Program: \n" + "Jinx program doesn't exist! [" + txtJinxFilePath.Text + "]");
                return false;
            }
            return true;
        }

        private void StartProcess()
        {
            // Start Receive thread
            allowProcessData = true;
            blockProcessData = false;

            // Open jinx!
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = txtJinxProgram.Text;
            startInfo.Arguments = @"-s1 " + txtJinxFilePath.Text;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(startInfo);
            Thread.Sleep(2000);
            var p = System.Diagnostics.Process.GetProcessesByName("jinx").FirstOrDefault();
            if (p != null)
            {
                ShowWindow(p.MainWindowHandle, SW_MAXIMIZE);
            }

        }

        private void processData()
        {
            // Block process data
            while (blockProcessData) { }

            byte inByte;
            byte[] dataBlock = new byte[8];
            const int STATE_WAIT_SEQUENCE = 0, STATE_READ_DATA = 1;
            int STATE = STATE_WAIT_SEQUENCE;
            int dataCounter = 0;
            int sequenceCounter = 0;
            byte preStrobe = 0;
            byte preMaster = 255;
            byte workingStrobeByte = 0;
            Color debugColor = Color.Red;
            Stopwatch debugUpdater = new Stopwatch();
            debugUpdater.Start();
            while (allowProcessData)
            {
                //Update debug
                if (debugUpdater.ElapsedMilliseconds > 100)
                {
                    SetBackgroundLabel(debugColor);
                    debugUpdater.Restart();
                    debugColor = Color.Red;
                } 

                // read bytes
                if (dmxDataPort.BytesToRead != 0)
                {
                    inByte = (byte)dmxDataPort.ReadByte();
                    debugColor = Color.Orange;
                    switch (STATE)
                    {
                        case STATE_WAIT_SEQUENCE:
                            if (inByte == 255)
                                sequenceCounter++;
                            else
                                sequenceCounter = 0;

                            if (sequenceCounter == 8)
                            {
                                STATE = STATE_READ_DATA;
                                sequenceCounter = 0;
                            }
                            break;
                        case STATE_READ_DATA:
                            dataBlock[dataCounter] = inByte;
                            dataCounter++;
                            if (dataCounter == 8)
                            {
                                
                                STATE = STATE_WAIT_SEQUENCE;
                                dataCounter = 0;


                                if (dataBlock[0] == 255)
                                {
                                    dataBlock[0] = preMaster;
                                }

                                if (dataBlock[1] == 255)
                                {
                                    workingStrobeByte = 1;
                                }else{
                                    workingStrobeByte = 0;
                                }

                                if (dataBlock[1] == 0 && preStrobe == 1)
                                    workingStrobeByte = 1;
                                MidiPlayer.Play(new Controller(0, 0, (byte)6, Math.Min((byte)(dataBlock[0] / 2),(byte)110)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)5, (byte)workingStrobeByte));
                                MidiPlayer.Play(new Controller(0, 0, (byte)0, (byte)(dataBlock[2] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)1, (byte)(dataBlock[3] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)4, (byte)(dataBlock[4] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)2, (byte)(dataBlock[5] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)3, (byte)(dataBlock[6] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)7, (byte)(dataBlock[7] / 2)));

                                preMaster = dataBlock[0];
                                preStrobe = workingStrobeByte;
                                debugColor = Color.Green;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            StartLaunch();
        }

        private void btnBrowseProgram_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Open JINX program! File";
            fileDialog.Filter = "exe files|*.exe";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtJinxProgram.Text = fileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void launchStart()
        {
            double tempvalue;
            int value;
            while (allowLaunch)
            {
                Thread.Sleep(100);
                tempvalue = ((double)launchStopWatch.Elapsed.Seconds / double.Parse(workingSettings.launchDelay));
                value = (int)(100.0 * tempvalue);
                SetProgressBar(Math.Min(value,100));
                if (tempvalue > 1)
                {
                    allowLaunch = false;
                    StartProcess();
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnLaunch.Enabled = true;

            CloseData();
        }

        private void SetProgressBar(int Value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pgbLaunch.InvokeRequired)
            {
                SetProgressBarCallback d = new SetProgressBarCallback(SetProgressBar);
                this.BeginInvoke(d, new object[] { Value });
            }
            else
            {
                this.pgbLaunch.Value = Value;
            }
        }

        private void SetBackgroundLabel(Color Value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblReceivedBlock.InvokeRequired)
            {
                SetReceiveLabelCallback d = new SetReceiveLabelCallback(SetBackgroundLabel);
                this.BeginInvoke(d, new object[] { Value });
            }
            else
            {
                this.lblReceivedBlock.BackColor = Value;
            }
        }

        private void OpenComPort()
        {
            if (dmxDataPort == null)
            {
                return;
            }
            // Open serial port
            dmxDataPort.PortName = workingSettings.comPortName;
            dmxDataPort.BaudRate = 250000;
            dmxDataPort.Parity = Parity.None;
            dmxDataPort.DataBits = 8;
            dmxDataPort.StopBits = StopBits.Two;
            dmxDataPort.ReadTimeout = 500;
            dmxDataPort.WriteTimeout = 500;
            dmxDataPort.Open();
        }

        private void CloseComPort()
        {
            if (dmxDataPort == null)
            {
                return;
            }
            Thread.Sleep(100);
            dmxDataPort.Close();
        }

        private void GetGUIData()
        {

            workingSettings.comPortName = (string)cmbComports.SelectedItem;
            workingSettings.jinxFile = txtJinxFilePath.Text;
            workingSettings.jinxProgram = txtJinxProgram.Text;
            workingSettings.launchDelay = txtLaunchDelay.Text;
            workingSettings.midiDeviceName = (string)cmbMidiDevices.SelectedItem;
            workingSettings.midiDeviceIdx = GetMidiDeviceIndex(workingSettings.midiDeviceName);
        }

        private int GetMidiDeviceIndex(string name)
        {
            MidiOutCaps caps = new MidiOutCaps();
            for (int c = 0; c < midiOutGetNumDevs(); c++)
            {
                int result = midiOutGetDevCaps(c, ref caps, Marshal.SizeOf(caps));
                if (caps.name == name)
                    return c;
            }
            return -1;
        }

        private void StartLaunch()
        {
            if (launchTest())
            {

                //Open comport
                OpenComPort();

                // Open midi device
                MidiPlayer.OpenMidi(workingSettings.midiDeviceIdx);

                allowLaunch = true;
                launchThread = new Thread(launchStart);
                launchThread.Start();
                launchStopWatch = new Stopwatch();
                launchStopWatch.Start();

                allowProcessData = false;
                blockProcessData = true;
                processDataThread = new Thread(processData);
                processDataThread.Start();

                btnLaunch.Enabled = false;
                btnCancel.Enabled = true;
            }
        }
    }

}
