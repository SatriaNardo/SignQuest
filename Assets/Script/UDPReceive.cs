using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UDPReceive : MonoBehaviour
{
    public static UDPReceive Instance { get; set; }
    Thread receiveThread;
    UdpClient client; 
    public int port = 1594;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;
    public TextMeshProUGUI receivedSign;
    private string latestReceivedData = "";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
       
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the TextMeshProUGUI by tag, name, or a custom method
        receivedSign = GameObject.FindWithTag("ReceivedSign")?.GetComponent<TextMeshProUGUI>();

        // Optional: log or warn if not found
        if (receivedSign == null)
        {
            Debug.LogWarning("receivedSign not found in scene!");
        }
    }
    public void Start()
    {

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void Update()
    {
        // Only update UI if new data has been received
        if (!string.IsNullOrEmpty(latestReceivedData))
        {
            if(receivedSign != null)
            {
                receivedSign.text = latestReceivedData;
                latestReceivedData = ""; // Clear it so we don't repeat
            }
        }
    }


    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);
                latestReceivedData = data;
                if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

}
