using Lib.Assets.Logging;
using NAudio.Wave;
using System.Security.Cryptography.X509Certificates;

namespace Lib.Converters;
public class VoiceRecorder
{
    #region Fields

    private readonly WaveInEvent waveIn;
    private WaveFormat waveFormat;

    private int sampleRate;
    private int channels;
    private int bitDepth;

    #endregion

    #region Ctors

    public VoiceRecorder(int sampleRate = 44100, int channels = 2, int bitDepth = 16)
    {
        waveIn = new WaveInEvent();
        waveFormat = new WaveFormat(sampleRate, channels, bitDepth);

        this.sampleRate = sampleRate;
        this.channels = channels;
        this.bitDepth = bitDepth;
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

    #endregion

    #region Events

    public event EventHandler<WaveInEventArgs> DataAvailable
    {
        add => waveIn.DataAvailable += value;
        remove => waveIn.DataAvailable -= value;
    }

    #endregion

    #region Methods

    public void StartRecord()
    {
        waveIn.WaveFormat = waveFormat;
        waveIn.StartRecording();
        Logger.GetLogger().MicroInfo("Запись голоса начата");
    }
    public void EndRecord()
    {
        waveIn.StopRecording();
        Logger.GetLogger().MicroInfo("Запись голоса окончена");
    }

    #endregion
}
