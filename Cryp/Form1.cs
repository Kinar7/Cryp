﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Cryp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // Для Ввода текста
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) // Для Вывода текста
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) // Открытый ключ
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) // Закртытый Ключ
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e) // Для Зашифровывания
        {
            string inputText = textBox1.Text;
            string keyText = textBox3.Text;
            string selectedCrypt = comboBoxCrypt.SelectedItem.ToString();
            string outText = string.Empty;

            try
            {
                switch (selectedCrypt)
                {
                    case "Cesar":
                        if (int.TryParse(keyText, out int CesarKey))
                        {
                            outText = CesarEncrypt(inputText, CesarKey);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите числовой ключ для шифра цезаря");
                            return;
                        }
                        break;
                    case "RSA":
                        if (!string.IsNullOrEmpty(textBox3.Text))
                        {
                            outText = RSAEncrypt(inputText, textBox3.Text);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите открытый ключ для RSA шифрования");
                            return;
                        }
                        break;
                    case "SHA":
                        outText = ComputeSHA256Hash(inputText);
                        break;
                    case "AES":
                        if (!string.IsNullOrEmpty(keyText))
                        {
                            outText = AESEncrypt(inputText, keyText);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите пароль для AES шифрования");
                            return;
                        }
                        break;
                    default:
                        MessageBox.Show("Выберите шифр");
                        return;
                }
                textBox2.Text = outText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка шифрования: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Дешифрованние
        {
            string inputText = textBox1.Text;
            string keyText = textBox3.Text;
            string selectedCrypt = comboBoxCrypt.SelectedItem.ToString();
            string outText = string.Empty;

            try
            {
                switch (selectedCrypt)
                {
                    case "Cesar":
                        if (int.TryParse(keyText, out int CesarKey))
                        {
                            outText = CesarDecrypt(inputText, CesarKey);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите числовой ключ для дешифрования цезаря");
                            return;
                        }
                        break;
                    case "RSA":
                        if (!string.IsNullOrEmpty(textBox3.Text))
                        {
                            outText = RSADecrypt(inputText, textBox3.Text);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите закрытый ключ для RSA дешифрования");
                            return;
                        }
                        break;
                    case "SHA":
                        MessageBox.Show("Hash функция SHA является необратимой, дешифровка невозможна.");
                        return;
                    case "AES":
                        if (!string.IsNullOrEmpty(keyText))
                        {
                            outText = AESDecrypt(inputText, keyText);
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста введите пароль для AES дешифрования");
                            return;
                        }
                        break;
                    default:
                        MessageBox.Show("Выберите шифр");
                        return;
                }
                textBox2.Text = outText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка дешифрования: " + ex.Message);
            }
        }

        private void comboBoxCrypt_SelectedIndexChanged(object sender, EventArgs e) // Выбор Шифра
        {
            string selectedCrypt = comboBoxCrypt.SelectedItem.ToString();

            button3.Visible = false;
            button2.Enabled = true;

            switch (selectedCrypt)
            {
                case "SHA":
                    button2.Enabled = false;
                    textBox5.Visible = false;
                    break;
                case "RSA":
                    button2.Enabled = false;
                    textBox5.Visible = false;
                    textBox4.Visible = true;
                    textBox3.Visible = true;
                    button3.Visible = true;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e) // Что будет происходить при запуске программы.
        {
            comboBoxCrypt.Items.AddRange(new string[] {
                "AES", "RSA", "SHA", "Cesar"
            });
            comboBoxCrypt.SelectedIndex = 0;
            comboBoxCrypt_SelectedIndexChanged(sender, e);
        }

        private void button3_Click(object sender, EventArgs e) // Генератор RSA Ключей
        {
            GenerateRSAKeys(out string publicKey, out string privateKey);
            textBox3.Text = publicKey;
            textBox4.Text = privateKey;
            MessageBox.Show("Ключи RSA сгенерированы");
        }

        private string CesarEncrypt(string input, int key)
        {
            StringBuilder sb = new StringBuilder();
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
                                    "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                    "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            int en = alphabet.Length; // отвечает за длину всего алфавита
            foreach (char c in input)
            {
                int index = alphabet.IndexOf(c);
                if (index >= 0)
                {
                    int newIndex = (index + key) % en;
                    if (newIndex < 0)
                    {
                        newIndex += en;
                    }
                    sb.Append(alphabet[newIndex]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private string CesarDecrypt(string input, int key)
        {
            
            return CesarEncrypt(input,-1); 
        }

        private string RSAEncrypt(string plainText, string publicKey)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(publicKey));
                encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
            }
            return Convert.ToBase64String(encryptedData);
        }

        private string RSADecrypt(string cipherText, string privateKey)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(privateKey));
                decryptedData = rsa.Decrypt(Convert.FromBase64String(cipherText), false);
            }
            return Encoding.UTF8.GetString(decryptedData);
        }


        private void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false)); // открытый ключ
                privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true)); // закрытый ключ
            }
        }
        private string AESEncrypt(string cipherText, string privatePassword)
        {
            byte[] encryptedData;

            using (Aes aes = Aes.Create())
            {
                byte[] key = new byte[32];
                byte[] iv = new byte[16];

                byte[] passwordBytes = Encoding.UTF8.GetBytes(privatePassword.PadRight(32));
                Array.Copy(passwordBytes, key, 32);
                Array.Copy(passwordBytes, iv, 16);

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform cryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cryptoStream, Encoding.UTF8))
                    {
                        sw.Write(cipherText);
                    }
                    encryptedData = ms.ToArray();
                }

                return Convert.ToBase64String(encryptedData);
            }
        }
        private string AESDecrypt(string cipherText, string privatePassword)
        {
            string text = null;

            using (Aes aes = Aes.Create())
            {
                byte[] key = new byte[32];
                byte[] iv = new byte[16];

                byte[] passwordBytes = Encoding.UTF8.GetBytes(privatePassword.PadRight(32));
                Array.Copy(passwordBytes, key, 32);
                Array.Copy(passwordBytes, iv, 16);

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        text = sr.ReadToEnd();
                    }
                }
            }

            return text;
        }

        private string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}