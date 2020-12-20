using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour {

    public bool useMic = false;
    public AudioClip clip;
    AudioSource a;
    public AudioMixerGroup amgMic;
    public AudioMixerGroup amgMaster;

    public string selectedDevice;

    public static int frameSize = 512;
    public static float[] spectrum;
    public static float[] wave;
    
    public static float[] bands;

    public static float amplitude = 0;
    public static float smoothedAmplitude = 0;

    public float binWidth;
    public float sampleRate;

    /*
     * 20-60 - Subbase
     * 60-250 - Bass
     * 250-500 - Low midrange
     * 500 - 2Khz - Midrange
     * 2Khz - 4Khz - Upper midrange
     * 4Khz - 6Khz - Presence
     * 6Khz - 20Khz - Brilliance
    */

    private void Awake()
    {
        a = GetComponent<AudioSource>();
        spectrum = new float[frameSize];
        wave = new float[frameSize];
        bands = new float[(int) Mathf.Log(frameSize, 2)];
        
        if (useMic)
        {
            Debug.Log("Microphone is used");
            if (Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[0].ToString();
                a.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);
                a.loop = true;
                a.outputAudioMixerGroup = amgMic;
            }
        }
        else
        {
            a.clip = clip;
            a.outputAudioMixerGroup = amgMaster;
        }
        a.Play();
    }

    // Use this for initialization
    void Start () {        
        sampleRate = AudioSettings.outputSampleRate;
        binWidth = AudioSettings.outputSampleRate / 2 / frameSize;
    }
    
    void GetFrequencyBands()
    {        
        for (int i = 0; i < bands.Length; i++)
        {
            int start = (int)Mathf.Pow(2, i) - 1;
            int width = (int)Mathf.Pow(2, i);
            int end = start + width;
            float average = 0;
            for (int j = start; j < end; j++)
            {
                average += spectrum[j] * (j + 1);
            }
            average /= (float) width;
            bands[i] = average;
            // Debug.Log(i + "\t" + start + "\t" + end + "\t" + start * binWidth + "\t" + (end * binWidth));
        }
    }

    public void GetAmplitude()
    {
        // Calculate the average amplitude of the frame of audio
        // From the wave array
        // And calculate the lerpedAmplitude
        // By lerping to it
        float sum = 0;
        for (int i = 0; i < wave.Length; i++) {
            // print(wave[i]);
            sum = sum + Mathf.Abs(wave[i]);
        }

        float avg = sum / (float) wave.Length;
        amplitude = avg;
        // print(avg);
    }
    
    
    // Update is called once per frame
    void Update () {
        a.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        a.GetOutputData(wave, 0);
        GetAmplitude();
        GetFrequencyBands();
    }
}