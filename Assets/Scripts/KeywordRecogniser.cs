using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class KeywordRecogniser : MonoBehaviour {

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start () {

        //Create keywords for keyword recognizer
        keywords.Add("start", () =>
        {
            // action to be performed when this keyword is spoken
            Debug.Log("Voice Recognised: Start called");
        });

        keywords.Add("describe", () =>
        {
            // action to be performed when this keyword is spoken
            Debug.Log("Voice Recognised: Describe called");
        });

        //Start recognizer
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        //Now register for the OnPhraseRecognized event
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        //Start recognizer
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Function to call when onphraserecognized
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

}
