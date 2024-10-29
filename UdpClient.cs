using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace _21._10_UDP_Multiplayer_game
{
    internal class Client
    {
        
        private const int serverPort = 11000;
        private const string multicastAddress = "235.5.5.11";

        private const int multicastPort = 12345;
        private UdpClient udpClient;
        private IPAddress multicastIp;
        private Board _board;

        List<Players> players = new List<Players>();    

        public Client(string ip, int port,Board board)
        {
            multicastIp = IPAddress.Parse(multicastAddress);
            udpClient = new UdpClient(multicastPort);
            udpClient.JoinMulticastGroup(multicastIp);
            _board = board;
        }

        public void SendMessage(string messageX, string messageY)
        {
            var msg = String.Join(";", new[] { messageX, messageY, $"{DateTime.Now}" });
            byte[] data = Encoding.UTF8.GetBytes(msg);
            udpClient.Send(data, new IPEndPoint(multicastIp, serverPort));



            
        }

        public async Task ReceiveMessageAsync()
        {
            
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                var result1 = udpClient.Receive(ref remoteEndPoint);
                string str = Encoding.UTF8.GetString(result1);
                var parts = str.Split(';');
                //Console.WriteLine($"Игрок {multicastAddress}:{multicastPort} находится на координатах x={parts[0]}, y={parts[1]}");
                int X = Int32.Parse(parts[0])-1;
                int Y = Int32.Parse(parts[1])-1;

                var fitPlayer = players.FirstOrDefault(x => x.port == multicastPort);
                if (fitPlayer == null)
                {
                    var player = new Players(multicastPort, X, Y);
                    players.Add(player);
                }
                else
                {
                    int curX = fitPlayer.x;
                    int curY = fitPlayer.y;
                    _board.CellMas[curX, curY].State = CellState.Empty;
                    fitPlayer.x = X;
                    fitPlayer.y = Y;
                }
                
               
               
                _board.CellMas[X,Y].State = CellState.Occupied;
                _board.Print();



            }
            
          
        }
        public  string ExtractNumber(string input)
        {
           
            Match match = Regex.Match(input, @"\d+");
           
            return match.Success ? match.Value : "Число не найдено";
        }
    }
}

public class Players
{
    public int port;
    public int x;
    public int y;
    public Players(int port, int x, int y)
    {
        this.port = port;
        this.x = x;
        this.y = y;
    }
}
