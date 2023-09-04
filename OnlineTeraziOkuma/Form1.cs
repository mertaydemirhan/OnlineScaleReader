using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Windows.Forms;

namespace OnlineTeraziOkuma
{
    public partial class Form1 : Form
    {
        public SerialPort comPort = new SerialPort();
        //public string[] ports = SerialPort.GetPortNames();
        //private static int[] baundRate = new int[] { 2400, 4800, 9600, 19200, 38400 };
        public string Gelen = "";
        public readonly string PortName = ConfigurationManager.AppSettings["PortName"];
        public readonly string BaundRate = ConfigurationManager.AppSettings["BaundRate"];
        public readonly string ServerAdress = ConfigurationManager.AppSettings["ServerAdress"];
        public readonly string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        public readonly string UsrName = ConfigurationManager.AppSettings["UsrName"];
        public readonly string Pw = ConfigurationManager.AppSettings["Pw"];
        public List<string> OkunanDeger = new List<string>();
        public string Os;
        SqlConnection baglanti;

        public Form1(string _Os)
        {
            InitializeComponent();
            Os = _Os;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PortOpen(PortName, int.Parse(BaundRate));
            //comPort.DataReceived += ComPort_DataReceived;
            comPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
            
        }

        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                
                var val = comPort.ReadLine().Replace("ST,GS,", "").Replace("kg", "").Replace("   ", "").Replace(" ", "").Replace("▒,", "").Replace("?","").Replace("","").Replace("K","").Replace(")","").Replace("r","").Replace("Z", "");
                OkunanDeger.Add(val);
                if (val != null)
                {
                    if (val == "") return;

                    if (double.Parse(val) > 0.000)
                    {
                        txtWeight.Text = val;
                        txtWeight.Text = txtWeight.Text.Trim();

                    }
                    //if (double.Parse(val) == 0.000 || double.Parse(val) > 0.000) txtWeight.Text = val;
                }
                Ekle();
                //Ekle();
                //txtWeight.Text = val;
                if (listBox1.Items.Count > 10) listBox1.Items.Clear();
                listBox1.Items.Add(val);
            }
            catch (System.IO.IOException) { Gelen = ""; }
            catch (InvalidOperationException ex)
            {
                Gelen = "";
            } // bağlantı kapalı olduğunda çıkıyor


        }

        public void PortOpen(string port_name, int baudrate)
        {
            if (comPort.IsOpen) comPort.Close();
            if (!comPort.IsOpen)
            {
                try
                {
                    comPort.PortName = port_name;
                    comPort.BaudRate = baudrate; 
                    comPort.Open();
                    comPort.Handshake = Handshake.RequestToSendXOnXOff;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().Name + Environment.NewLine + ex.Message);
                }
            }
        }


        public void Ekle()
        {
            baglanti = new SqlConnection("Server="+ServerAdress+";Database="+DatabaseName+";User Id="+UsrName+";Password="+Pw+";");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            cmd.CommandText = "INSERT INTO " + DatabaseName + ".dbo.ArelOnlineScale (Weight) VALUES ('" + txtWeight.Text.Trim() + "')";           
            baglanti.Open();
            cmd.ExecuteNonQuery();
            int rowsAffected = cmd.ExecuteNonQuery();
            baglanti.Close();
            Dispose();
            Application.Exit();
            GC.SuppressFinalize(this);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            txtWeight.Text = "0.000";
        }
    }
}
