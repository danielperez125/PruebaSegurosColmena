using System.Text;
using Utils.Colmena.NuGet.Response;
using System.Security.Cryptography;

namespace Utils.Colmena.NuGet.CipherAES256
{
    public class CipherAES
    {
        private const string IV_ = "|ColmenaIVDecrypt|";
        private static string Text = String.Empty;
        private static string Pwd = String.Empty;
        private static bool IsEncrypt;
        private static string[] TxtIv = Array.Empty<string>();

        public static ApiResult Cipher(string text, string pwd, bool isEncrypt)
        {
            try
            {
                Text = text;
                Pwd = pwd;
                IsEncrypt = isEncrypt;

                List<string> validations = Validations();

                if (validations.Any())
                {
                    return new ApiResult(StateOperation.InvalidRequest, true, String.Join(" - ", validations));
                }

                using (Aes myAes = Aes.Create())
                {
                    myAes.Key = Encoding.ASCII.GetBytes(Pwd);

                    string? ciph;

                    if (IsEncrypt)
                    {
                        ciph = Encrypt(Text, myAes.Key, myAes.IV);

                    }
                    else
                    {
                        ciph = Decrypt(TxtIv[0], myAes.Key, Convert.FromBase64String(TxtIv[1]));
                    }

                    if (!String.IsNullOrWhiteSpace(ciph))
                    {
                        return new ApiResult(StateOperation.OK, ciph);
                    }
                    else
                    {
                        return new ApiResult(StateOperation.InternalServerError, new Exception("Error no controlado al ejecutar funcionalidad AES"));
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResult(StateOperation.InternalServerError, ex);
            }
        }
        private static String? Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));
            //byte[] encrypted;

            string? plainTextCiph;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        //encrypted = msEncrypt.ToArray();
                        plainTextCiph = Convert.ToBase64String(msEncrypt.ToArray());
                        plainTextCiph = (!String.IsNullOrWhiteSpace(plainTextCiph) && plainTextCiph.Length > 0) ? $"{plainTextCiph}{IV_}{Convert.ToBase64String(aesAlg.IV)}" : null;
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            //return encrypted;
            return plainTextCiph;
        }
        private static String? Decrypt(string ciphTxt, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (ciphTxt == null || ciphTxt.Length <= 0)
                throw new ArgumentNullException(nameof(ciphTxt));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));


            byte[] cipherText = Convert.FromBase64String(ciphTxt);

            // Declare the string used to hold
            // the decrypted text.
            string? plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new(cipherText))
                {
                    using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                            plaintext = (!String.IsNullOrWhiteSpace(plaintext) && plaintext.Length > 0) ? plaintext : null;
                        }
                    }
                }
            }

            return plaintext;
        }

        private static List<string> Validations()
        {
            List<string> validations = new List<string>();

            if (String.IsNullOrWhiteSpace(Text))
            {
                validations.Add("Texto es requerido");
            }
            else if (!IsEncrypt)
            {
                TxtIv = Text.Split(IV_);

                if (TxtIv.Length != 2)
                {
                    validations.Add("Texto incorrecto. Debe contener estructura: Texto Cifrado y B64IV");
                }
                else if (TxtIv.Any(x => String.IsNullOrWhiteSpace(x)))
                {
                    validations.Add($"Error de parseo a vector: {String.Join("SEPARADOR", TxtIv)}");
                }
            }
            if (String.IsNullOrWhiteSpace(Pwd))
            {
                validations.Add("Contraseña es requerida");
            }
            else
            {
                if (Pwd.Length != 32)
                {
                    validations.Add("Contraseña debe ser de 32 caracteres");
                }
            }

            return validations;
        }

    }
}
