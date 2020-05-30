using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoLogger
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

    

        private void button1_Click(object sender, EventArgs e)
        {

            int inputCount = 0;
            string path = Directory.GetCurrentDirectory();
            string textFile = path + "\\" + "Configuration" + ".ini";
            using (StreamWriter sw = new StreamWriter(textFile))
            {
                
                sw.WriteLine(sensorCount.Value.ToString());
                string[] inputs = inputNos.Text.Split(',');
                foreach (var inp in inputs)
                {
                    inputCount++;
                    sw.WriteLine(inp);
                }
                if (inputCount != sensorCount.Value)
                {
                    MessageBox.Show("Please check the sensor count or input nos");
                    return;
                }
                else
                {
                    MessageBox.Show("Saved successfully");
                    sw.Close();
                    this.Close();
                }
                
            }
          
        }
    }
}
