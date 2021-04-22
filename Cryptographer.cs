using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    /// <summary>
    /// Only rudimentary "cryptography" is applied here. This has to be updated and encryption/decryption needs to be mode secure.
    /// </summary>
    public class Cryptographer
    {
        public const string HASH_ALGO = "SHA512";
        private string MasterKey;

        public byte[] MasterKeyHash
        {
            get
            {
                byte[] input = Encoding.UTF8.GetBytes(MasterKey);
                MD5 hash = MD5.Create();
                byte[] hashed = hash.ComputeHash(input);
                return hashed;
            }
        }
        public string MasterKeyHashString
        {
            get
            {
                return Encoding.UTF8.GetString(MasterKeyHash);
            }
        }

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

            // Defunct:
            //Cryptographer.DFT(ref content, MasterKeyHash);

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

            // Defunct:
            //Cryptographer.iDFT(ref content, MasterKeyHash);

            // TODO: Create real cryptography stuff
            //////////////////////////////
            // Real cryptography action //
            //////////////////////////////
            



            for (int i = 0; i < content.Length; i++)
            {
                ToggleLSB(ref content[i]);
            }

            return Encoding.UTF8.GetString(content);
        }

        #region Helper

        private static void ToggleLSB(ref byte toManipulate)
        {
            toManipulate = (byte)(toManipulate ^ 0x01);
        }
        private static void ToggleABit(ref byte toManipulate)
        {
            toManipulate = (byte)(toManipulate ^ 0x10);
        }
        
        #endregion
        #region COMPLEX ALGORITHMS
        /// These dont work in the context I would have liked them to be used...


        /// <summary> This just scrambles the MasterKey in some way which doesnt makes sense at all (I had fun playing with bits :3 )</summary>
        /// <returns>A number which was created by the MasterKey string.</returns>
        private byte[] CreateWindowFromMasterKeyHash()
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
            digit_sum = ~digit_sum;

            // TODO: Chop value into bytes

            return new byte[] { 1 };
        }

        private static void DFT(ref byte[] data, byte[] window)
        {
            // TODO: Create DFT

            int N = data.Length;
            byte[] input = (byte[])data.Clone();

            // Create complex input:
            // Input is treated as sorely real signal
            Complex[] complex_input = new Complex[N];
            for (int o = 0; o < N; o++)
            {
                complex_input[o] = new Complex(Convert.ToDouble(input[o]), 0);
            }

            // Calculate DFT
            Complex[] a = new Complex[N];
            for (int k = 0; k < N; k++)
            {
                for (int j = 0; j < N; j++)
                {
                    a[k] = Complex.Exp((-2 * Math.PI * (j * k)) / N) * complex_input[j];
                }
            }

            // Convert complex output to real output
            byte[] output = new byte[N];
            for(int y = 0; y < N; y++)
            {
                output[y] = Convert.ToByte(a[y].Real);
            }

            data = output;

        }
        private static void iDFT(ref byte[] data, byte[] window)
        {
            // TODO: Create iDFT

            int N = data.Length;
            byte[] input = (byte[])data.Clone();

            // Create complex input:
            // Input is treated as sorely real signal
            Complex[] complex_input = new Complex[N];
            for (int o = 0; o < N; o++)
            {
                complex_input[o] = new Complex(Convert.ToDouble(input[o]), 0);
            }

            // Calculate DFT
            Complex[] a = new Complex[N];
            for (int k = 0; k < N; k++)
            {
                for (int j = 0; j < N; j++)
                {
                    a[k] = Complex.Exp((2 * Math.PI * (j * k)) / N) * complex_input[j];
                }
                a[k] /= N;
            }

            // Convert complex output to real output
            byte[] output = new byte[N];
            for (int y = 0; y < N; y++)
            {
                output[y] = Convert.ToByte(a[y].Real);
            }

            data = output;
        }
        #endregion

    }
}
