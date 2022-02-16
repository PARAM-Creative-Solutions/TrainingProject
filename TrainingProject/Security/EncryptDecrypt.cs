using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.Constants;

namespace TrainingProject.Security
{
    /// <summary>
    /// Class for encrypt and decrypt files
    /// </summary>
    public class EncryptDecrypt
    {

        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="sourceFile"></param>
        ///<param name="destinationFilename"></param>
        public static void EncryptFile(HttpPostedFileBase sourceFile, string destinationFilename)
        {
            //string password = @"KsbPa$4&";
            string password = Constants.Key;
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            string cryptFile = destinationFilename;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

            int data;
            while ((data = sourceFile.InputStream.ReadByte()) != -1)
                cs.WriteByte((byte)data);

            cs.Close();
            cs.Dispose();
            fsCrypt.Close();
            fsCrypt.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilepath"></param>
        /// <returns></returns>
        public static bool DecryptFile1(string sourceFilepath)
        {
            //string password = @"KsbPa$4&";
            string password = Constants.Key;

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new FileStream(sourceFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //FileStream fileOut = new FileStream(sourceFilepath, FileMode.Open, FileAccess.Write,FileShare.Read);
            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                                               RMCrypto.CreateDecryptor(key, key),
                                               CryptoStreamMode.Read);

            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = GetFileContentType(sourceFilepath);
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + Path.GetFileName(sourceFilepath) + Path.GetExtension(sourceFilepath));
                int data;

                while ((data = cs.ReadByte()) != -1)
                {

                    HttpContext.Current.Response.OutputStream.WriteByte((byte)data);
                }
                HttpContext.Current.Response.Flush();


                cs.Close();
                cs.Dispose();

                fsCrypt.Close();
                fsCrypt.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
            finally
            {

                cs.Close();
                cs.Dispose();

                fsCrypt.Close();
                fsCrypt.Dispose();
            }
        }





        /// <summary>
        /// for decryption
        /// </summary>
        /// <param name="sourceFilepath"></param>
        /// <returns></returns>
        public static byte[] DecryptFile(string sourceFilepath)
        {
           // byte[] FileData;
            string password = Constants.Key;

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            
            FileStream fsCrypt = new FileStream(sourceFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //FileStream fileOut = new FileStream(sourceFilepath, FileMode.Open, FileAccess.Write,FileShare.Read);
            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                                               RMCrypto.CreateDecryptor(key, key),
                                               CryptoStreamMode.Read);

            MemoryStream ms = new MemoryStream();
                cs.CopyTo(ms);
            
            //using (BinaryReader br = new BinaryReader(cs))
            //{
            //    FileData = br.ReadBytes((int)fsCrypt.Length);
            //}

            cs.Close();
            cs.Dispose();
            ms.Dispose();
            fsCrypt.Close();
            fsCrypt.Dispose();
            return ms.ToArray(); 

        }

        public static string GetFileContentType(string sourceFilepath)
        {
            string ContentType = null;
            if (Path.GetExtension(sourceFilepath) == ".pdf")
            {
                ContentType = "application/pdf";
            }
            else if (Path.GetExtension(sourceFilepath) == ".jpg" || Path.GetExtension(sourceFilepath) == ".jpeg" || Path.GetExtension(sourceFilepath) == ".png" || Path.GetExtension(sourceFilepath) == ".tiff" || Path.GetExtension(sourceFilepath) == ".gif")
            {
                ContentType = "image/png";
            }
            else if (Path.GetExtension(sourceFilepath) == ".xls")
            {
                ContentType = "application / vnd.ms - excel";
            }
            else if (Path.GetExtension(sourceFilepath) == ".xlsx")
            {
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else if (Path.GetExtension(sourceFilepath) == ".doc")
            {
                ContentType = "application/msword";
            }
            else if (Path.GetExtension(sourceFilepath) == ".docx")
            {
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }

            return ContentType;
        }
    }
}