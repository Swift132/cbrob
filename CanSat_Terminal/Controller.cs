using System;
using System.Windows.Forms;
using System.IO.Ports;



namespace CanSat_Terminal
{
    static class Controller
    {
        static View view;
        static SerialPort port;
        static Model model;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            model = new Model();
            view = new View();
            port = new SerialPort();

            view.onGetCOM += getCOM;
            view.onSelectCOM += selectCOM;
            view.onIniciar += iniciar;
            view.onTerminar += terminar;
            view.onSelectBaudRate += selectBaudRate;
            port.DataReceived += new SerialDataReceivedEventHandler(NewStringReceived);

            Application.Run(view);

        }

        static void getCOM(object sender, EventArgs e)
        {
            string[] avPorts = SerialPort.GetPortNames();
            if (avPorts.Length == 0)
            {
                throw new ViewException();
            }
            else
            {
                model.availableCOMs = avPorts;
                view.comboBoxCOM.Items.AddRange(model.availableCOMs);
            }
        }

        static void selectCOM(object sender, EventArgs e)
        {
            view.comboBoxCOM.SelectedIndex = 0;
            model.SelectedCOM = view.comboBoxCOM.Text;
            port.PortName = model.SelectedCOM;
        }

        static void selectBaudRate(object sender, EventArgs e)
        {
            model.selectedBaudRate = view.baud_box.Text;
            view.relatorio.Text += "Baud Rate: " + model.selectedBaudRate + Environment.NewLine;
        }

        static void iniciar(object sender, EventArgs e)
        {
            bool flag = false;
            do
            {
                try
                {
                    if (!port.IsOpen)
                    {
                        port.Open();
                        view.buttonIniciar.Enabled = false;
                        view.buttonTerminar.Enabled = true;
                        view.relatorio.Text += "Porta " + model.SelectedCOM + " aberta" + Environment.NewLine;
                        view.relatorio.Text += "A receber dados..." + Environment.NewLine;
                        flag = true;
                    }
                }
                catch (System.IO.IOException)
                {
                    //executar se o utilizador escolhe Não na caixa de diálogo -> terminar execução
                    if (!view.ShowException("Não foi possível aceder à porta. \n Selecione porta COM. \nTentar novamente?"))
                    {
                        Application.Exit();
                    }
                    //executar se o utilizador escolher Sim na caixa de diálogo -> sair do método e ir para a View tentar de novo
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

        static void terminar(object sender, EventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                view.buttonIniciar.Enabled = true;
                view.buttonTerminar.Enabled = false;
                view.relatorio.Text += "Porta " + model.SelectedCOM + " fechada" + Environment.NewLine;
            }
        }

        private static void NewStringReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /* flag controla a saída do método que ocorre se o parse da string tiver sucesso ou
            se o utilizador escolher tentar de novo */
            bool flag = false;
            do
            {
                try
                {
                    string newString = port.ReadLine();
  
                    flag = true;
                }
                catch (ModelException)
                {
                    //executar se o utilizador escolhe Não na caixa de diálogo -> terminar execução
                    if (!view.ShowException("Ocorreu um erro! \n Tentar de novo? \n"))
                    {
                        Application.Exit();
                    }
                    //executar se o utilizador escolher Sim na caixa de diálogo -> sair do método e tentar novo parse
                    else
                    {
                        flag = true;
                    }
                }
            } while (flag == false);
        }

    }

    public class ControllerException : Exception
    {
        public ControllerException(string message)
        {
            //construtor
        }
    }
}
