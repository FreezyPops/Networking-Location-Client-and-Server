using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace location
{
    //localhost
    //whois.net.dcs.hull.ac.uk
    public class Whois
    {
        static bool usingGUI = false;
        private static string[] mArrayFromGUI;       

        public static void setStringFromGUI(string[] pArray)
        {
            mArrayFromGUI = pArray;            
        }

        static void Main(string[] args)
        {
            //if no arguments are supplied, client input form is generated, else, continues using the console
            if (args.Count() == 0)
            {
                location.InputForm inputForm = new location.InputForm();
                Application.EnableVisualStyles();
                Application.Run(inputForm);
                usingGUI = true;
                commandLineProcessing(mArrayFromGUI);
            }
            else
            {
                commandLineProcessing(args);
            }
        }

        static void commandLineProcessing(string[] args)
        {
            try
            {
                bool breakBool = false;
                int portNumber = 43;
                string protocolChoice = "whois";
                string address = "localhost";
                int timeout = 1000; 
                string errorType = "";

                for (int i = 0; i < args.Length; i++)
                {

                    if (breakBool == true)
                    {
                        break;
                    }
                    //Iterates through each argument in turn to determine specified commands in the whole commnd line
                    switch (args[i])
                    {
                        case "-p":              //port number check
                            bool parseCheck;
                            int parsedInt = 0;
                            if (parseCheck = int.TryParse(args[i + 1], out parsedInt))
                            {
                                args[i] = "0";
                                args[i + 1] = "0";
                                portNumber = parsedInt;
                                break;
                            }
                            else
                            {
                                breakBool = true;
                                errorType = "-p";
                                break;
                            }

                        case "-t":              //timeout check                   
                            parsedInt = 0;                           
                            if (parseCheck = int.TryParse(args[i + 1], out parsedInt))
                            {
                                args[i] = "0";
                                args[i + 1] = "0";
                                timeout = parsedInt;
                                break;
                            }
                            else
                            {
                                breakBool = true;
                                errorType = "-t";
                                break;
                            }

                        case "-h":              //address check
                            args[i] = "0";
                            address = args[i + 1];
                            args[i + 1] = "0";
                            break;

                        case "-h0":                     //protocol checks
                            protocolChoice = "-h0";
                            break;

                        case "-h1":
                            protocolChoice = "-h1";
                            break;

                        case "-h9":
                            protocolChoice = "-h9";
                            break;
                    }
                }

                if (breakBool == true)      //during int parsing previously, if exception caught pass error type
                {                           //and diaplay error message based on incorect data
                    switch (errorType)
                    {
                        case "-t":
                            Console.WriteLine("Error - Timeout entered must be a number");
                            break;

                        case "-p":
                            Console.WriteLine("Error - Port number must be a number");
                            break;

                        default:
                            Console.WriteLine("Invalid input command: check command rules and retry command");
                            break;
                    }                    
                    // Console.ReadKey();
                }
                else
                {
                    switch (protocolChoice) //protocol choice determines whether to use http or who is argument builders
                    {
                        case "-h0":
                            httpArgumentBuilder(args, portNumber, address, protocolChoice, timeout);
                            break;

                        case "-h1":
                            httpArgumentBuilder(args, portNumber, address, protocolChoice, timeout);
                            break;

                        case "-h9":
                            httpArgumentBuilder(args, portNumber, address, protocolChoice, timeout);
                            break;

                        case "whois":
                            whoIsArgumentBuilder(args, portNumber, address, timeout);
                            break;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void whoIsArgumentBuilder(string[] pArgs, int pPortNumber, string pAddress, int pTimeout)
        {
            List<string> newArgsList = new List<string>();
            for (int i = 0; i < pArgs.Length; i++)          //creates new list, removing 0's from pArgs as they are not needed 
            {
                if (pArgs[i] == "0")
                {
                    continue;
                }
                else
                {
                    newArgsList.Add(pArgs[i]);
                }
            }
            string[] args = newArgsList.ToArray();
            whoIsProtocol(args, pPortNumber, pAddress, pTimeout); //start whois protocol with new args set
        }

        static void whoIsProtocol(string[] pArgs, int pPortNumber, string pAddress, int pTimeout)
        {
            TcpClient client = new TcpClient();
            if (pTimeout != 0)
            {
                client.ReceiveTimeout = pTimeout;
                client.SendTimeout = pTimeout;
            }
            client.Connect(pAddress, pPortNumber);            

            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());
            if (pArgs.Length == 0)
            {
                Console.WriteLine("No Arguments supplied");
            }
            else
            {
                if (pArgs.Length == 1) //if one argument supplied then try fetch location sequence
                {
                    try
                    {
                        sw.Write(pArgs[0] + "\r\n");
                        sw.Flush();
                        string srAnswer = sr.ReadLine();
                        if (srAnswer.Contains("ERROR: no entries found"))
                        {
                            if (usingGUI == true) //determines whether GUI is being used or not for server output
                            {
                                serverResponseWindowHandler("ERROR: no entries found");
                            }
                            else
                            {
                                Console.WriteLine("ERROR: no entries found");
                            }
                            //     Console.ReadKey();
                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " is " + srAnswer + "\r\n");
                            }
                            else
                            {
                                Console.Write(pArgs[0] + " is " + srAnswer + "\r\n");
                            }
                            //   Console.ReadKey();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        //  Console.ReadKey();
                    }
                }
                else if (pArgs.Length > 1) //if name and location arguments supplied
                {
                    try
                    {
                        if (pArgs.Length > 2) //if location arguments include spaces, use string builder to make one location argument
                        {
                            StringBuilder tempArgsStringBuilder = new StringBuilder();
                            for (int i = 1; i < pArgs.Count(); i++)
                            {
                                tempArgsStringBuilder.Append(pArgs[i]);
                                tempArgsStringBuilder.Append(" ");
                            }
                            pArgs[1] = tempArgsStringBuilder.ToString().Trim();
                        }

                        sw.Write(pArgs[0] + " " + pArgs[1] + "\r\n");
                        sw.Flush();
                        string srAnswer = sr.ReadLine();
                        if (srAnswer.Contains("OK"))
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                            else
                            {
                                Console.Write(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(srAnswer);
                            }
                            else
                            {
                                Console.Write(srAnswer);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        // Console.ReadKey();
                    }
                }
            }
        }

        static void httpArgumentBuilder(string[] pArgs, int pPortNumber, string pAddress, string pProtocolChoice, int pTimeout)
        {
            char commandType;

            List<string> newArgsList = new List<string>();
            for (int i = 0; i < pArgs.Length; i++) //iterate pArgs to determine protocol type and remove empty indexes
            {
                if (pArgs[i] == "0")
                {
                    continue;
                }
                if (pArgs[i] == "")
                {
                    continue;
                }
                if (pArgs[i] == "-h9")
                {
                    continue;
                }
                if (pArgs[i] == "-h0")
                {
                    continue;
                }
                if (pArgs[i] == "-h1")
                {
                    continue;
                }
                else
                {
                    newArgsList.Add(pArgs[i]);
                }
            }
            try
            {
                if (newArgsList.Count == 0)
                {
                    Console.WriteLine("Error: No arguments supplied");
                    //Console.ReadKey();
                }
                else if (newArgsList.Count == 1) 
                {
                    commandType = 'G';
                    string[] args = newArgsList.ToArray();
                    switch (pProtocolChoice)
                    {
                        case "-h9":
                            http09Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h0":
                            http10Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h1":
                            http11Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;
                    }
                }
                else if (newArgsList.Count == 2) //if two arguments assign PUT/POST command type 
                {
                    commandType = 'P';
                    string[] args = newArgsList.ToArray();
                    switch (pProtocolChoice)
                    {
                        case "-h9":
                            http09Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h0":
                            http10Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h1":
                            http11Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;
                    }
                }
                else if (newArgsList.Count >= 3) //if location arguments include spaces, use string builder to make one location argument
                {
                    StringBuilder tempArgsStringBuilder = new StringBuilder();
                    for (int i = 1; i < newArgsList.Count; i++)
                    {
                        tempArgsStringBuilder.Append(newArgsList[i].ToString());
                        tempArgsStringBuilder.Append(" ");
                    }
                    string argsFullString = tempArgsStringBuilder.ToString();
                    argsFullString.Trim();

                    List<string> argsSwapList = new List<string>();
                    argsSwapList.Add(newArgsList[0]);
                    argsSwapList.Add(argsFullString);
                    string[] args = argsSwapList.ToArray();

                    commandType = 'P';
                    switch (pProtocolChoice)
                    {
                        case "-h9":
                            http09Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h0":
                            http10Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;

                        case "-h1":
                            http11Protocol(args, pPortNumber, pAddress, commandType, pTimeout);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Console.ReadKey();
            }
        }

        static void http09Protocol(string[] pArgs, int pPortNumber, string pAddress, char pCommandType, int pTimeout)
        {
            TcpClient client = new TcpClient();
            client.Connect(pAddress, pPortNumber);
            if (pTimeout != 0)
            {
                client.ReceiveTimeout = pTimeout;
                client.SendTimeout = pTimeout;
            }            

            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            switch (pCommandType) //switch determines which command to carry out based upon parsed command type
            {
                case 'G':
                    try
                    {
                        sw.Write("GET /" + pArgs[0] + "\r\n");
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/0.9 200 OK"))
                        {
                            string[] lines = srAnswer.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " is " + lines[3] + "\r\n");
                            }

                            else
                            {
                                Console.WriteLine(pArgs[0] + " is " + lines[3] + "\r\n");
                            }
                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(sr.ReadToEnd());
                            }
                            else
                            {
                                Console.WriteLine(sr.ReadToEnd());
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        // Console.ReadKey();
                    }
                    break;

                case 'P':
                    try
                    {
                        string temp = "PUT" + " " + "/" + pArgs[0] + "\r\n" + "\r\n" + pArgs[1] + "\r\n";
                        Console.WriteLine(temp);
                        sw.Write("PUT" + " " + "/" + pArgs[0] + "\r\n" + "\r\n" + pArgs[1] + "\r\n");
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n"))
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                            else
                            {
                                Console.WriteLine(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                            //Console.ReadKey();
                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(srAnswer);
                            }
                            else
                            {
                                Console.WriteLine(srAnswer);
                            }
                            //Console.ReadKey();
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
            }

        }

        static void http10Protocol(string[] pArgs, int pPortNumber, string pAddress, char pCommandType, int pTimeout)
        {
            TcpClient client = new TcpClient();
            client.Connect(pAddress, pPortNumber);
            if (pTimeout != 0)
            {
                client.ReceiveTimeout = pTimeout;
                client.SendTimeout = pTimeout;
            }            

            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            switch (pCommandType)
            {
                case 'G':
                    try
                    {
                        sw.Write("GET /?" + pArgs[0] + " " + "HTTP/1.0" + "\r\n" + "\r\n");
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/1.0 200 OK"))
                        {
                            string[] lines = srAnswer.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " is " + lines[3] + "\r\n");
                            }
                            else
                            {
                                Console.WriteLine(pArgs[0] + " is " + lines[3] + "\r\n");
                            }

                            // Console.ReadKey();
                        }
                        Console.WriteLine(sr.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 'P':
                    try
                    {
                        int locationLength = pArgs[1].Length;
                        sw.WriteLine("POST /" + pArgs[0] + " HTTP/1.0\r\n" + "Content-Length: " + locationLength + "\r\n" + "\r\n" + pArgs[1]);
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n"))
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                            else
                            {
                                Console.WriteLine(pArgs[0] + " location changed to be " + pArgs[1]);
                            }

                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(srAnswer);
                            }
                            else
                            {
                                Console.WriteLine(srAnswer);
                            }
                            //  Console.ReadKey();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
            }

        }

        static void http11Protocol(string[] pArgs, int pPortNumber, string pAddress, char pCommandType, int pTimeout)
        {
            TcpClient client = new TcpClient();
            client.Connect(pAddress, pPortNumber);
            if (pTimeout != 0)
            {
                client.ReceiveTimeout = pTimeout;
                client.SendTimeout = pTimeout;
            }            

            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            switch (pCommandType)
            {
                case 'G':
                    try
                    {
                        sw.Write("GET /?name=" + pArgs[0] + " " + "HTTP/1.1" + "\r\n" + "Host: " + pAddress + "\r\n" + "\r\n" + "\r\n");
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n"))
                        {
                            string[] lines = srAnswer.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " is " + lines[4] + "\r\n");
                            }
                            else
                            {
                                Console.WriteLine(pArgs[0] + " is " + lines[4] + "\r\n");
                            }

                            //Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine(srAnswer);
                            // Console.ReadKey();
                        }
                        Console.WriteLine(sr.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 'P':
                    try
                    {
                        int contentLength = 15 + pArgs[0].Length + pArgs[1].Length;
                        string streamWriteString = "POST / HTTP/1.1" + "\r\n" + "Host: " + pAddress + "\r\n" + "Content-Length: " + contentLength + "\r\n" + "\r\n" + "name=" + pArgs[0] + "&location=" + pArgs[1];
                        sw.WriteLine(streamWriteString);
                        sw.Flush();

                        string srAnswer = sr.ReadToEnd();
                        if (srAnswer.Contains("HTTP/1.1 200 OK" + "\r\n" + "Contet-Type: " + "text/plain" + "\r\n" + "\r\n"))
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(pArgs[0] + " location changed to be " + pArgs[1]);
                            }
                            else
                            {
                                Console.WriteLine(pArgs[0] + " location changed to be " + pArgs[1]);
                            }

                        }
                        else
                        {
                            if (usingGUI == true)
                            {
                                serverResponseWindowHandler(srAnswer);
                            }
                            else
                            {
                                Console.WriteLine(srAnswer);
                            }

                            // Console.ReadKey();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
            }
        }

        static void serverResponseWindowHandler(string pServerResponse)
        {
            location.ServerOutputForm serverOutputForm = new location.ServerOutputForm(pServerResponse);
            Application.EnableVisualStyles();
            Application.Run(serverOutputForm);
        }
    }
}


