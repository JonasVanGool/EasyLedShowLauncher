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

namespace EasyLedShowLauncher
{
    public partial class EasyLedShowLauncher : Form
    {
        [DllImport("winmm.dll")]
        protected static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        protected static extern int midiOutGetDevCaps(int deviceID,
            ref MidiOutCaps caps, int sizeOfMidiOutCaps);

        Thread processDataThread;
        Thread launchThread;
        SerialPort dmxDataPort;
        Stopwatch launchStopWatch;
        bool allowProcessData = true;
        bool allowLaunch = true;

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetProgressBarCallback(int value);
        delegate void SetReceiveLabelCallback(Color value);
        delegate void SetLaunchCallBack();
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
            public string launchDelay { get; set; }
            public string jinxFile { get; set; }
            public string jinxProgram { get; set; }
        }

        public EasyLedShowLauncher(string filePath = null)
        {
            filePath = @"C:\Users\2425\Desktop\test.els";
            SavedSettings tempSettings = new SavedSettings();
            if (filePath != null)
            {
                tempSettings = loadSettings(filePath);
            }

            InitializeComponent();

            InitializeComports(tempSettings);

            InitializeMidiDevices(tempSettings);

            InitializeLaunchDelay(tempSettings);

            InitializeJINXFile(tempSettings);

            InitializeJINXProgram(tempSettings);

            this.FormClosing += EasyLedShowLauncher_FormClosing;
        }


        void CloseData()
        {
            // Stop process thread
            allowProcessData = false;
            if (processDataThread != null)
                while (processDataThread.IsAlive) { }

            // Stop launche thread
            allowLaunch = false;
            if (launchThread != null)
                while (launchThread.IsAlive) { }

            // Close midi device
            MidiPlayer.CloseMidi();

            // Close serial prot
            if (dmxDataPort != null)
            {
                if (dmxDataPort.IsOpen)
                    dmxDataPort.Close();
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

        private bool launcheTest(){
            // Check comport
            try
            {
                SerialPort testPort = new SerialPort((string)cmbComports.SelectedItem, 250000, Parity.None, 8, StopBits.Two);
                testPort.Open();
                testPort.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show("DMX Comport: \n" + e1.Message);
                return false;
            }

            // Check midi data
            try
            {
                MidiPlayer.OpenMidi(cmbMidiDevices.SelectedIndex);
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

        private void launch()
        {
            // Open serial port
            dmxDataPort = new SerialPort((string)cmbComports.SelectedItem, 250000, Parity.None, 8, StopBits.Two);
            dmxDataPort.ReadTimeout = 500;
            dmxDataPort.WriteTimeout = 500;
            dmxDataPort.Open();

            // Open midi device
            MidiPlayer.OpenMidi(cmbMidiDevices.SelectedIndex);

            // Start Receive thread
            allowProcessData = true;
            processDataThread = new Thread(processData);
            processDataThread.Start();

            // Open jinx!
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = txtJinxProgram.Text;
            startInfo.Arguments = @"-s1 " + txtJinxFilePath.Text;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(startInfo);
        }

        private void processData()
        {
            byte inByte;
            byte[] dataBlock = new byte[8];
            const int STATE_WAIT_SEQUENCE = 0, STATE_READ_DATA = 1;
            int STATE = STATE_WAIT_SEQUENCE;
            int dataCounter = 0;
            int sequenceCounter = 0;
            byte preStrobe = 0;
            byte preMaster = 255;
            Color debugColor = Color.Red;
            Stopwatch debugUpdater = new Stopwatch();
            debugUpdater.Start();
            while (allowProcessData)
            {
                //Update debyg
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

                                // Make data correction when rapid scene switch
                                if (dataBlock[0] == 255)
                                {
                                    dataBlock[0] = preMaster;
                                }

                                if (dataBlock[1] == 0)
                                {
                                    dataBlock[1] = preStrobe;
                                }

                                MidiPlayer.Play(new Controller(0, 0, (byte)6, (byte)(dataBlock[0] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)5, (byte)(dataBlock[1] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)0, (byte)(dataBlock[2] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)1, (byte)(dataBlock[3] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)4, (byte)(dataBlock[4] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)2, (byte)(dataBlock[5] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)3, (byte)(dataBlock[6] / 2)));
                                MidiPlayer.Play(new Controller(0, 0, (byte)7, (byte)(dataBlock[7] / 2)));

                                preMaster = dataBlock[0];
                                preStrobe = dataBlock[1];

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
            if (launcheTest())
            {
                allowLaunch = true;
                launchThread = new Thread(launchStart);
                launchThread.Start();
                launchStopWatch = new Stopwatch();
                launchStopWatch.Start();
                btnLaunch.Enabled = false;
                btnCancel.Enabled = true;
            }
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
                tempvalue = ((double)launchStopWatch.Elapsed.Seconds / double.Parse(txtLaunchDelay.Text));
                value = (int)(100.0 * tempvalue);
                SetProgressBar(Math.Min(value,100));
                Thread.Sleep(20);
                if (tempvalue > 1)
                {
                    allowLaunch = false;
                    SetLaunch();
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
                this.Invoke(d, new object[] { Value });
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
            if (this.pgbLaunch.InvokeRequired)
            {
                SetReceiveLabelCallback d = new SetReceiveLabelCallback(SetBackgroundLabel);
                this.Invoke(d, new object[] { Value });
            }
            else
            {
                this.lblReceivedBlock.BackColor = Value;
            }
        }

        private void SetLaunch()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pgbLaunch.InvokeRequired)
            {
                SetLaunchCallBack d = new SetLaunchCallBack(SetLaunch);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                launch();
            }
        }

    }
}
