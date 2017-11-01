using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace chatServidor.Classes
{
    public class StatusMensagem : EventArgs
    {
        private string MsgEvento { get; set; } //evento da mensagem

        public StatusMensagem(string MsgEvento) //Contrutor do evento
        {
            this.MsgEvento = MsgEvento;
        }

    }
}
