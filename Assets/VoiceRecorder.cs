using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using TMPro;

public class VoiceWhisper : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI outputText; // Drag your TMP object here

    [Header("Recording Settings")]
    private AudioClip recordedClip;
    private bool isRecording = false;
    private string micName;
    private int sampleRate = 16000; // Good for Whisper
    private string fileName = "RecordedCommand.wav";

    [Header("Whisper API Settings")]
    private string whisperEndpoint = "https://api.openai.com/v1/audio/transcriptions";

    // 🔑 Use your API key directly here (for testing only)
    private string apiKey = "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";

    void Start()
    {
        // Pick the first available microphone
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            Debug.Log("Using mic: " + micName);
        }
        else
        {
            Debug.LogError("No microphone found!");
        }
    }

    // Call this to start recording
    public void StartRecording()
    {
        if (!isRecording && micName != null)
        {
            recordedClip = Microphone.Start(micName, false, 10, sampleRate);
            isRecording = true;
            Debug.Log("🎙️ Recording started...");
        }
    }

    // Call this to stop recording and send to Whisper
    public void StopRecording()
    {
        if (isRecording)
        {
            Microphone.End(micName);
            isRecording = false;
            Debug.Log("🛑 Recording stopped.");

            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            SaveWav(filePath, recordedClip);
            Debug.Log("Saved WAV to: " + filePath);

            // Start transcription
            StartCoroutine(TranscribeAudio(filePath));
        }
    }

    // Save AudioClip as WAV
    void SaveWav(string filePath, AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        using (var fs = new FileStream(filePath, FileMode.Create))
        using (var bw = new BinaryWriter(fs))
        {
            int frequency = clip.frequency;
            int channels = clip.channels;
            short bitsPerSample = 16;
            int byteRate = frequency * channels * (bitsPerSample / 8);

            // WAV header
            bw.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
            bw.Write(36 + samples.Length * 2);
            bw.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
            bw.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
            bw.Write(16);
            bw.Write((short)1);
            bw.Write((short)channels);
            bw.Write(frequency);
            bw.Write(byteRate);
            bw.Write((short)(channels * bitsPerSample / 8));
            bw.Write(bitsPerSample);
            bw.Write(System.Text.Encoding.UTF8.GetBytes("data"));
            bw.Write(samples.Length * 2);

            // PCM samples
            foreach (var sample in samples)
            {
                short val = (short)(sample * short.MaxValue);
                bw.Write(val);
            }
        }
    }

    // Send audio to Whisper API
    private IEnumerator TranscribeAudio(string filePath)
    {
        byte[] audioData = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddField("model", "whisper-1");
        form.AddBinaryData("file", audioData, Path.GetFileName(filePath), "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(whisperEndpoint, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogError("Whisper API Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log("Whisper raw response: " + response);

                try
                {
                    WhisperTranscription result = JsonUtility.FromJson<WhisperTranscription>(response);

                    if (outputText != null)
                        outputText.text = "Transcription: " + result.text;

                    Debug.Log("📝 Transcription: " + result.text);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("❌ Failed to parse Whisper API response: " + ex.Message);
                }
            }
        }
    }
}

// Renamed to avoid ambiguity
[System.Serializable]
public class WhisperTranscription
{
    public string text;
}
