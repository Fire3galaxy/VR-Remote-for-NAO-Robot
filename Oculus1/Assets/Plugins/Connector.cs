// This is the client DLL class code to use for the sockServer
// include this DLL in your Plugins folder under Assets
// using it is very simple
// Look at LinkSyncSCR.cs


using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;

namespace SharpConnect {
    public class Connector {
        private const int READ_BUFFER_SIZE = 921604; // Size of "IMG|" + a 640x480 RGB image
        private const int IMG_SIZE = READ_BUFFER_SIZE - 4;
        private const int PORT_NUM = 10000;
        private static readonly string[] ACCEPTED_COMMANDS = new string[] {"IMG"};
        private const int MAX_COMMAND_LENGTH = 3; // Update this for longer commands

        private TcpClient client;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
        private byte[] imgBuffer = new byte[IMG_SIZE]; // Size of 640x480 image only
        private int imgByteOffset = 0;
        private enum ReceivingState {IMG, NONE};
        private ReceivingState state = ReceivingState.NONE;

        public string res = String.Empty;
        public bool isConnected = false;

        public Connector() { }

        public string fnConnectResult(string sNetIP, int iPORT_NUM) {
            try {
                // The TcpClient is a subclass of Socket, providing higher level 
                // functionality like streaming.
                client = new TcpClient(sNetIP, PORT_NUM);
                // Start an asynchronous read invoking DoRead to avoid lagging the user
                // interface.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                isConnected = true;

                return "Connection Succeeded";
            } catch (Exception ex) {
                isConnected = false;
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }

        public void fnPacketTest(string sInfo) {
            SendData(sInfo);
        }

        public void fnDisconnect() {
            isConnected = false;
            SendData("DISCONNECT");
        }

        // Function that actually constantly receives stream from server in background
        private void DoRead(IAsyncResult ar) {
            int BytesRead;
            try {
                // Finish asynchronous read into readBuffer and return number of bytes read.
                BytesRead = client.GetStream().EndRead(ar);
                if (BytesRead < 1) {
                    // if no bytes were read server has close.  
                    res = "Disconnected";
                    return;
                }

                // Get type of command (must be from accepted commands)
                string messageType = string.Empty;
                for (int i = 0; i < MAX_COMMAND_LENGTH; i++) {
                    messageType += Convert.ToChar(readBuffer[i]);
                }

                // Validate that command is acceptable type
                if (!ValidateCommand(messageType))
                    messageType = string.Empty;

                // Process message (if empty, assume packet continues from previous message type)
                ProcessCommands(messageType, BytesRead);

                // Start a new asynchronous read into readBuffer.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
            } catch {
                res = "Disconnected";
            }
        }

        private bool ValidateCommand(string messageType) {
            foreach (string s in ACCEPTED_COMMANDS)
                if (s == messageType)
                    return true;
            return false;
        }
        
        private void ProcessCommands(string messageType, int BytesRead) {
            switch (messageType) {
                case "IMG":
                    Debug.Assert(state == ReceivingState.NONE, "Connector.cs: state should be NONE, was " + state.ToString() + ", bytes read: " + BytesRead);
                    state = ReceivingState.IMG;

                    // If packet came with "IMG|" + data, handle below
                    if (BytesRead > 4) {
                        for (int i = 4; i < BytesRead; i++) {
                            imgBuffer[imgByteOffset + i - 4] = readBuffer[i];
                        }
                        imgByteOffset += (BytesRead - 4);

                        if (imgByteOffset >= IMG_SIZE) {
                            imgByteOffset = 0;
                            state = ReceivingState.NONE;
                        }
                    }
                    break;
                case "": // string.Empty
                    if (state == ReceivingState.IMG) {
                        for (int i = 0; i < BytesRead; i++) {
                            imgBuffer[imgByteOffset + i] = readBuffer[i];
                        }
                        imgByteOffset += BytesRead;

                        if (imgByteOffset >= IMG_SIZE) {
                            imgByteOffset = 0;
                            state = ReceivingState.NONE;
                        }
                    }
                    break;
                default:
                    Debug.Assert(false, "Connector.cs: messageType (" + messageType + ") was not valid");
                    break;
            }
        }

        // Reads image buffer and returns Texture2D
        public Texture2D GetImageTexture () {
            if (state != ReceivingState.IMG) {
                Texture2D tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
                tex.LoadRawTextureData(imgBuffer);
                tex.Apply(false);

                return tex;
            } else {
                return null;
            }
        }

        // Use a StreamWriter to send a message to server.
        private void SendData(string data) {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.Write(data + (char)10);
            writer.Flush();
        }
    }
}