using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CloudCFG_s_Test
{
    public partial class Form1 : Form
    {
        string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        WebClient wc = new WebClient { Encoding = Encoding.UTF8 };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // создаём папку с кфг
            if (Directory.Exists(md + "\\cfg") == false)
            {
                Directory.CreateDirectory(md + "\\cfg");
            }
            // получаем конфиги с сервера
            wc.Headers["User-Agent"] = "Mozilla/5.0";
            string url = wc.DownloadString("http://u96227ak.beget.tech/getCfgList.php");
            string[] parts = url.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string part in parts)
            listBox1.Items.Add(part);
            // грузим конфиги в comboBox только с расширением .txt
            foreach (string file in Directory.EnumerateFiles(md + @"\cfg", "*.txt", SearchOption.AllDirectories))
            {
                    comboBox1.Items.Add(Path.GetFileName(file) + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // очищаем listBox и заного выводим кфг пользователей
            listBox1.Items.Clear();
            wc.Headers["User-Agent"] = "Mozilla/5.0";
            string url = wc.DownloadString("http://u96227ak.beget.tech/getCfgList.php");
            string[] parts = url.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
                listBox1.Items.Add(part);
            // очищаем и грузим конфиги в comboBox только с расширением .txt
            comboBox1.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(md + @"\cfg", "*.txt", SearchOption.AllDirectories))
            {
                comboBox1.Items.Add(Path.GetFileName(file) + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // скачивание файлов
            wc.Headers["User-Agent"] = "Mozilla/5.0";
            string link = "http://u96227ak.beget.tech/cfgs/" + listBox1.SelectedItem.ToString();
            string path = md + @"\cfg\";
            wc.DownloadFile(link, path + listBox1.SelectedItem.ToString());
            text.Text = "Конфиг скачан!";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // тут ваш код, который отвечает за загрузку конфига
            // string cfgFile = listBox1.SelectedItem.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // опубликовываем файл
                string dir = md + @"\cfg\" + comboBox1.SelectedItem.ToString();
                wc.Headers.Add("Content-Type", "binary/octet-stream");
                wc.Headers["User-Agent"] = "Mozilla/5.0";
                byte[] result = wc.UploadFile("http://u96227ak.beget.tech/uploadFile.php", "POST", dir);
                string s = Encoding.UTF8.GetString(result, 0, result.Length);
                Clipboard.SetText(comboBox1.SelectedItem.ToString());
                text.Text = "ID Конфига добавлен в буфер обмена!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // скачивание файла по id
            string dir = md + @"\cfg\" + idText.Text;
            string link = "http://u96227ak.beget.tech/cfgs/" + idText.Text;
            wc.Headers.Add("Content-Type", "binary/octet-stream");
            wc.Headers["User-Agent"] = "Mozilla/5.0";
            wc.DownloadFile(link, dir);
            text.Text = "Конфиг скачан!";
        }
    }
}
