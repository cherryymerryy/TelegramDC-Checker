using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main()
    {
        while (true)
        {
            string[] dcServers = new string[] {
                "149.154.175.50:443", // DC1
                "149.154.167.50:443",// DC2
                "149.154.175.100:443",// DC3
                "149.154.167.91:443",// DC4
                "91.108.56.100:443" // DC5
            };

            int _serverIndex = 1;

            foreach (string dcServer in dcServers)
            {
                Console.WriteLine($"Checking DC{_serverIndex}...");

                Stopwatch stopwatch = Stopwatch.StartNew();

                try
                {
                    TcpClient client = new TcpClient();

                    var parts = dcServer.Split(':');
                    var ipAddress = IPAddress.Parse(parts[0]);
                    var port = int.Parse(parts[1]);

                    var endPoint = new IPEndPoint(ipAddress, port);

                    client.Connect(endPoint);

                    Console.ForegroundColor = stopwatch.ElapsedMilliseconds switch
                    {
                        > 150 => ConsoleColor.Red,
                        > 70 => ConsoleColor.Yellow,
                        < 70 => ConsoleColor.Green,
                        _ => ConsoleColor.White
                    };
                    Console.WriteLine($"Connected to {dcServer}! Time taken: {stopwatch.ElapsedMilliseconds} ms.");

                    _serverIndex++;

                    client.Close();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            
            Thread.Sleep(5000);
            Console.Clear();
        }
    }
}