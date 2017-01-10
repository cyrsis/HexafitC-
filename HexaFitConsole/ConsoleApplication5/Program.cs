using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static int rbytesCount;
        static void Main(string[] args)
        {
            
            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 9600;
            serialPort.PortName = "COM7"; // Set in Windows
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
          
            serialPort.Open();
            for (int i = 0; i < 999999999; i++)
            {
                //Console.WriteLine("MessageProcessingHandler");
                rbytesCount = serialPort.BytesToRead;
                byte[] ReadBuffer = new byte[500];
               // Console.WriteLine("Bytes to read"+rbytesCount);
                serialPort.Read(ReadBuffer,0,rbytesCount);

                var Header0 = (Char)ReadBuffer[0];
               // Console.WriteLine("header"+Header0.ToString());
                var Header1 = (Char)ReadBuffer[1];
               var  Footer0 = (Char)ReadBuffer[38];
                var Footer1 = (Char)ReadBuffer[39];

                if (rbytesCount == 40 && Header0 == '*' && Header1 == '#' && Footer0 == '+' && Footer1 == '%' )
                {
                    var X_Acc = BitConverter.ToInt16(ReadBuffer, 5);
                    var Y_Acc = BitConverter.ToInt16(ReadBuffer, 7);
                    var Z_Acc = BitConverter.ToInt16(ReadBuffer, 9);
                    Console.WriteLine("X acc " + X_Acc);
                    Console.WriteLine("Y acc " + Y_Acc);
                    Console.WriteLine("Z acc " + Z_Acc+Environment.NewLine);

                    var Temperature = BitConverter.ToInt16(ReadBuffer, 11);

                    Console.WriteLine("Temperature " + Temperature);

                    var X_Gyro = BitConverter.ToInt16(ReadBuffer, 13);
                    Console.WriteLine("X Gyro "+ X_Gyro);

                    var Y_Gyro = BitConverter.ToInt16(ReadBuffer, 15);
                    Console.WriteLine("Y Gyro "+ Y_Gyro);

                    var Z_Gyro = BitConverter.ToInt16(ReadBuffer, 17);
                    Console.WriteLine("Z Gyro "+ Z_Gyro);


                }
            
                
                
                
                

                Thread.Sleep(40);
            }
            //Console.WriteLine(serialPort.BytesToRead);
            //serialPort.DataReceived += SerialPort_DataReceived;
            int counter = 0;
            Console.ReadLine();
            serialPort.Close();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            Console.WriteLine("Recieve something");
            SerialPort spL = (SerialPort)sender;
            byte[] buf = new byte[spL.BytesToRead];
            Console.WriteLine("DATA RECEIVED!");
            spL.Read(buf, 0, buf.Length);
            foreach (Byte b in buf)
            {
                Console.Write(b.ToString());
            }
            Console.WriteLine();
        }
    }
}
