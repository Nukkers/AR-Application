/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System.Collections.Generic;
using UnityEngine.UI;

public class ExampleStreaming : MonoBehaviour
{
    const string _username = "d96273f1-d789-4615-a98e-c6d9835d576c";
    const string _password = "Ydu7ifGnKUjI";
    const string _url = "https://stream.watsonplatform.net/speech-to-text/api";

    [Header("Settings")]
    [Tooltip("Disable speech recognition if not needed to prevent unneccesary use of cloud allowance")]
    public bool Enabled = false;    //Allow switch off to preserve cloud allowance

    [Tooltip("Keywords being detected. If changing, amend code to trigger required functions.")]
    public string[] keywords = { "start", "quit", "test"};  //Keywords to watch for

    [Tooltip("Tolerance to recognise keyword - too low will cause false recognitions, too high will miss recognitions. Default 0.2f.")]
    [Range(0.0f, 1.0f)]
    public float keywordConfidence = 0.2f; //Confidence - low due to uniqueness of words

    [Header("Debugging")]

    [Tooltip("Print all recognition to console if enabled.")]
    public bool DebugMode = false;    //Switches on console logging for all recognition

    [Tooltip("Text field for recognition result to be displayed")]
    public Text ResultsField;   //Where output will be displayed - for debugging
    [Tooltip("UI overlay for debugging")]
    public GameObject ResultsDisplay; //Display UI

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToText _speechToText;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        ResultsDisplay.SetActive(DebugMode);


        //  Create credential and instantiate service
        Credentials credentials = new Credentials(_username, _password, _url);

        _speechToText = new SpeechToText(credentials);
        Active = true;

        StartRecording();
    }

    public bool Active
    {
        get { return _speechToText.IsListening; }
        set
        {
            if (value && !_speechToText.IsListening && Enabled)
            {
                _speechToText.RecognizeModel = "en-GB_BroadbandModel";
                _speechToText.DetectSilence = true;
                _speechToText.EnableWordConfidence = true;
                _speechToText.EnableTimestamps = true;
                _speechToText.SilenceThreshold = 0.01f;
                _speechToText.MaxAlternatives = 0;
                _speechToText.EnableInterimResults = false;
                _speechToText.OnError = OnError;
                _speechToText.InactivityTimeout = -1;
                _speechToText.ProfanityFilter = false;
                _speechToText.SmartFormatting = true;
                _speechToText.SpeakerLabels = false;
                _speechToText.WordAlternativesThreshold = null;
                _speechToText.Keywords = keywords;          
                _speechToText.KeywordsThreshold = keywordConfidence;
                _speechToText.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _speechToText.IsListening)
            {
                _speechToText.StopListening();
            }
        }
    }

    private void StartRecording()
    {
        if (_recordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            _recordingRoutine = Runnable.Run(RecordingHandler());
        }
    }

    private void StopRecording()
    {
        if (_recordingRoutine != 0)
        {
            Microphone.End(_microphoneID);
            Runnable.Stop(_recordingRoutine);
            _recordingRoutine = 0;
        }
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {
        if (DebugMode)
        {
            Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
        }
        _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
        yield return null;      // let _recordingRoutine get set..

        if (_recording == null)
        {
            StopRecording();
            yield break;
        }

        bool bFirstBlock = true;
        int midPoint = _recording.samples / 2;
        float[] samples = null;

        while (_recordingRoutine != 0 && _recording != null)
        {
            int writePos = Microphone.GetPosition(_microphoneID);
            if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
            {
                if (DebugMode)
                {
                    Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");
                }

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
              || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
				record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                record.Clip.SetData(samples, 0);

                _speechToText.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)_recordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }
        }

        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    if (DebugMode)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", text);
                    }

                    ResultsField.text = text;
                }

                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        if (DebugMode)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                        }

                        ResultsField.text = "Keyword Recognised: " + keyword.normalized_text;

                        //Do things based on keyword
                        if (keyword.normalized_text.Equals("start"))
                        {
                            //start game function call here
                            Debug.Log("Start recognised - function called!");
                        }
                        if (keyword.normalized_text.Equals("quit"))
                        {
                            //end game function here
                            Debug.Log("Quit game recognised - function called!");
                        }
                        if (keyword.normalized_text.Equals("test"))
                        {
                            //test game function here
                            Debug.Log("Test recognised - function called!");
                        }
                    }
                }

                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        if (DebugMode)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                            foreach (var alternative in wordAlternative.alternatives)
                                Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                        }
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null && DebugMode)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognize()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }
}
