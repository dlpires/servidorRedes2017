﻿using chatServidor.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatServidor
{
    public partial class frmServidor : Form
    {
        private delegate void AtualizaStatusCallback(string strMensagem);

        public frmServidor()
        {
            InitializeComponent();
        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            //Tratamento para campo vazio
            if (txtIP.Text == string.Empty)
            {
                MessageBox.Show("Informe o endereço IP.");
                txtIP.Focus();
                return;
            }

            try
            {
                // Analisa o endereço IP do servidor informado no textbox
                IPAddress enderecoIP = IPAddress.Parse(txtIP.Text);

                // Cria uma nova instância do objeto ChatServidor
                ChatServidor mainServidor = new ChatServidor(enderecoIP);

                // Vincula o tratamento de evento StatusChanged a mainServer_StatusChanged
                ChatServidor.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);

                // Inicia o atendimento das conexões
                mainServidor.IniciaAtendimento();

                // Mostra que nos iniciamos o atendimento para conexões
                txtLog.AppendText("Monitorando as conexões...\r\n");
            }catch(Exception ex)
            {
                MessageBox.Show("Erro de conexão : " + ex.Message);
            }

        }

        public void mainServidor_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // Chama o método que atualiza o formulário
            this.Invoke(new AtualizaStatusCallback(this.AtualizaStatus), new object[] { e.EventMessage });
        }

        private void AtualizaStatus(string strMensagem)
        {
            // Atualiza o logo com mensagens
            txtLog.AppendText(strMensagem + "\r\n");
        }
    }
}
