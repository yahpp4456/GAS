using System.Net.Sockets;
using System.Text;

namespace GAS
{
    public partial class Form1 : Form
    {
        private const string ScriptUrl = "https://script.google.com/macros/s/AKfycbwJhqZYuwZGsRIgWJxGEelARDTUgBoYEXyYVSr0UrR8gP2bOVDNNRd8fQTZyEijjUpAyg/exec";
        private const string TcpServerIp = "10.0.8.244";
        private const int TcpServerPort = 1030;

        public Form1()
        {
            InitializeComponent();
        }

        private async  void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // �u��GET�������@�eʾ����key�����O��"your_value"
                    string parameter = "key=your_value";

                    // �M��GETՈ��URL
                    string urlWithParameters = $"{ScriptUrl}?{parameter}";

                    // �l��GETՈ��
                    HttpResponseMessage response = await client.GetAsync(urlWithParameters);

                    // �_�JՈ���Ƿ�ɹ�
                    if (response.IsSuccessStatusCode)
                    {
                        // �xȡ푑�����
                        string result = await response.Content.ReadAsStringAsync();

                        // ���@�e̎��GETՈ��ĽY��
                        MessageBox.Show(result, "GET Request Successful");

                        // ���Y�����f�oTCP�ŷ���
                        SendToTcpServer(result);
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}", "GET Request Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }



        private void SendToTcpServer(string data)
        {
            try
            {
                // �B�ӵ��ȾWTCP�ŷ���
                using (TcpClient client = new TcpClient(TcpServerIp, TcpServerPort))
                using (NetworkStream stream = client.GetStream())
                {
                    // ��ֵ�D�Q���ֹ����M
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    // �l�͔�����TCP�ŷ���
                    stream.Write(dataBytes, 0, dataBytes.Length);

                    // ��������~���̎��߉݋������ȴ��ŷ���푑���
                    // ...

                    MessageBox.Show("Data sent to TCP server successfully", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending data to TCP server: {ex.Message}", "Error");
            }
        }

    }
}