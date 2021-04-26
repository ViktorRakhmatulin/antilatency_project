using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Security;
using System;

using Antilatency.Alt.Tracking;

public class tcptoPython2devices : MonoBehaviour
{
    public string IP = "127.0.0.1"; //
    public int Port = 20;
    public byte[] dane;
    public Socket client;

    public float rx1;
    public float ry1;
    public float rz1;
    public float x1;
    public float y1;
    public float z1;

    public float rx2;
    public float ry2;
    public float rz2;
    public float x2;
    public float y2;
    public float z2;

    // Use this for initialization
    public void Changing()
    {
        //GameObject.Find("RightHandBracer").
        //GameObject.Find("RightHandBracer").

        dane = System.Text.Encoding.ASCII.GetBytes("[" +
            GameObject.Find("RightHandBracer").transform.localToWorldMatrix.ToString() + ";" +
            GameObject.Find("LeftHandBracer").transform.localToWorldMatrix.ToString()+ "]");

        //dane = System.Text.Encoding.ASCII.GetBytes(x0 + ";" + y0 + ";" + z0 + ";" + rx + ";" + ry + ";" + rz + ";" + x0b + ";" + y0b + ";" + z0b + ";" + rxb + ";" + ryb + ";" + rzb);//encode string  data into byte for sending 

        client.Send(dane);
        //Debug.Log(Buffer.ByteLength(dane));
        //for (int i = 0; i < 10000000 ; i++)
        //{
        //    int a = 2; int b = 3;
        //    a++; b++;

        //}
    }


    void Start()
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//
        client.Connect(IP, Port);//connecting port with ip address 
}

    void Update()
    {
        Changing();
    }
}
