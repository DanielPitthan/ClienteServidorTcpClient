//Cria um servidor

using System.Net;
using System.Net.Sockets;
using System.Text;


//Porta de conexão do Servidor 
const int PORTA_CONEXAO = 8181;

Console.WriteLine($"Iniciando Servidor. Escutando porta: {PORTA_CONEXAO}" );

//Obtem os dados do servidor e define a porta de conexão 
var ipEndPoint = new IPEndPoint(IPAddress.Any, PORTA_CONEXAO);

//Inicia listner que responsável por receber conexão 
TcpListener listener = new(ipEndPoint);
listener.Start();


var message = $"Servidor Iniciado, aguardando mensagens do cliente";


while (true)
{
	try
	{
        //Indica que vai receber qualquer conxão 
        using TcpClient handler = await listener.AcceptTcpClientAsync();

        //Abre o stream de conxão 
        await using NetworkStream stream = handler.GetStream();

        //codifia a mensagem 
        var msgEconding = Encoding.UTF8.GetBytes(message);

        //Envia a mensagem ao Cliente
        await stream.WriteAsync(msgEconding);

        //Faz a leitura da mensagem enviada pelo cliente
        var buffer = new byte[1024];
        int received = await stream.ReadAsync(buffer);

        //escreve a mensagem recebida
        message = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine(message);
        message = "<|CONTINUE|>";


        if (message.Trim().Equals("<|STOP|>"))
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes("Servidor finalizando..."));
            listener.Stop();
            break;
        }
    }
	catch (Exception ex)
	{

        Console.WriteLine($"{ex.Message}");
	}
       
}


