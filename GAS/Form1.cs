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
                    // 製作GET參數，這裡示範將key參數設為"your_value"
                    string parameter = "key=your_value";

                    // 組合GET請求URL
                    string urlWithParameters = $"{ScriptUrl}?{parameter}";

                    // 發送GET請求
                    HttpResponseMessage response = await client.GetAsync(urlWithParameters);

                    // 確認請求是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        // 讀取響應內容
                        string result = await response.Content.ReadAsStringAsync();

                        // 在這裡處理GET請求的結果
                        MessageBox.Show(result, "GET Request Successful");

                        // 將結果傳遞給TCP伺服器
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
                // 連接到內網TCP伺服器
                using (TcpClient client = new TcpClient(TcpServerIp, TcpServerPort))
                using (NetworkStream stream = client.GetStream())
                {
                    // 將值轉換成字節數組
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    // 發送數據到TCP伺服器
                    stream.Write(dataBytes, 0, dataBytes.Length);

                    // 可以添加額外的處理邏輯，例如等待伺服器響應等
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