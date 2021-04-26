using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Security;
using System;

public class tcptoPython : MonoBehaviour
{
    public string IP = "127.0.0.1"; //
    public int Port = 1234;
    public byte[] dane;
    public Socket client;
    public float rx1;
    public float ry1;
    public float rz1;
    public float x1;
    public float y1;
    public float z1;

    public Transform cam;
    public Vector3 cameraRelative;

    // Use this for initialization
    public void Changing()
    {


      

        //x1 = GameObject.Find("Camera").transform.position.z;
        rx1 = Camera.main.transform.rotation.eulerAngles.x;
        ry1 = Camera.main.transform.rotation.eulerAngles.y;
        rz1 = Camera.main.transform.rotation.eulerAngles.z;
        x1 = Camera.main.transform.position.x;
        y1 = Camera.main.transform.position.y;
        z1 = Camera.main.transform.position.z;


        int x = (int)x1;

        string rx = rx1.ToString();
        string ry = ry1.ToString();
        string rz = rz1.ToString();
        string x0 = x1.ToString();
        string y0 = y1.ToString();
        string z0 = z1.ToString();

        dane = System.Text.Encoding.ASCII.GetBytes(x0 + ";" + y0 + ";" + z0 + ";" + rx + ";" + ry + ";" + rz  );//encode string  data into byte for sending 
        client.Send(dane);//send data to port 
        byte[] b = new byte[1024];

        ////////////////////////////////////////////////////////
        //dane1 = System.Text.Encoding.ASCII.GetBytes(x0);//encode string  data into byte for sending 
        //dane2 = System.Text.Encoding.ASCII.GetBytes(y0);//encode string  data into byte for sending 
        //dane3 = System.Text.Encoding.ASCII.GetBytes(z0);//encode string  data into byte for sending 
        //client.Send(dane1);//send data to port 
        //client.Send(dane2);//send data to port 
        //client.Send(dane3);//send data to port 
        //byte[] b = new byte[1024];

        ////////////////////////////////////////

        

    }
    void Start()
    {
        //calling that function of sending and reciveing data 
        //Changing();
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(IP, Port);//connecting port with ip address 

    }

    void Update()
    {
        //if u want to chang again again so fast then cal that function herer
        Changing();

    }
}
