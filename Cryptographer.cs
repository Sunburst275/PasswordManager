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

            ///////////////////////////////////////////
            // DFT of all bytes, MASTER_PW as window //
            ///////////////////////////////////////////

            // CONT: Create the DFT
            var window = CreateWindowFromMasterKey();
            for (int i = 0; i < content.Length; i++)
            {
                Cryptographer.DFT(ref content[i], window);
            }

            // TODO: Create real cryptography stuff
            //////////////////////////////
            // Real cryptography action //
            //////////////////////////////
            // ...

            for (int i = 0; i < content.Length; i++)
            {
                ToggleABit(ref content[i]);
            }

            return Encoding.UTF8.GetString(content);
        }

        // This just untoggles the toggled bits, thats why I can call "Encrypt" as decryption method.
        public string Decrypt(string content)
        {
            byte[] in_bytes = Encoding.UTF8.GetBytes(content);
            return Decrypt(in_bytes);
        }
        public string Decrypt(byte[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                ToggleABit(ref content[i]);
            }

            ////////////////////////////////////////////
            // iDFT of all bytes, MASTER_PW as window //
            ////////////////////////////////////////////

            // CONT: Create the iDFT
            var window = CreateWindowFromMasterKey();
            for (int i = 0; i < content.Length; i++)
            {
                Cryptographer.iDFT(ref content[i], window);
            }

            // TODO: Create real cryptography stuff
            //////////////////////////////
            // Real cryptography action //
            //////////////////////////////
            // ...

            for (int i = 0; i < content.Length; i++)
            {
                ToggleLSB(ref content[i]);
            }

            return Encoding.UTF8.GetString(content);
        }

        private static void ToggleLSB(ref byte toManipulate)
        {
            toManipulate = (byte)(toManipulate ^ 0x01);
        }
        private static void ToggleABit(ref byte toManipulate)
        {
            toManipulate = (byte)(toManipulate ^ 0x10);
        }

        #region COMPLEX ALGORITHMS

        /// <summary> This just scrambles the MasterKey in some way which doesnt makes sense at all (I had fun playing with bits :3 )</summary>
        /// <returns>A number which was created by the MasterKey string.</returns>
        private uint CreateWindowFromMasterKey()
        {
            uint digit_sum = 0;
            byte k = 0;
            char[] MasterKeyCharArray = MasterKey.ToCharArray();
            for (int i = 0; i < MasterKeyCharArray.Length; i++)
            {
                char c = MasterKeyCharArray[i];
                digit_sum += (uint)((++k) * c);
                digit_sum = ~digit_sum;
            }
            digit_sum &= 0x8BADF00D;
            var checkpoint = digit_sum;
            for (int i = 0; i < MasterKeyCharArray.Length; i++)
            {
                if (digit_sum == 0)
                    digit_sum = checkpoint;
                
                char c = MasterKeyCharArray[i];
                digit_sum *= (uint)((++k) * c);
                digit_sum &= (uint)~(0x01 << k);
            }
            digit_sum &= 0xDEFEC8ED;

            return (~digit_sum);
        }

        private static void DFT(ref byte data, uint window)
        {
            // TODO: Create DFT
        }
        private static void iDFT(ref byte data, uint window)
        {
            // TODO: Create iDFT
        }
        #endregion

    }
}
