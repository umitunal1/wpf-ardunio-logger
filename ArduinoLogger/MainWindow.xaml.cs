using NLog;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace ArduinoLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public string inputCount= "";
        public string[] inputs = new string[6];        
        public string strLog = "";
        public string strLog1 = "";
        public string strLog2 = "";
        public string strLog3 = "";
        public string inp = "";
        public int checkedRb;    
        Thread dataLogging;
        private bool isLoggingEnabled = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        SerialPort serialPort1 = new SerialPort();
        public double tmp2;
        public delegate void dlgLog0();
        public delegate void dlgLog1();
        public delegate void dlgLog2();
        public delegate void dlgLog3();
        public delegate void dlgLog4();
        public delegate void dlgLog5();

        public MainWindow()
        {
            InitializeComponent();
            readIniFile();
        }

        private void readIniFile()
        {
            string path = Directory.GetCurrentDirectory();
            string textFile = path + "\\" + "Configuration" + ".ini";
            using (StreamReader sw = new StreamReader(textFile))
            {
                inputCount = sw.ReadLine();
                for (int x = 0;  x <100; x++)
                {
                    if (!sw.EndOfStream)
                    {
                        inputs[x] = sw.ReadLine();
                    }
                    else
                    { break; }
                    
                }
                sw.Close();
            }
        }
        
        public void CallFromDelegateDataLogger5()
        {
            if (_5.IsChecked == true)
            {
                inp = "5";
                serialPort1.Write("5");
                strLog2 = serialPort1.ReadExisting().ToString();
            }
        }

        public void CallFromDelegateDataLogger4()
        {
            if (_4.IsChecked == true)
            {
                inp = "4";
                serialPort1.Write("4");

            }
        }

        public void CallFromDelegateDataLogger3()
        {
            if (_3.IsChecked == true)
            {
                inp = "3";
                serialPort1.Write("3");

            }
        }

        public void CallFromDelegateDataLogger2()
        {
            if (_2.IsChecked == true)
            {
                inp = "2";
                serialPort1.Write("2");
                strLog1 = serialPort1.ReadExisting().ToString();
            }
        }

        public void CallFromDelegateDataLogger1()
        {
            if (_1.IsChecked == true)
            {
                inp = "1";
                serialPort1.Write("1");

            }
        }

        public void CallFromDelegateDataLogger0()
        {
            if (_0.IsChecked == true)
            {
                inp = "0";
                serialPort1.Write("0");

            }
        }
        
        public void CallFromDelegateTxtTemp()
        {
            tmp2 = Convert.ToDouble(txtTemp.Text);
        }
        
        private void DataLogger()
        {
            try
            {             
                while (isLoggingEnabled)
                {
                    _0.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger0));
                    _1.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger1));
                    _2.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger2));
                    _3.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger3));
                    _4.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger4));
                    _5.Dispatcher.Invoke(new dlgLog0(CallFromDelegateDataLogger5));
                    Thread.Sleep(2000);
                    strLog = serialPort1.ReadExisting().ToString();
                    double tmp = Convert.ToDouble(strLog, CultureInfo.InvariantCulture);
                    txtTemp.Dispatcher.Invoke(new dlgLog1(CallFromDelegateTxtTemp));
               
                    if (tmp > tmp2)
                    {
                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.Host = "smtp.gmail.com";
                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("youremail", "yourpassword");
                        MailMessage mm = new MailMessage("donotreply@domain.com", txtTo.Text, "Temperature Warning!", "Please check the system room ! ");
                        //mm.BodyEncoding = UTF8Encoding.UTF8;
                        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        client.Send(mm);
                    }                    
                    logger.Info(inp.ToString() + " 'ten ölçülen Anlık sıcaklık Değeri : " + strLog);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private async void btn_ledYak(object sender, RoutedEventArgs e)
        {
            txtTo.IsEnabled = false;
            isLoggingEnabled = true;
            dataLogging = new Thread(new ThreadStart(DataLogger));
            dataLogging.Start();
        }

        private void Log()
        {
            logger.Info("   " + inp + " 'den ölçülen sıcaklık değeri : " + strLog);
        }

        private void btn_ledSondur(object sender, RoutedEventArgs e)
        {
            serialPort1.Write("0");
        }

        private void btnShowTemperature(object sender, RoutedEventArgs e)
        {
            Log();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);       
           
        }

        private void btn_ledYakStop(object sender, RoutedEventArgs e)
        {
            txtTo.IsEnabled = false;
            isLoggingEnabled = false;
            dataLogging.Abort();
        }

        private void config(object sender, MouseButtonEventArgs e)
        {
            Config x = new Config();
            x.Show();
        }   

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort1.PortName = txtComPort.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Open();
                if (serialPort1.IsOpen)
                {
                    MessageBox.Show("Connected");
                }
                else
                {
                    MessageBox.Show("Check your connection");
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
