using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeapInputReciever : MonoBehaviour
{
    private byte mChannelID;
    private int mSocketID;

    // Use this for initialization
    void Start()
    {
        NetworkTransport.Init(); // Initialize the low-level network transport API
        ConnectionConfig config = new ConnectionConfig();
        mChannelID = config.AddChannel(QosType.ReliableSequenced); // We use reliable sequenced QoS to ensure that packets are delivered and are processed in chronological order.
        HostTopology hostTopology = new HostTopology(config, 10);
        mSocketID = NetworkTransport.AddHost(hostTopology, 8888);
        //byte errorValue;
        // TODO : Error handling
    }

    // Update is called once per frame
    void Update()
    {
        int recieverSocketID;
        int recieverConnectionID;
        int recieverChannelID;
        byte[] recBuffer = new byte[512];
        int recBufferSize = 512;
        int readDataSize;
        byte readError;
        NetworkEventType netEvent = NetworkTransport.Receive(out recieverSocketID, out recieverConnectionID, out recieverChannelID, recBuffer, recBufferSize, out readDataSize, out readError);

        switch (netEvent)
        {
            case NetworkEventType.DataEvent:
                UIManager.Instance.DisplaySettingsScreen();
                break;
            case NetworkEventType.ConnectEvent:
                break;
            case NetworkEventType.DisconnectEvent:
                break;
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.BroadcastEvent:
                break;
            default:
                break;
        }
    }
}
