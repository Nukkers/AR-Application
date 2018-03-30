using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;

public class WatsonSpeechRecognition : MonoBehaviour
{
    private const string watsonUrl = "https://stream.watsonplatform.net/speech-to-text/api";
    private const string watsonUser = "d96273f1-d789-4615-a98e-c6d9835d576c";
    private const string watsonPassword = "Ydu7ifGnKUjI";

    // Use this for initialization
    void Start () {
        Credentials credentials = new Credentials(watsonUser, watsonPassword, watsonUrl);
        SpeechToText _speechToText = new SpeechToText(credentials);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Watson fail handler
    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Debug.LogError("ExampleSpeechToText.OnFail() Error received: " + error.ToString());
    }
}
