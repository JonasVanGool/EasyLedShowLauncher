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
        private const int NR_MAIN_PROGRAMS = 40;
        private const int NR_SLIDERS = 8;

        [DllImport("winmm.dll")]
        protected static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        protected static extern int midiOutGetDevCaps(int deviceID,
            ref MidiOutCaps caps, int sizeOfMidiOutCaps);

        private const int SW_MAXIMIZE = 3;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private static System.Timers.Timer timer;
        private static int secondsElapsed = 0;
        private int[] dmxSubEffects;

        private bool[] thresEnabled;
        private bool[] thresActive;
        private byte[] thresFreeValue;
        private byte[] thresPreValue;
        private int[] thresActivateTime;
        private long[] thresOKTime;


        Thread processDataThread;
        Thread launchThread;
        SerialPort dmxDataPort;
        Stopwatch launchStopWatch;
        Stopwatch thresStopWatch;
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
            //filePath = @"C:\Users\2425\Desktop\newyear.els";
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

            dmxSubEffects = new int[NR_MAIN_PROGRAMS];
            for (int i = 0; i < dmxSubEffects.Length; i++)
            {
                if(i == 0){
                    dmxSubEffects[i] = 4;
                }
                else if (i == 1)
                {
                    dmxSubEffects[i] = 4;
                }
                else
                {
                    dmxSubEffects[i] = 0;
                }         
            }


            thresEnabled = new bool[NR_SLIDERS];
            thresActive = new bool[NR_SLIDERS];
            thresFreeValue = new byte[NR_SLIDERS];
            thresOKTime = new long[NR_SLIDERS];
            thresActivateTime = new int[NR_SLIDERS];
            thresPreValue = new byte[NR_SLIDERS];

            thresEnabled[0] = false;
            thresEnabled[1] = false;
            thresEnabled[2] = true;
            thresEnabled[3] = false;
            thresEnabled[4] = false;
            thresEnabled[5] = false;
            thresEnabled[6] = true;
            thresEnabled[7] = false;
            for(int i =0; i<NR_SLIDERS; i++){
                if(thresEnabled[i]){
                    thresFreeValue[i] = 12;
                    thresActivateTime[i] = 2000;
                    thresActive[i] = false;
                }
            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;


            this.FormClosing += EasyLedShowLauncher_FormClosing;

            //Auto Launch
            if (filePath != null)
            {
                StartLaunch();
            }
        }


        void CloseData()
        {
            timer.Stop();
            timer.Enabled = false;
            secondsElapsed = 0;
            SetProgressBar(0);

            // Stop process thread
            blockProcessData = false;
            allowProcessData = false;

            if (processDataThread != null)
            {
                while (processDataThread.IsAlive) { }
            }

            // Close midi device
            MidiPlayer.CloseMidi();

            // Close serial prot
            CloseComPort();
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            secondsElapsed += 1;

            if (secondsElapsed > double.Parse(workingSettings.launchDelay))
            {
                timer.Stop();
                timer.Enabled = false;
                secondsElapsed = 0;
                //launch program
                if (launchTest())
                {

                    //Open comport
                    OpenComPort();

                    // Open midi device
                    MidiPlayer.OpenMidi(workingSettings.midiDeviceIdx);
                    
                    processDataThread = new Thread(processData);
                    processDataThread.Start();
                    StartProcess();
                }
            }
            else
            {
                SetProgressBar((int)(100.0 * (double)secondsElapsed / double.Parse(workingSettings.launchDelay)));
            }
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
            fileDialog.DefaultExt = "els";
            fileDialog.FilterIndex = 1;
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
            // start threshold time
            thresStopWatch = new Stopwatch();
            thresStopWatch.Start();
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
            byte workingMasterByte = 0;
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

                                // Lock strobe and master when switching scenes
                                if (dataBlock[0] == 255)
                                {
                                    workingMasterByte = preMaster;
                                }
                                else
                                {
                                    workingMasterByte = dataBlock[0];
                                }

                                if (dataBlock[1] == 255)
                                {
                                    workingStrobeByte = 1;
                                }else{
                                    workingStrobeByte = 0;
                                }

                                if (dataBlock[1] == 0 && preStrobe == 1)
                                    workingStrobeByte = 1;

                                // Apply threshold values
                                for (int k = 0; k < NR_SLIDERS; k++)
                                {
                                    if (thresEnabled[k])
                                    {
                                        if (thresActive[k])
                                        {
                                            if (Math.Abs(thresPreValue[k] - dataBlock[k]) > thresFreeValue[k])
                                            {
                                                thresActive[k] = false;
                                                thresOKTime[k] = thresStopWatch.ElapsedMilliseconds;
                                            }
                                        }
                                        else
                                        {
                                            if (thresPreValue[k] == dataBlock[k])
                                            {
                                                if (thresStopWatch.ElapsedMilliseconds - thresOKTime[k] > thresActivateTime[k])
                                                {
                                                    thresActive[k] = true;
                                                }

                                            }
                                            else
                                            {
                                                thresOKTime[k] = thresStopWatch.ElapsedMilliseconds;
                                                thresPreValue[k] = dataBlock[k];
                                            }
                                        }
                                    }
                                }


                                // Master
                                MidiPlayer.Play(new Controller(0, 0, (byte)6, Math.Min((byte)(workingMasterByte / 2), (byte)107)));
                                // Strobo
                                MidiPlayer.Play(new Controller(0, 0, (byte)5, (byte)workingStrobeByte));
                                // Left effect
                                int mainLeftEffect = ConvertRange(0, 255, 0, NR_MAIN_PROGRAMS - 1, thresPreValue[2]);
                                int subLeftEffect = ConvertRange(0, 255, 0, dmxSubEffects[mainLeftEffect], dataBlock[3]);
                                byte leftEffect = (byte) getEffectNumber(mainLeftEffect,subLeftEffect);
                                MidiPlayer.Play(new Controller(0, 0, (byte)0, leftEffect));
                                // Right effect
                                int mainRightEffect = ConvertRange(0, 255, 0, NR_MAIN_PROGRAMS - 1, thresPreValue[6]);
                                int subRightEffect = ConvertRange(0, 255, 0, dmxSubEffects[mainRightEffect], dataBlock[5]);
                                byte rightEffect = (byte) getEffectNumber(mainRightEffect,subRightEffect);
                                MidiPlayer.Play(new Controller(0, 0, (byte)1, rightEffect));


                                // Cross fade
                                MidiPlayer.Play(new Controller(0, 0, (byte)4, (byte)ConvertRange(0, 255, 0, 127, dataBlock[4])));

                                // Other
                                MidiPlayer.Play(new Controller(0, 0, (byte)7, (byte)(dataBlock[7] / 2)));

                                if (dataBlock[0] != 255)
                                    preMaster = workingMasterByte;
                                if (dataBlock[1] != 0)
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
            btnLaunch.Enabled = false;
            btnCancel.Enabled = true;
            GetGUIData();
            timer.Start();
        }

        
        public static int ConvertRange(
            int originalStart, int originalEnd, // original range
            int newStart, int newEnd, // desired range
            int value) // value to convert
        {
            double scale = (double)(newEnd - newStart) / (double)(originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }

        private int getEffectNumber(int maineffect, int subeffect)
        {
            int result=0;
            for (int i = 0; i < dmxSubEffects.Length; i++)
            {
                if (maineffect == i)
                {
                    return result + subeffect + 1;
                }
                else
                {
                    result++;
                    result += dmxSubEffects[i];
                }
            }
            return 0;
        }
    }

}
