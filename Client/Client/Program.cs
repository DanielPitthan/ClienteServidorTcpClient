using System.Net.Sockets;
using System.Net;
using System.Text;

//Configuração do Servidor
const string ENDERECO_IP = "127.0.0.1";
const int PORTA_CONEXAO = 8181;


Console.WriteLine("Iniciando Cliente...");
Console.WriteLine($"Conectando no servidro {ENDERECO_IP} porta {PORTA_CONEXAO}");

IPHostEntry ipHostInfo = Dns.GetHostByAddress(ENDERECO_IP);
IPAddress ipAddress = ipHostInfo.AddressList[0];

var ipEndPoint = new IPEndPoint(ipAddress, PORTA_CONEXAO);


while (true)
{
    try
    {
        //Inicia o TCPClient
        using TcpClient client = new();

        //Conecta no servidor
        await client.ConnectAsync(ipEndPoint);

        //Abre o Stream de conecxão 
        await using NetworkStream stream = client.GetStream();

        //Faz a leitura da mensagem enviada do servidor 
        var buffer = new byte[1024];
        int received = await stream.ReadAsync(buffer);

        var message = Encoding.UTF8.GetString(buffer, 0, received);
        if (!message.Equals("<|CONTINUE|>"))
            Console.WriteLine(message);

        //Pede um mensagem para ser enviada ao servidor 
        Console.WriteLine("Escreva a sua mensagem:");
        message = Console.ReadLine();
        await stream.WriteAsync(Encoding.UTF8.GetBytes(message));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Conexão encerrada pelo servidor. {ex.Message}");
        break;
    }

}