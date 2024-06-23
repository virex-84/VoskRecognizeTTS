using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Newtonsoft.Json;
using Vosk;

/*
 * https://github.com/virex-84
*/

namespace VoskRecognizeTTS
{
    public partial class frmMain : Form
    {
        private class MicInfo
        {
            public string deviceName { get; set; }
            public int deviceID { get; set; }
        }
        private class WorkerParameters
        {
            public string voskPath { get; set; }
            public string voiceName { get; set; }
            public int voiceRate { get; set; }
            public int voiceVolume { get; set; }
            public string lang { get; set; } //en, fr, ru
            public string wavPath { get; set; }
            public string text { get; set; }
            public string savePath { get; set; }

            public bool playRecognized { get; set; }
        }

        private class RecognizeWord
        {
            public double conf { get; set; }
            public double end { get; set; }
            public double start { get; set; }
            public string word { get; set; }
        }
        private class RecognizeResult
        {
            public RecognizeWord[] result { get; set; }
            public string text { get; set; }
        }

        int recognize_variant = 0; //0 - tts, 1 - mic, 2 - wav

        //localized resources
        //add a default value because when you edit the form, Visual Studio recreates the ".resx" file and the orphaned text that you added manually
        internal string recognize_text => getString("recognize", "Recognize");
        internal string stop_text => getString("stop","Stop");
        internal string can_not_save_file_text => getString("can_not_save_to_file", "Can not save to file!");
        internal string vosk_model_path_is_empty_text => getString("vosk_model_path_is_empty", "Vosk model path is empty!");
        internal string not_select_microfone_text => getString("not_select_microfone", "Not select microfone!");
        

        private VoskRecognizer recognizer;
        private WaveInEvent waveIn;
        private WaveFileWriter waveFile; //save from microfone

        private bool break_operation = false;

        public frmMain()
        {
            InitializeComponent(); //winforms
            InitializeVoices(); //tts voices
            InitializeMicrofones(); //system microfones
            selectCheckVariant(rbTTS,null); //enable/disable form's controls
            saveToFileCheckedChanged(rbTTS, null);
        }

        private void InitializeVoices()
        {
            cbTTSVoice.Items.Clear();

            cbTTSVoice.DisplayMember = "Name";
            cbTTSVoice.ValueMember = "Name";

            using (var synth = new SpeechSynthesizer())
            {
                //fix search all voices
                synth.InjectOneCoreVoices();

                cbTTSVoice.Items.Clear();
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    cbTTSVoice.Items.Add(info);
                }
                if (cbTTSVoice.Items.Count > 0)
                    cbTTSVoice.SelectedIndex = 0;
            }
        }

        private static string getString(string name, string defaultValue)
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));

            /*
            var resourceSet = resources.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry de in resourceSet)
                {
                    var a = de.Key;
                    var t = de.Value;
                }
            */

            string result = resources.GetString(name);

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        private void InitializeMicrofones()
        {
            cbMicrofone.Items.Clear();

            cbMicrofone.DisplayMember = "deviceName";
            cbMicrofone.ValueMember = "deviceName";

            Dictionary<string, MMDevice> retVal = new Dictionary<string, MMDevice>();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                //deviceInfo.ProductName truncated 32 simbols
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
                {
                    if (device.FriendlyName.StartsWith(deviceInfo.ProductName))
                    {
                        MicInfo info = new MicInfo() {deviceID = waveInDevice, deviceName = device.FriendlyName };
                        cbMicrofone.Items.Add(info);
                        break;
                    }
                }
            }

            if (cbMicrofone.Items.Count > 0)
                cbMicrofone.SelectedIndex = 0;
        }

        private void selectCheckVariant(object sender, EventArgs e)
        {
            //default - all disabled
            cbTTSVoice.Enabled = false;
            tbTTSRate.Enabled = false;
            lbTTSRate.Enabled = false;
            tbTTSVolume.Enabled = false;
            lbTTSVolume.Enabled = false;
            cbMicrofone.Enabled = false;
            tbWavPath.Enabled = false;
            btnWavPath.Enabled = false;
            cbTTSSavePath.Enabled = false;
            tbSavePath.Enabled = false;
            btnSavePath.Enabled = false;
            cbPlayRecognized.Enabled = false;

            if (sender is RadioButton)
            switch ((sender as RadioButton).Name)
            {
                case "rbTTS":
                        cbTTSVoice.Enabled = true;

                        tbTTSRate.Enabled = true;
                        lbTTSRate.Enabled = true;
                        tbTTSVolume.Enabled = true;
                        lbTTSVolume.Enabled = true;

                        cbTTSSavePath.Enabled = true;
                        tbSavePath.Enabled = true;
                        btnSavePath.Enabled = true;
                        cbPlayRecognized.Enabled = true;

                        recognize_variant = 0;
                    break;
                case "rbMic":
                        cbMicrofone.Enabled = true;

                        cbTTSSavePath.Enabled = true;
                        tbSavePath.Enabled = true;
                        btnSavePath.Enabled = true;

                        recognize_variant = 1;
                        break;
                case "rbWav":
                        tbWavPath.Enabled = true;
                        btnWavPath.Enabled = true;
                        cbPlayRecognized.Enabled = true;
                        recognize_variant = 2;
                        break;
            }
        }

        private void saveToFileCheckedChanged(object sender, EventArgs e)
        {
            tbSavePath.Enabled = cbTTSSavePath.Checked;
            btnSavePath.Enabled = cbTTSSavePath.Checked;
        }

        [HandleProcessCorruptedStateExceptions()] //catch access violation exception
        private void btnRecognize_Click(object sender, EventArgs e)
        {
            switch (recognize_variant)
            {
                case 0: //TTS
                case 2: //Wav
                    if (backgroundWorkerRecognize.IsBusy)
                    {
                        break_operation = true;
                        if (backgroundWorkerRecognize.WorkerSupportsCancellation)
                            backgroundWorkerRecognize.CancelAsync();
                        return;
                    }

                    break_operation = false;
                    tbLog.Clear();
                    tbResult.Text = tbText.Text;

                    var parameters = new WorkerParameters() { text = tbText.Text, voskPath = tbVoskPath.Text };
                    parameters.playRecognized = cbPlayRecognized.Checked;

                    if (recognize_variant == 2){
                        //Wav
                        parameters.wavPath = tbWavPath.Text;
                    } else
                    {
                        //TTS
                        VoiceInfo voice = (VoiceInfo)cbTTSVoice.Items[cbTTSVoice.SelectedIndex];
                        string voiceName = voice.Name;
                        string lang = voice.Culture.TwoLetterISOLanguageName;
                        parameters.voiceName = voiceName;
                        parameters.lang = lang;

                        parameters.voiceRate = tbTTSRate.Value;
                        parameters.voiceVolume = tbTTSVolume.Value;

                        //save file
                        if (cbTTSSavePath.Checked)
                            parameters.savePath = tbSavePath.Text;
                    }

                    backgroundWorkerRecognize.RunWorkerAsync(parameters);

                    gbControls.Enabled = false;
                    break;
                case 1: //Mic
                    if (waveIn == null)
                    {
                        if (cbMicrofone.SelectedIndex < 0 || cbMicrofone.SelectedIndex > cbMicrofone.Items.Count)
                        {
                            showMessage(not_select_microfone_text);
                            return;
                        }

                        gbControls.Enabled = false;

                        break_operation = false;
                        tbLog.Clear();
                        tbResult.Text = tbText.Text;

                        InitializeVosk(tbVoskPath.Text);

                        waveIn = new WaveInEvent();
                        MicInfo micInfo = (MicInfo)cbMicrofone.Items[cbMicrofone.SelectedIndex];
                        waveIn.DeviceNumber = micInfo.deviceID;
                        waveIn.WaveFormat = new WaveFormat(16000, 1);
                        waveIn.DataAvailable += WaveIn_DataAvailable;

                        if (cbTTSSavePath.Checked && tbSavePath.Text.Length > 0)
                            try
                            {
                                waveFile = new WaveFileWriter(tbSavePath.Text, waveIn.WaveFormat);
                            }
                            catch (Exception)
                            {

                            }

                        waveIn.StartRecording();
                        btnRecognize.Text = stop_text;
                    }
                    else
                    {
                        break_operation = true;

                        if (waveFile != null)
                        {
                            waveFile.Flush();
                            waveFile.Dispose();
                            waveFile = null;
                        }

                        StopVosk();

                        waveIn.StopRecording();
                        waveIn.Dispose();
                        waveIn = null;

                        btnRecognize.Text =  recognize_text;

                        gbControls.Enabled = true;
                    }
                    break;
            }
        }

        private void showMessage(string text)
        {
            MessageBox.Show(text);
        }

        private void SpeechToStream(string text, string voiceName, Stream streamAudio, int Rate = -1, int Volume = 100)
        {
            using (var synth = new SpeechSynthesizer())
            {
                //fix search all voices
                synth.InjectOneCoreVoices();

                //audio format for best recognize in vosk
                var format = new System.Speech.AudioFormat.SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
                synth.SetOutputToAudioStream(streamAudio, format);

                var builder = new PromptBuilder();

                synth.SelectVoice(voiceName);

                synth.Rate = Rate;
                synth.Volume = Volume;

                builder.StartVoice(synth.Voice);
                builder.AppendText(text);
                builder.EndVoice();

                synth.Speak(builder);
                synth.SetOutputToNull();

                streamAudio.Position = 0;
            }
        }

        private void WavToStream(string wavPath, Stream streamAudio)
        {
            using (Stream source = File.OpenRead(wavPath))
            {
                if (break_operation) return;

                source.CopyTo(streamAudio);
            }
            streamAudio.Position = 0;
        }

        [HandleProcessCorruptedStateExceptions()] //catch access violation exception
        private void backgroundWorkerRecognize_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var parameters = (e.Argument as WorkerParameters);
            try
            {
                InitializeVosk(parameters.voskPath); //path to vosk model
            }
            catch (Exception exception)
            {
                showMessage(exception.Message);
                return;
            }

            if (btnRecognize.InvokeRequired)
            {
                btnRecognize.Invoke(new Action(() =>
                {
                    btnRecognize.Text = stop_text;
                }));
            }

            using (var playAudio = new MemoryStream())
            using (var streamAudio = new MemoryStream())
            {
                if (parameters.wavPath != null)
                    try
                    {
                        WavToStream(parameters.wavPath, streamAudio);
                    }
                    catch (Exception exception)
                    {
                        //example: file not found
                        showMessage(exception.Message);
                        return;
                    }

                if (parameters.voiceName != null)
                {
                    SpeechToStream(parameters.text, parameters.voiceName, streamAudio, parameters.voiceRate, parameters.voiceVolume);

                    //save file
                    if (parameters.savePath!= null && parameters.savePath.Length > 0)
                        using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new RawSourceWaveStream(streamAudio, new WaveFormat(16000, 16, 1))))
                        {
                            streamAudio.Position = 0;
                            try
                            {
                                WaveFileWriter.CreateWaveFile(parameters.savePath, waveStream);
                            }
                            catch {
                                showMessage(can_not_save_file_text);
                                return;
                            }
                        }
                    streamAudio.Position = 0;
                }

                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = streamAudio.Read(buffer, 0, buffer.Length)) > 0)
                {

                    if (break_operation) return;
                    if (e.Cancel) return;
                    if (recognizer == null) break;

                    //add for play
                    if (parameters.playRecognized)
                        playAudio.Write(buffer, 0, buffer.Length);

                    if (recognizer.AcceptWaveform(buffer, buffer.Length))
                    {
                        //recognizer.Result - faster, but if source text is small, then there will be no result
                        if (recognizer != null)
                            UpdateFinalTextBox(recognizer.Result(), parameters.lang);

                        Application.DoEvents();

                        if (parameters.playRecognized)
                        {
                            //play recognized chunk
                            playAudio.Position = 0;
                            using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new RawSourceWaveStream(playAudio, new WaveFormat(16000, 16, 1)))))
                            {
                                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                                {
                                    waveOut.Init(blockAlignedStream);
                                    waveOut.Play();
                                    while (waveOut.PlaybackState == PlaybackState.Playing)
                                    {
                                        if (break_operation) return;
                                        if (e.Cancel) return;
                                        Thread.Sleep(10);
                                    }
                                }
                            }
                            //clear chunk
                            playAudio.SetLength(0);
                        }
                        
                    }
                    else
                    {
                        //recognizer.PartialResult()
                    }
                }

                //FinalResult - slowly, but more accurately
                if (recognizer != null) {
                    UpdateFinalTextBox(recognizer.FinalResult(), parameters.lang);

                    if (parameters.playRecognized)
                    {
                        //play recognized chunk
                        playAudio.Position = 0;
                        using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new RawSourceWaveStream(playAudio, new WaveFormat(16000, 16, 1)))))
                        {
                            using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                            {
                                waveOut.Init(blockAlignedStream);
                                waveOut.Play();
                                while (waveOut.PlaybackState == PlaybackState.Playing)
                                {
                                    if (break_operation) return;
                                    if (e.Cancel) return;
                                    Thread.Sleep(10);
                                }
                            }
                        }
                        //clear chunk
                        playAudio.SetLength(0);
                    }
                }
            }
        }

        [HandleProcessCorruptedStateExceptions()] //catch access violation exception
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (recognizer == null) return;
            try
            {
                if (waveFile != null)
                {
                    waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                    waveFile.Flush();
                }

                if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                {
                    //string txt = recognizer.Result(); vosk ASSERTION_FAILED (VoskAPI:Compute():mel-computations.cc:229) Assertion failed: (mel_energies_out->Dim() == num_bins)
                    string txt = recognizer.FinalResult();
                    UpdateFinalTextBox(txt, "default");
                }
                else
                {
                    //recognizer.PartialResult()
                }
            }
            //recognizer.FinalResult()
            catch (Exception)
            {
            }
        }

        private void UpdateFinalTextBox(string text, string language)
        {
            if (text.Length == 0) return;
            RecognizeResult values = JsonConvert.DeserializeObject<RecognizeResult>(text);

            if (values.text.Length == 0) return;
            foreach (var word in values.result)
            {
                Log(word.word);

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        //+ Environment.NewLine необходимо для исключения ошибки, не влияет на результат
                        var sb = new StringBuilder(tbResult.Text + Environment.NewLine);

                        //last selected word
                        int start = sb.ToString().LastIndexOf(']');

                        //default
                        Regex regex = new Regex(@"\w+", RegexOptions.IgnoreCase);

                        switch (language)
                        {
                            case "en":
                            case "fr":
                                //summer's
                                regex = new Regex(@"[\p{L}\\\\p{N}_\’\']+", RegexOptions.IgnoreCase);
                                break;
                        }

                        var maches = regex.Matches(sb.ToString());
                        string w = "";
                        foreach (Match m in maches)
                        {
                            //ignore word if last simbol ]
                            string simb = sb.ToString().Substring(m.Index + m.Length, 1);
                            if (simb.Contains("]")) continue;

                            w = sb.ToString().Substring(m.Index, m.Length);

                            if (w.Equals(word.word, StringComparison.OrdinalIgnoreCase))
                            {
                                sb.Insert(m.Index, "[");
                                sb.Insert(m.Index + m.Length + 1, "]");
                                tbResult.Text = sb.ToString();
                                return;
                            }

                        }

                        tbResult.Text = tbResult.Text.Trim();
                    }));
                }
            }
        }

        private void backgroundWorkerRecognize_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            StopVosk();

            if (btnRecognize.InvokeRequired)
            {
                btnRecognize.Invoke(new Action(() =>
                {
                    btnRecognize.Text = recognize_text;
                    gbControls.Enabled = true;
                }));
            }
            else
            {
                btnRecognize.Text = recognize_text;
                gbControls.Enabled = true;
            }
        }

        private void InitializeVosk(string modelPath)
        {
            if (recognizer != null) return;

            if (!Directory.Exists(modelPath) || modelPath == null || modelPath.Trim().Length == 0)
                throw new Exception(vosk_model_path_is_empty_text);

            Vosk.Vosk.SetLogLevel(-1);

            Model model = new Model(modelPath);

            recognizer = new VoskRecognizer(model, 16000.0f);
            recognizer.SetMaxAlternatives(0); //set no alternate tags
            recognizer.SetWords(true); //add array of words
        }

        private void StopVosk()
        {
            if (recognizer != null)
            {
                recognizer.Dispose();
                recognizer = null;
            }
        }

        private void Log(string Text)
        {
            if (tbLog.InvokeRequired)
            {
                tbLog.Invoke(new Action(() =>
                {
                    tbLog.Text += " " + Text;
                    tbLog.SelectionStart = tbLog.TextLength;
                    tbLog.ScrollToCaret();
                }));
            }
            else
            {
                tbLog.SelectionStart = tbLog.TextLength;
                tbLog.ScrollToCaret();
                tbLog.Text += " " + Text;
            }
        }

        private void btnVoskPath_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();
                if (Directory.Exists(tbVoskPath.Text))
                    folderBrowserDialog.SelectedPath = Path.GetFullPath(tbVoskPath.Text);
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                     tbVoskPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btnWavPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.wav (*.wav)|*.wav";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (File.Exists(tbWavPath.Text))
                openFileDialog.InitialDirectory = Path.GetFullPath(tbWavPath.Text);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbWavPath.Text = openFileDialog.FileName;
            }
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.wav (*.wav)|*.wav";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = false;

            if (File.Exists(tbSavePath.Text))
                openFileDialog.InitialDirectory = Path.GetFullPath(tbSavePath.Text);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbSavePath.Text = openFileDialog.FileName;
            }
        }

    }
}
