using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JSON
{
    public static class AES
    {       
        private const string AES_IV_256 = @"pf69DL6GrWFyZcMK";                 // 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"        
        private const string AES_Key_256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";// 暗号化鍵<半角32文字（8bit*32文字=256bit）>

        /// <summary>
        /// 対称鍵暗号を使って文字列を暗号化する
        /// </summary>
        /// <param name="text">暗号化する文字列</param>
        /// <param name="iv">対称アルゴリズムの初期ベクター</param>
        /// <param name="key">対称アルゴリズムの共有鍵</param>
        /// <returns>暗号化された文字列</returns>
        public static string Encrypt(string text) {
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.BlockSize = 128;      // ブロックサイズ（何文字単位で処理するか）        
            myRijndael.KeySize = 256;        // 暗号化方式はAES-256を採用        
            myRijndael.Mode = CipherMode.CBC;// 暗号利用モード        
            myRijndael.Padding = PaddingMode.PKCS7;// パディング

            myRijndael.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            myRijndael.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);// 暗号化

            byte[] encrypted;
            using (MemoryStream mStream = new MemoryStream()) {
                using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter sw = new StreamWriter(ctStream)) {
                        sw.Write(text);
                    }
                    encrypted = mStream.ToArray();
                }
            }
            return (System.Convert.ToBase64String(encrypted));// Base64形式（64種類の英数字で表現）で返す
        }//Encrypt

        /// <summary>
        /// 対称鍵暗号を使って暗号文を復号する
        /// </summary>
        /// <param name="cipher">暗号化された文字列</param>
        /// <param name="iv">対称アルゴリズムの初期ベクター</param>
        /// <param name="key">対称アルゴリズムの共有鍵</param>
        /// <returns>復号された文字列</returns>
        public static string Decrypt(string cipher) {
            RijndaelManaged rijndael = new RijndaelManaged();

            rijndael.BlockSize = 128;            // ブロックサイズ（何文字単位で処理するか）       
            rijndael.KeySize = 256;              // 暗号化方式はAES-256を採用       
            rijndael.Mode = CipherMode.CBC;      // 暗号利用モード    
            rijndael.Padding = PaddingMode.PKCS7;// パディング

            rijndael.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            rijndael.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

            string plain = string.Empty;
            using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(cipher))) {
                using (CryptoStream ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read)) {
                    using (StreamReader sr = new StreamReader(ctStream)) {
                        plain = sr.ReadLine();
                    }
                }
            }
            return plain;
        }//Decrypt
    }//AESCipher
}