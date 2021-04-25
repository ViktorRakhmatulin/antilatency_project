# -*- coding: utf-8 -*-
"""
Created on Sun Apr 25 14:03:50 2021

@author: vikt-
"""
import time,serial
import  socket
import urx
import serial.tools.list_ports
import sys
import numpy as np

def initialize_hap(port = "COM4",baudrate=115200, timeout=0.1):
    try:
        hap = serial.Serial(port = port, baudrate=baudrate, timeout = timeout)
        return hap
    except serial.SerialException:
       # hap = serial.Serial(port, baudrate, timeout)
        #hap.close()
    
        print("No connected to hap")
    
    return None

def list_available_com_ports():
    list = serial.tools.list_ports.comports()
    connected = []
    for element in list:
        connected.append(element.device)
    print("Connected COM ports: " + str(connected))
    if (sys.platform.startswith('win')):
    # !attention assumes pyserial 3.x
        ports = ['COM%s' % (i + 1) for i in range(256)]
    else:
        raise EnvironmentError('Unsupported platform')
    
    result = []
    for port in ports:
        try:
            s = serial.Serial(port)
            s.close()
            result.append(port)
        except (OSError, serial.SerialException):
            pass
    print("Availible COM Ports: " + str(result))
    
def hap_action(hap):
    x1=20
    y1=-14
    v1 = 0
    v2 = 0
    x3= 100
    y3= 0
    c = 0

    while (c < 30):
        c += 2
        hap.write(bytes(str(x1) + "," + str(y1)+ "," + str(v1)+ "," + str(v2)+ "," + str(x3)+ "," + str(y3)+ "\n",'utf-8'))
        time.sleep(0.1)

    x1= 35
    y1= 10
    x2= 0
    y2= 0
    x3= 0
    y3= 0

    hap.write(bytes(str(x1) + "," + str(y1)+ "," + str(x2)+ "," + str(y2)+ "," + str(x3)+ "," + str(y3)+ "\n",'utf-8'))
    print("hap function executed")
    
def initialize_al(port = 20,bind_ip = '127.0.0.1',backlog = 5):
    try:
        s = socket.socket()

        print('socket created ')
    
        s.bind((bind_ip, port)) #bind port with ip address
        print('socket binded to port ')
        s.listen(backlog)#listening for connection
        print('socket listensing ... ')
    
        c, addr = s.accept() #when port connected
        return c, addr
    except:  
        print('interrupted ')
        s.close()
        return None

def initialize_robot(robot_ip = "192.168.88.162"):
#    success = True
    try:
        rob = urx.Robot(robot_ip)
        return rob 
    except:
        print('robot error')
        return None

# preliminary rough calibration of tool
def get_al_2_robot_tf():
# shitty position of a robot base in AL. z is swapped 
    p_al = np.array([-1.13,1.10,0.41, 1])
    #tf2robot = np.array([[0,0,1,-p_al[2]],[1,0,0,-p_al[0]],[0,1,0,-p_al[1]],[0,0,0,1]],dtype = np.float32)
    
    p_tool_al = np.array([-0.66207,1.341,0.993, 1])
    p_tool_r = np.array([0.612870852821809, 0.4789758999604802, 0.5050407343318414,1])
    
    from_tool_to_al_origin_tf = np.array([[1,0,0,-p_tool_al[0]],[0,1,0,-p_tool_al[1]],[0,0,1,-p_tool_al[2]],[0,0,0,1]],dtype = np.float32)
    base_to_tool_tf = np.array([[1,0,0,p_tool_r[0]],[0,1,0,p_tool_r[1]],[0,0,1,p_tool_r[2]],[0,0,0,1]],dtype = np.float32)
    rotate_to_al_cs_tf = np.array([[0,0,1,0],[1,0,0,0],[0,1,0,0],[0,0,0,1]],dtype = np.float32)
    
    tf2robot = base_to_tool_tf @ rotate_to_al_cs_tf @ from_tool_to_al_origin_tf            
    print(tf2robot @ p_al)
    return tf2robot 

#def append_data_to_file(filename):
    