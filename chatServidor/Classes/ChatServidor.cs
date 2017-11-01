using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;
using chatServidor.Classes;
using static chatServidor.Classes.StatusChangedEventArgs;

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

        //notificações do chat
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;

        public ChatServidor (IPAddress enderecoIP)
        {
            this.enderecoIP = enderecoIP;
        }

        //A thread que tratará o escutador do chat
        private Thread thrListener;

        //Que escuta as conexões TCP
        private TcpListener tlsCliente;

        //Verifica se o servidor está rodando (será usado no while)
        bool ServRodando = false;

        // Inclui o usuário no Hash
        public static void IncluiUsuario(TcpClient tcpUsuario, string strUsername)
        {
            //inclui o nome e conexão associada para ambas as hash tables
            ChatServidor.htUsuarios.Add(strUsername, tcpUsuario);
            ChatServidor.htConexoes.Add(tcpUsuario, strUsername);

            // Envia uma mensagem a nova conexão para todos os usuário e para o formulário do servidor
            EnviaMensagemAdmin(htConexoes[tcpUsuario] + " entrou..");
        }

        // Remove o usuário das tabelas (hash tables)
        public static void RemoveUsuario(TcpClient tcpUsuario)
        {
            // Se o usuário existir
            if (htConexoes[tcpUsuario] != null)
            {
                // Primeiro mostra a informação e informa os outros usuários sobre a conexão
                EnviaMensagemAdmin(htConexoes[tcpUsuario] + " saiu...");

                // Removeo usuário da hash table
                ChatServidor.htUsuarios.Remove(ChatServidor.htConexoes[tcpUsuario]);
                ChatServidor.htConexoes.Remove(tcpUsuario);
            }
        }

        // Este evento é chamado quando queremos disparar o evento StatusChanged
        public static void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChangedEventHandler statusHandler = StatusChanged;
            if (statusHandler != null)
            {
                // invoca o  delegate
                statusHandler(null, e);
            }
        }

        // Envia mensagens administratias
        public static void EnviaMensagemAdmin(string Mensagem)
        {
            StreamWriter swSenderSender;

            // Exibe primeiro na aplicação
            e = new StatusChangedEventArgs("Administrador: " + Mensagem);
            OnStatusChanged(e);

            // Cria um array de clientes TCPs do tamanho do numero de clientes existentes
            TcpClient[] tcpClientes = new TcpClient[ChatServidor.htUsuarios.Count];
            // Copia os objetos TcpClient no array
            ChatServidor.htUsuarios.Values.CopyTo(tcpClientes, 0);
            // Percorre a lista de clientes TCP
            for (int i = 0; i < tcpClientes.Length; i++)
            {
                // Tenta enviar uma mensagem para cada cliente
                try
                {
                    // Se a mensagem estiver em branco ou a conexão for nula sai...
                    if (Mensagem.Trim() == "" || tcpClientes[i] == null)
                    {
                        continue;
                    }
                    // Envia a mensagem para o usuário atual no laço
                    swSenderSender = new StreamWriter(tcpClientes[i].GetStream());
                    swSenderSender.WriteLine("Administrador: " + Mensagem);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch // Se houver um problema , o usuário não existe , então remove-o
                {
                    RemoveUsuario(tcpClientes[i]);
                }
            }
        }




        // Envia mensagens de um usuário para todos os outros
        public static void EnviaMensagem(string Origem, string Mensagem)
        {
            StreamWriter swSenderSender;

            // Primeiro exibe a mensagem na aplicação
            e = new StatusChangedEventArgs(Origem + " disse : " + Mensagem);
            OnStatusChanged(e);

            // Cria um array de clientes TCPs do tamanho do numero de clientes existentes
            TcpClient[] tcpClientes = new TcpClient[ChatServidor.htUsuarios.Count];
            // Copia os objetos TcpClient no array
            ChatServidor.htUsuarios.Values.CopyTo(tcpClientes, 0);
            // Percorre a lista de clientes TCP
            for (int i = 0; i < tcpClientes.Length; i++)
            {
                // Tenta enviar uma mensagem para cada cliente
                try
                {
                    // Se a mensagem estiver em branco ou a conexão for nula sai...
                    if (Mensagem.Trim() == "" || tcpClientes[i] == null)
                    {
                        continue;
                    }
                    // Envia a mensagem para o usuário atual no laço
                    swSenderSender = new StreamWriter(tcpClientes[i].GetStream());
                    swSenderSender.WriteLine(Origem + " disse: " + Mensagem);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch // Se houver um problema , o usuário não existe , então remove-o
                {
                    RemoveUsuario(tcpClientes[i]);
                }
            }
        }
    }
}
