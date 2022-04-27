using System;
using System.Windows.Forms;

namespace CanSat_Terminal
{
    public partial class View : Form
    {

        public event EventHandler onGetCOM = null;
        public event EventHandler onSelectCOM = null;
        public event EventHandler onIniciar = null;
        public event EventHandler onTerminar = null;
        public event EventHandler onSelectBaudRate = null;

        public View()
        {
            InitializeComponent();
        }

        private void getCOM(object sender, EventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    onGetCOM?.Invoke(sender, e);
                    flag = true;
                }
                catch (ViewException)
                {
                    //executar se o utilizador escolhe Não na caixa de diálogo -> terminar execução
                    if (!ShowException("Não há portas COM disponíveis. \n Tentar de novo?\n"))
                    {
                        Application.Exit();
                    }
                    //executar se o utilizador escolher Sim na caixa de diálogo -> sair do método e ficar na View para tentar de novo
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

        private void comboBoxCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            onSelectCOM?.Invoke(sender, e);
        }

        private void onSelectBaudRateEvent(object sender, EventArgs e)
        {
            onSelectBaudRate?.Invoke(sender, e);
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    onIniciar?.Invoke(sender, e);
                    flag = true;
                }
                catch (ViewException)
                {
                    //executar se o utilizador escolhe Não na caixa de diálogo -> terminar execução
                    if (!ShowException("Não há portas COM disponíveis. \n Tentar de novo?\n"))
                    {
                        Application.Exit();
                    }
                    //executar se o utilizador escolher Sim na caixa de diálogo -> sair do método e ficar na View para tentar de novo
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

        private void buttonTerminar_Click(object sender, EventArgs e)
        {
            onTerminar?.Invoke(sender, e);
        }


        public bool ShowException(string message)
        {
            string title = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ViewException : Exception
    {
        public ViewException()
        {
            //construtor
        }
    }
}
