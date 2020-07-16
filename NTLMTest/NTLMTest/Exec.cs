using SMBMessageParser.Pipe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NTLMTest
{
    class Exec
    {
        volatile bool IsRun;
        PipeClient co;
        PipeClient ci;
        public Exec(string ipAddress, string userName, string password)
        {
            IsRun = true;
            co = new PipeClient(ipAddress, "PipeServerReceive", PipeDirection.Write, userName, password);
            ci = new PipeClient(ipAddress, "PipeServerSend", PipeDirection.Read, userName, password);
        }
        public void Start()
        {
            co.Connect();
            ci.Connect();
            Thread thread = new Thread(AAAA);
            thread.Start();
            Console.WriteLine("input command");
            while (true)
            {
                var strCmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(strCmd))
                {
                    continue;
                }
                if (strCmd == "exit")
                {
                    break;
                }
                var c = Encoding.UTF8.GetBytes(strCmd);
                List<byte> vs = new List<byte>();
                vs.AddRange(BitConverter.GetBytes(0x1122334455667788u));
                vs.AddRange(BitConverter.GetBytes(c.Length + 1));
                vs.Add(0);//type
                vs.AddRange(c);
                co.Write(vs.ToArray());
            }
            co.Write(new byte[] {
                0x88, 0x77, 0x66, 0x55,
                0x44, 0x33, 0x22, 0x11,
                0x01, 0x00, 0x00, 0x00,
                0x01});
            IsRun = false;
            co.Close();
            ;
        }
        private void AAAA()
        {
            while (IsRun)
            {
                try
                {
                    byte[] buff = ci.Read(12);
                    if (buff.Length != 12)
                    {
                        throw new Exception("Pipe Read Length Error");

                    }
                    ulong transportHeader = BitConverter.ToUInt64(buff, 0);
                    if (transportHeader != 0x1122334455667788u)
                    {
                        throw new Exception("Pipe Read TransportHeader Error");

                    }
                    uint transportLength = BitConverter.ToUInt32(buff, 8);

                    buff = ci.Read(transportLength);
                    if (buff.Length != transportLength)
                    {
                        throw new Exception("Pipe Read TransportLength Error");

                    }
                    var str = Encoding.UTF8.GetString(buff, 1, buff.Length - 1);
                    if (buff[0] == 1)
                    {
                        Console.Write(str);
                    }
                    else
                    {
                        Console.Write("Server failure:" + str);
                    }
                }
                catch { }
            }
            ci.Close();
        }
    }
}
