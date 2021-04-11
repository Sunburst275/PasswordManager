using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    /// <summary>
    /// Only rudimentary "cryptography" is applied here. This has to be updated and encryption/decryption needs to be mode secure.
    /// </summary>
    public class Cryptographer
    {

        private string MasterKey;

        public Cryptographer(string masterkey)
        {
            MasterKey = masterkey;
        }

        public string Encrypt(string content)
        {
            byte[] in_bytes = Encoding.UTF8.GetBytes(content);
            return Encrypt(in_bytes);
        }
        public string Encrypt(byte[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                ToggleLSB(ref content[i]);
            }
            return Encoding.UTF8.GetString(content);
        }

        // This just untoggles the toggled bits, thats why I can call "Encrypt" as decryption method.
        public string Decrypt(string content)
        {
            return Encrypt(content);
        }
        public string Decrypt(byte[] content)
        {
            return Encrypt(content);
        }

        private static void ToggleLSB(ref byte toManipulate)
        {
            toManipulate = (byte)(toManipulate ^ 0x01);
        }

    }
}
