using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace locationserver
{
    public class Whois
    {
        private static int[] mArrayFromGUI;
        static bool usingGUI = false;
        public static void setArrayFromGUI(int[] pArray)
        {
            mArrayFromGUI = pArray;
        }
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-w")
                {
                    usingGUI = true;
                    locationserver.ServerInputForm inputForm = new locationserver.ServerInputForm();
                    Application.EnableVisualStyles();
                    Application.Run(inputForm);
                    runServer(mArrayFromGUI);                    
                }
            }
            if (usingGUI == false)
            {
                runServer(null);
            }
        }

        static Dictionary<string, string> currentLocationData = new Dictionary<string, string>();

        static void runServer(int [] pArgs)
        {            
            TcpListener listener;
            Socket connection;
            Handler requestHandler;
            
            try
            {
                listener = new TcpListener(IPAddress.Any, 43);
                listener.Start();
                Console.WriteLine("runServer has started listening");
                Dictionary<string, string> currentLocationData = new Dictionary<string, string>();
                //if (pArgs.Count() != 0)
                //{
                //    for (int i = 0; i < pArgs.Length; i++)
                //    {
                //        switch (pArgs[i])
                //        {
                //            case "-f":
                //                currentLocationData = makeDictionary();
                //                break;

                //            case "-w":
                //                break;

                //            case "l":
                //                break;

                            
                //            default:
                //                break;
                //        }
                //    }                    
                //}
                while (true)
                {
                    connection = listener.AcceptSocket();
                    try
                    {
                        if (usingGUI == true)
                        {
                            connection.ReceiveTimeout = pArgs[0];
                            connection.SendTimeout = pArgs[0];
                        }
                        else
                        {
                            connection.ReceiveTimeout = 1000;
                            connection.SendTimeout = 1000;
                        }                        
                        requestHandler = new Handler();
                        
                        Thread t = new Thread(() => requestHandler.protocolDecider(connection));
                        t.Start();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to connect in 1000ms");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }

        //static Dictionary<string, string> makeDictionary()
        //{
        //    Dictionary<string, string> locationDictionary = new Dictionary<string, string>();

        //    using (var sr = new StreamReader("LocationData.txt"))
        //    {
        //        string line = null;

        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            locationDictionary.Add(line, sr.ReadLine());
        //        }
        //    }
        //    return locationDictionary;
        //}

        class Handler
        {
            public void protocolDecider(Socket connection)
            {
                NetworkStream socketStream;
                socketStream = new NetworkStream(connection);
                Console.WriteLine("Connection Recieved");
                try
                {
                    StreamReader sr = new StreamReader(socketStream);
                    StreamWriter sw = new StreamWriter(socketStream);
                    string protocolReturn;

                    string sections = "";
                    while (sr.Peek() != -1)
                    {
                        sections = sections + " " + sr.ReadLine();                        
                    }
                    sections = sections.Trim();
                    Console.WriteLine("Respond Recieved: " + sections);
                    string[] sectionsArray = sections.Split(new char[] { ' ', '/', '?', ':', '&', '=' });
                    List<string> sectionsList = sectionsArray.ToList<string>();

                    string commandType = "WHOIS";
                    string protocolType = "";

                    for (int i = 0; i < sectionsList.Count(); i++)
                    {
                        switch (sectionsList[i])
                        {
                            case "HTTP":
                                sectionsList[i] = "";
                                break;

                            case "GET":
                                commandType = "GET";
                                sectionsList[i] = "";
                                break;

                            case "PUT":
                                commandType = "PUT";
                                sectionsList[i] = "";
                                break;

                            case "POST":
                                commandType = "POST";
                                sectionsList[i] = "";
                                break;

                            case "1.0":
                                protocolType = "1.0";
                                sectionsList[i] = "";
                                break;

                            case "1.1":
                                protocolType = "1.1";
                                sectionsList[i] = "";
                                break;

                            case "Content-Length":
                                sectionsList[i] = "";
                                sectionsList[i + 2] = "";
                                break;

                            case "name":
                                sectionsList[i] = "";
                                break;

                            case "Host":
                                sectionsList[i] = "";
                                sectionsList[i + 2] = "";
                                break;

                            case "location":
                                sectionsList[i] = "";
                                break;
                        }
                    }

                    int blankCounter = 0;
                    for (int i = 0; i < sectionsList.Count(); i++)
                    {
                        if (sectionsList[i] == "")
                        {
                            blankCounter++;
                        }
                    }

                    if (blankCounter != 0)
                    {
                        for (int i = 0; i < blankCounter; i++)
                        {
                            sectionsList.Remove("");
                        }
                    }
                    sectionsList.Remove("HTTP");

                    sectionsArray = sectionsList.ToArray();

                    switch (commandType)
                    {
                        case "GET":

                            switch (protocolType)
                            {
                                case "1.0":
                                    protocolReturn = doHttp10Request(sectionsArray, commandType);
                                    sw.Write(protocolReturn);
                                    sw.Flush();
                                    break;

                                case "1.1":
                                    protocolReturn = doHttp11Request(sectionsArray, commandType);
                                    sw.Write(protocolReturn);
                                    sw.Flush();
                                    break;

                                default:
                                    protocolReturn = doHttp09Request(sectionsArray, commandType);
                                    sw.Write(protocolReturn);
                                    sw.Flush();
                                    break;
                            }
                            break;

                        case "PUT":
                            protocolReturn = doHttp09Request(sectionsArray, commandType);
                            sw.Write(protocolReturn);
                            sw.Flush();
                            break;

                        case "POST":
                            switch (protocolType)
                            {
                                case "1.0":
                                    protocolReturn = doHttp10Request(sectionsArray, commandType);
                                    sw.Write(protocolReturn);
                                    sw.Flush();
                                    break;

                                case "1.1":
                                    protocolReturn = doHttp11Request(sectionsArray, commandType);
                                    sw.Write(protocolReturn);
                                    sw.Flush();
                                    break;
                            }
                            break;

                        case "WHOIS":
                            protocolReturn = doRequestWhoIs(sections);
                            sw.Write(protocolReturn);
                            sw.Flush();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    socketStream.Close();
                    connection.Close();
                }
            }
        }

        static string doRequestWhoIs(string socketStreamString)
        {
            Console.WriteLine("WhoIs request started");

            try
            {
                Console.WriteLine("Request recieved: " + socketStreamString);
                string[] lineSections = socketStreamString.Split(new char[] { ' ' });
                string lineOne;

                if (lineSections.Length == 0)
                {
                    return ("Nothing Entered");
                }

                lineOne = lineSections[0];

                if (lineSections.Length == 1)
                {
                    Console.WriteLine(lineSections[0] + " recieved");
                    if (currentLocationData.ContainsKey(lineOne) == true)
                    {
                        Console.WriteLine("Location for " + lineOne + " is: " + currentLocationData[lineOne]);
                        return (currentLocationData[lineOne]);
                    }
                    else
                    {
                        Console.WriteLine("ERROR: no entries found");
                        return ("ERROR: no entries found\r\n");
                    }
                }
                else if (lineSections.Length > 1)
                {
                    if (lineSections.Length > 2)
                    {                        
                        lineSections = stringBuilder(lineSections);
                    }
                    string lineTwo = lineSections[1].Trim();

                    Console.WriteLine(lineSections[0] + " and " + lineSections[1] + " recieved");
                    if (currentLocationData.ContainsKey(lineOne) == true)
                    {
                        currentLocationData[lineOne] = lineTwo;
                        Console.WriteLine("Location data for " + lineOne + " changed to " + lineTwo);
                        return ("OK\r\n");
                    }
                    else
                    {
                        currentLocationData.Add(lineOne, lineTwo);
                        return ("OK\r\n");
                    }
                }
                else
                {
                    return ("-1");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (e.ToString());
            }
        }

        static string doHttp09Request(string[] pArgs, string pCommandType)
        {
            Console.WriteLine("Http0.9 request started");           

            try
            {
                if (pCommandType == "GET")
                {
                    Console.WriteLine("GET section started");
                    if (currentLocationData.ContainsKey(pArgs[0]))
                    {
                        Console.WriteLine(currentLocationData[pArgs[0]] + " found");
                        return ("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n" + currentLocationData[pArgs[0]] + "\r\n");
                    }
                    else
                    {
                        Console.WriteLine(pArgs[0] + " not found in the server database");
                        return ("HTTP/0.9 404 Not Found\r\nContent-Type: text/plain\r\n\r\n");
                    }
                }
                else if (pCommandType == "PUT")
                {
                    Console.WriteLine("PUT section started");
                    if (pArgs.Count() > 2)
                    {
                        pArgs = stringBuilder(pArgs);
                    }
                    if (currentLocationData.ContainsKey(pArgs[0]))
                    {
                        currentLocationData[pArgs[0]] = pArgs[1];
                        Console.WriteLine(pArgs[0] + " location changed to " + pArgs[1]);
                        return ("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n");
                    }
                    else
                    {
                        currentLocationData.Add(pArgs[0], pArgs[1]);
                        Console.WriteLine(pArgs[0] + " added with location " + pArgs[1]);
                        return ("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n");
                    }
                }
                else
                {
                    return ("-1");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ("-1");
            }
        }

        static string[] stringBuilder(string [] pArgs)
        {
            StringBuilder tempArgsStringBuilder = new StringBuilder();
            List<string> newArgsList = new List<string>();
            newArgsList.Add(pArgs[0]);
            for (int i = 1; i < pArgs.Length; i++)
            {
                tempArgsStringBuilder.Append(pArgs[i]);
                tempArgsStringBuilder.Append(" ");
            }
            newArgsList.Add(tempArgsStringBuilder.ToString().Trim());
            string[] returnArray = newArgsList.ToArray();
            return returnArray;
        }

        static string doHttp10Request(string[] pArgs, string pCommandType)
        {
            Console.WriteLine("Http1.0 request started");
            try
            {
                switch (pCommandType)
                {
                    case "GET":
                        Console.WriteLine("GET section started");
                        if (currentLocationData.ContainsKey(pArgs[0]))
                        {
                            Console.WriteLine(currentLocationData[pArgs[0]] + " found");
                            return ("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n" + currentLocationData[pArgs[0]] + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine(pArgs[0] + " not found");
                            return ("HTTP/1.0 404 Not Found\r\nContent-Type: text/plain\r\n\r\n");
                        }

                    case "POST":
                        Console.WriteLine("POST section started");
                        if (pArgs.Count() > 2)
                        {
                            pArgs = stringBuilder(pArgs);
                        }
                        if (currentLocationData.ContainsKey(pArgs[0]))
                        {
                            currentLocationData[pArgs[0]] = pArgs[1];
                            Console.WriteLine(pArgs[0] + " location changed to " + pArgs[1]);
                            return ("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n");
                        }
                        else
                        {
                            currentLocationData.Add(pArgs[0], pArgs[1]);
                            Console.WriteLine(pArgs[0] + " added with location " + pArgs[1]);
                            return ("HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n");
                        }

                    default:
                        return ("-1");                        
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ("-1");
            }
        }

        static string doHttp11Request(string[] pArgs, string pCommandType)
        {
            Console.WriteLine("Http 1.1 request started");

            try
            {
                switch (pCommandType)
                {                    
                    case "GET":
                        Console.WriteLine("GET section started");
                        if (currentLocationData.ContainsKey(pArgs[0]))
                        {
                            Console.WriteLine(currentLocationData[pArgs[0]] + " found");
                            return ("HTTP/1.1 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n" + "\r\n" + "\r\n" + currentLocationData[pArgs[0]] + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine(pArgs[0] + " not found");
                            return ("HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n\r\n");
                        }                        

                    case "POST":
                        Console.WriteLine("POST section started");
                        if (pArgs.Count() > 2)
                        {
                            pArgs = stringBuilder(pArgs);
                        }
                        if (currentLocationData.ContainsKey(pArgs[0]))
                        {
                            Console.WriteLine(currentLocationData[pArgs[0]] + " found");
                            return ("HTTP / 1.1 200 OK\r\nContent-Type: text / plain" + "\r\n" + "\r\n" + "\r\n" + currentLocationData[pArgs[0]] + "\r\n");
                        }
                        else
                        {
                            currentLocationData.Add(pArgs[0], pArgs[1]);
                            Console.WriteLine(pArgs[0] + " added with location " + pArgs[1]);
                            return ("HTTP/1.1 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n" + "\r\n" + "\r\n");
                        }
                    default:
                        return ("-1");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ("-1");
            }
        }
    }
}
