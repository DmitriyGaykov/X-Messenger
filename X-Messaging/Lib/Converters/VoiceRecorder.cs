using Lib.Assets.Logging;
using NAudio.Wave;

namespace Lib.Converters;
public class VoiceRecorder
{
    #region Fields

    private WaveInEvent waveIn;
    private WaveFormat waveFormat;
    private WaveOutEvent waveOut;
    private EventHandler<StoppedEventArgs> onStopped;

    private int sampleRate;
    private int channels;
    private int bitDepth;
    private readonly List<byte> buffer = new();

    #endregion

    #region Ctors

    public VoiceRecorder(int sampleRate = 44100, int channels = 2, int bitDepth = 16)
    {
        waveFormat = new WaveFormat(sampleRate, bitDepth, channels);

        this.sampleRate = sampleRate;
        this.channels = channels;
        this.bitDepth = bitDepth;

        OnStopped += WhenStoped;
    }

    #endregion

    #region Props

    public int? SampleRate
    {
        get => this.sampleRate;
        set
        {
            this.sampleRate = value.HasValue ? value.Value : 44100;
            waveFormat = new WaveFormat(sampleRate, channels, bitDepth);
        }
    }

    public int? Channels
    {
        get => this.channels;
        set
        {
            this.channels = value ?? 2;

            waveFormat = new WaveFormat(sampleRate, channels, bitDepth);
        }
    }

    public int? BitDepth
    {
        get => this.bitDepth;
        set
        {
            this.bitDepth = value ?? 16;

            waveFormat = new WaveFormat(sampleRate, channels, bitDepth);
        }
    }

    public List<byte> Buffer => new(buffer);

    public bool IsStoped { get; private set; } = true;

    public event EventHandler<StoppedEventArgs> OnStopped 
    { 
        add => this.onStopped += value;
        remove => this.onStopped -= value;
    }

    #endregion

    #region Events

    #endregion

    #region Methods

    public void StartRecord()
    {
        waveIn = new()
        {
            WaveFormat = waveFormat
        };

        waveIn.DataAvailable += RecordBytes;

        buffer.Clear();
        waveIn.StartRecording();
        Logger.GetLogger().MicroInfo("Запись голоса начата");

        while(waveIn.BufferMilliseconds is not > 0)
        {

        }
    }
    public void EndRecord()
    {
        waveIn.StopRecording();
        Logger.GetLogger().MicroInfo("Запись голоса окончена");
    }
    public void StartVoicePresent(byte[] voice)
    {
        IsStoped = false;
        MemoryStream stream = new(voice);
        RawSourceWaveStream rawSourceStream = new(stream, waveFormat);
        waveOut = new();
        waveOut.Init(rawSourceStream);
        waveOut.Play();
        waveOut.PlaybackStopped += onStopped;
    }

    public void StopVoicePresent()
    {
        waveOut.Stop();
        waveOut.Dispose();
    }

    private void RecordBytes(object sender, WaveInEventArgs e)
    {
        buffer.AddRange(e.Buffer);
    }

    private void WhenStoped(object sender, StoppedEventArgs e)
    {
        IsStoped = true;
    }


    #endregion
}
