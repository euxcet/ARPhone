using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class Net : MonoBehaviour {

    private Socket server;
    private TcpListener listener;
    private TcpClient clientSocket;
    private NetworkStream stream;
    private int done = 0;

	// Use this for initialization
	void Start () {
        IPAddress address = IPAddress.Parse("192.168.1.49");
        int port = 2000;
        listener = new TcpListener(address, port);
        listener.Start();

        Thread connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnect()
    {
        if (clientSocket != null)
            clientSocket.Close();
        clientSocket = listener.AcceptTcpClient();
        done = 1;
    }

    void SocketReceive()
    {
        SocketConnect();
        stream = clientSocket.GetStream();
        byte[] temp = Encoding.UTF8.GetBytes("Test");
        stream.Write(temp, 0, temp.Length);
        byte[] data = new byte[256];
        string responseData = string.Empty;
        int bytes = stream.Read(data, 0, data.Length);
        responseData = Encoding.UTF8.GetString(data, 0, bytes);
        Debug.Log(responseData);
    }

    void OnDestroy()
    {
        if (listener != null)
        {
            listener.Stop();
        }
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (done == 1)
        {
            Sprite newSprite = Resources.Load<Sprite>("3");
            GameObject.Find("ImageContainer").GetComponent<SpriteRenderer>().sprite = newSprite;
            Debug.Log(newSprite.bounds.size);
            GameObject.Find("ImageContainer").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1 / newSprite.bounds.size.x, 1/ newSprite.bounds.size.y, 1);
        }
    }
}
