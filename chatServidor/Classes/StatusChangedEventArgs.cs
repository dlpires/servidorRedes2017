using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace chatServidor.Classes
{
    public class StatusChangedEventArgs : EventArgs
    {
        private string EventMsg { get; set; } //evento da mensagem

        public StatusChangedEventArgs(string EventMsg) //Contrutor do evento
        {
            this.EventMsg = EventMsg;
        }

    }
}
