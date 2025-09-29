using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using TMPro;

public class WhisperAPI : MonoBehaviour
{
    public TextMeshProUGUI outputText; // Link to your VR canvas text
    private string whisperEndpoint = "https://api.openai.com/v1/audio/transcriptions";

    // 🔑 Directly assign your API key here
    private string apiKey = "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";

    public IEnumerator TranscribeAudio(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"❌ Audio file not found at path: {filePath}");
            yield break;
        }

        byte[] audioData = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddField("model", "whisper-1");
        form.AddBinaryData("file", audioData, Path.GetFileName(filePath), "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(whisperEndpoint, form))
        {
            // 🔑 Here we use the API key directly
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
                    WhisperResponse result = JsonUtility.FromJson<WhisperResponse>(response);

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

[System.Serializable]
public class WhisperResponse
{
    public string text;
}
