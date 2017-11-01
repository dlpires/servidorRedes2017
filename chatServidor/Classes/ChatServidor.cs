using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace chatServidor.Classes
{
    public class ChatServidor
    {

        //consultado por usuário
        public static Hashtable htUsuarios = new Hashtable(30); //Lista de Hash armazenando usuários(máx. 30)

        //consultado por conexões
        public static Hashtable htConexoes = new Hashtable(30); //Lista de Hash armazenando usuários(máx. 30)

        private IPAddress enderecoIP; //guarda o endereço IP inserido;

        private TcpClient tcpCliente;

        public static event StatusChangedEventHandler StatusChanged;
    }
}
