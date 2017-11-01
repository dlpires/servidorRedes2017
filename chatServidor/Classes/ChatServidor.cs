﻿using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace chatServidor.Classes
{
    public class ChatServidor
    {
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
