using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using static RygDataModel.ModelHelper;

namespace RygDataModel
{
    /// <summary>
    /// Symmetric (Private-key) Cryptography functions used for long term data storage.  
    /// Data Encryption, Decryption and Hash calculation Functionality.  
    /// </summary>
    /// <remarks>
    /// Use when storing/retrieving DataModel encrypted fields (properties) to ensure a standardized approach.  
    /// Do NOT use when Asymmetric (Public-key) Encryption is more appropriate!  
    /// Actual Encryption key is still stored by project using appropriate protection and passed as parameters to these functions.  
    /// NB: Note that the encryption key passed as a parameter is not the final key that will be used, 
    /// it will be combined with the Salt, ExtraSalt and some Hardcoded values to create the final encryption key that will be used.  
    /// This is done to add an extra layer of abstraction/difficulty and make it harder 
    /// for Website host provider or Someone with access to the key files to decrypt data.  
    /// Encrypted data is returned in Base64url text form by default for storage in the data fields/properties, so eventually the db. 
    /// Encrypted hash is returned in Short Hex text form by default for storage in the data fields/properties, so eventually the db. 
    /// The return text data format can be overriden using function parameters.  
    /// </remarks>
    /// <see cref="https://en.wikipedia.org/wiki/Cryptography"/>
    public class CryptoHelper
    {
        //Class Properties populated by EncryptData/DecryptData functions 
        /// <summary>
        /// The decrypted value in UTF8 string format.
        /// </summary>
        public string DecryptedValue { get; set; }
        /// <summary>
        /// The encrypted value in the requested outputEncodingType StringEncodingType string format.
        /// </summary>
        public string EncryptedValue { get; set; }
        /// <summary>
        /// The cipher Initialization Vector value to reuse in StringEncodingType.Base64url string format.
        /// </summary>
        /// <remarks>
        /// Typically left blank when encrypting new data, can be populated for previously encrypted data to reuse the same IV.
        /// </remarks>
        public string EncryptedInitializationVector { get; set; }
        /// <summary>
        /// The StringEncodingType string format of the encrypted string.
        /// </summary>
        /// <remarks>
        /// The application standard to be used when writing to the Database is StringEncodingType.Base64url 
        /// as it is most compatible with the majority of DB engines and is both URL and File Name safe.
        /// </remarks>
        public StringEncodingType EncryptedValueEncodingType { get; set; }
        /// <summary>
        /// The encrypted value to write to the Database, contains the Encrypted data and cipher IV separated by a semicolon (;).
        /// </summary>
        public string EncryptedDbValue => new StringBuilder($"{EncryptedValue?.ToString()};{EncryptedInitializationVector?.ToString()}").ToString();

        /// <summary>
        /// Encrypt the requested data.
        /// </summary>
        /// <remarks>
        /// Returns the data in the Class Property fields.  
        /// NB: When writing to the Database, use the StringEncodingType.Base64url string format as a standard. 
        /// </remarks>
        /// <param name="dataToEncrypt">The UTF8 string to encrypt.</param>
        /// <param name="dataSecretKeyValue">The securely stored key to be seeded and used to perform the encryption in UTF8 format.  NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is stored in another format in your secured configuration.</param>
        /// <param name="useInitializationVectorValue">The StringEncodingType.Base64url formatted string cipher IV value to reuse only if desired.  Leave blank for the value to be calculated when adding new data.</param>
        /// <param name="dataKeySaltValue">A Salt value to be added to the key itself to perform the encryption.  Added as an extra layer of security so that merely having access to the securely stored settings alone would not be enough to decrypt the data.</param>
        /// <param name="dataKeyExtraSaltValue">An Additional Salt value to be added to the key itself to perform the encryption.  Added as an extra layer of security so that merely having access to the securely stored settings alone would not be enough to decrypt the data.</param>
        /// <param name="outputEncodingType">The string format in which to return the encrypted data.  Note that the standard to be used when writing to the Database is StringEncodingType.Base64url as it is most compatible with the majority of DB engines and is both URL and File Name safe.</param>
        public void EncryptData(string dataToEncrypt,
                                string dataSecretKeyValue = "",
                                string useInitializationVectorValue = "",
                                string dataKeySaltValue = "",
                                string dataKeyExtraSaltValue = "",
                                StringEncodingType outputEncodingType = StringEncodingType.Base64url)
        {

            //Generate the *actual* Encryption Key that will be used using a hashed value of the obfuscated password to ensure 512 bit key length
            //1. Create the Key value to be hashed
            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new StringBuilder(dataSecretKeyValue);
            _sbDataKey.Append(dataKeySaltValue);
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append("d^T@==>...(x9x)");
            _sbDataKey.Append((char)115);
            _sbDataKey.Append((char)125);
            _sbDataKey.Append((char)101);
            _sbDataKey.Append("2");
            _sbDataKey.Append((char)87);
            _sbDataKey.Append((char)59);
            _sbDataKey.Append("-");
            _sbDataKey.Append((char)106);
            _sbDataKey.Append((char)57);
            _sbDataKey.Append((char)45);
            _sbDataKey.Append((char)44);
            _sbDataKey.Append("*");
            _sbDataKey.Append((char)37);
            _sbDataKey.Append((char)77);
            _sbDataKey.Append((char)83);
            _sbDataKey.Append(",");
            _sbDataKey.Append((char)66);
            _sbDataKey.Append(dataKeyExtraSaltValue);
            //2. Create the Key value to be used by the hashing function 
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            StringBuilder _sbHashKey = new StringBuilder("0_=^_&>128");
            _sbHashKey.Append((char)118);
            _sbHashKey.Append((char)47);
            _sbHashKey.Append((char)55);
            _sbHashKey.Append("$");
            _sbHashKey.Append((char)70);
            _sbHashKey.Append((char)62);
            _sbHashKey.Append("#");
            _sbHashKey.Append((char)99);
            _sbHashKey.Append((char)101);
            _sbHashKey.Append((char)87);
            _sbHashKey.Append((char)78);
            _sbHashKey.Append("%");
            _sbHashKey.Append((char)49);
            _sbHashKey.Append("?");
            _sbHashKey.Append((char)111);
            _sbHashKey.Append((char)69);
            _sbHashKey.Append((char)91);
            _sbDataKey.Append(dataSecretKeyValue);
            //Hash the result to create a tamper proof 512 bit value using a 128 byte key for efficiency to avoid SHA-256 hashing to derive the correct key length
            HashAlgorithm _hashService = new HMACSHA512(Encoding.UTF8.GetBytes(_sbHashKey.ToString().Substring(2, 128)));
            _sbHashKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Create the AES Encryption Cipher 
            Aes _cryptCipher = Aes.Create();
            _cryptCipher.Mode = CipherMode.CBC;
            _cryptCipher.KeySize = 512;
            _cryptCipher.Padding = PaddingMode.ISO10126;
            _cryptCipher.Key = _hashResult;
            if (useInitializationVectorValue?.Length > 0)
            {
                _cryptCipher.IV = ConvertStringEncodingTypeToBytes(useInitializationVectorValue, StringEncodingType.Base64url);
            }

            //Create the encryptor, convert source data to bytes, and encrypt
            ICryptoTransform _cryptService = _cryptCipher.CreateEncryptor();
            byte[] _bytesToEncrypt = Encoding.UTF8.GetBytes(dataToEncrypt); 
            byte[] _encryptedBytes = _cryptService.TransformFinalBlock(_bytesToEncrypt, 0, _bytesToEncrypt.Length);

            //Populate Class Properties with Encrypted results
            DecryptedValue = dataToEncrypt;
            EncryptedValue = ConvertBytesToStringEncodingType(_encryptedBytes, outputEncodingType);
            EncryptedInitializationVector = ConvertBytesToStringEncodingType(_cryptCipher.IV, StringEncodingType.Base64url); 
            EncryptedValueEncodingType = outputEncodingType;

        }

        /// <summary>
        /// Decrypt the requested data.
        /// </summary>
        /// <remarks>
        /// Returns the data in the Class Property fields.  
        /// NB: When reading from the Database, use the StringEncodingType.Base64url string format as a standard. 
        /// </remarks>
        /// <param name="dataToDecrypt">The UTF8 string to Decrypt.</param>
        /// <param name="dataSecretKeyValue">The securely stored key to be seeded and used to perform the Decryption in UTF8 format.  NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is stored in another format in your secured configuration.</param>
        /// <param name="useInitializationVectorValue">The StringEncodingType.Base64url formatted string cipher IV value to use to Decrypt the data.  Leave blank for the value to be calculated if the encrypted data contains the IV, typically the case when using encrypted values read from the Database.</param>
        /// <param name="dataKeySaltValue">The Salt value to be added to the key itself to perform the Decryption.  NB: This must match the value originally used when encrypting the data.</param>
        /// <param name="dataKeyExtraSaltValue">The Additional Salt value to be added to the key itself to perform the Decryption.  NB: This must match the value originally used when encrypting the data.</param>
        /// <param name="encryptedDataEncodingType">The string format of the Encrypted data.  Note that the standard to be used when reading/writing from/to the Database is StringEncodingType.Base64url as it is most compatible with the majority of DB engines and is both URL and File Name safe.</param>
        public void DecryptData(string dataToDecrypt,
                                string dataSecretKeyValue = "",
                                string useInitializationVectorValue = "",
                                string dataKeySaltValue = "",
                                string dataKeyExtraSaltValue = "",
                                StringEncodingType encryptedDataEncodingType = StringEncodingType.Base64url)
        {

            //Validate that we have an IV to perform the decryption
            if (useInitializationVectorValue?.ToString().Length == 0)
            {
                if (dataToDecrypt.IndexOf(";") >= 0)
                {
                    useInitializationVectorValue = dataToDecrypt.Substring(dataToDecrypt.IndexOf(";") + 1, dataToDecrypt.Length - dataToDecrypt.IndexOf(";"));
                    dataToDecrypt = dataToDecrypt.Substring(0, dataToDecrypt.IndexOf(";"));
                }
                else
                {
                    throw new ApplicationException("Cipher Initialization Vector Value not supplied or found in requested data to decrypt, decryption of data not possible.");
                }
            }

            //Generate the *actual* Decryption Key that will be used using a hashed value of the obfuscated password to ensure 512 bit key length
            //1. Create the Key value to be hashed
            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new StringBuilder(dataSecretKeyValue);
            _sbDataKey.Append(dataKeySaltValue);
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append("d^T@==>...(x9x)");
            _sbDataKey.Append((char)115);
            _sbDataKey.Append((char)125);
            _sbDataKey.Append((char)101);
            _sbDataKey.Append("2");
            _sbDataKey.Append((char)87);
            _sbDataKey.Append((char)59);
            _sbDataKey.Append("-");
            _sbDataKey.Append((char)106);
            _sbDataKey.Append((char)57);
            _sbDataKey.Append((char)45);
            _sbDataKey.Append((char)44);
            _sbDataKey.Append("*");
            _sbDataKey.Append((char)37);
            _sbDataKey.Append((char)77);
            _sbDataKey.Append((char)83);
            _sbDataKey.Append(",");
            _sbDataKey.Append((char)66);
            _sbDataKey.Append(dataKeyExtraSaltValue);
            //2. Create the Key value to be used by the hashing function 
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            StringBuilder _sbHashKey = new StringBuilder("0_=^_&>128");
            _sbHashKey.Append((char)118);
            _sbHashKey.Append((char)47);
            _sbHashKey.Append((char)55);
            _sbHashKey.Append("$");
            _sbHashKey.Append((char)70);
            _sbHashKey.Append((char)62);
            _sbHashKey.Append("#");
            _sbHashKey.Append((char)99);
            _sbHashKey.Append((char)101);
            _sbHashKey.Append((char)87);
            _sbHashKey.Append((char)78);
            _sbHashKey.Append("%");
            _sbHashKey.Append((char)49);
            _sbHashKey.Append("?");
            _sbHashKey.Append((char)111);
            _sbHashKey.Append((char)69);
            _sbHashKey.Append((char)91);
            _sbDataKey.Append(dataSecretKeyValue);
            //Hash the result to create a tamper proof 512 bit value using a 128 byte key for efficiency to avoid SHA-256 hashing to derive the correct key length
            HashAlgorithm _hashService = new HMACSHA512(Encoding.UTF8.GetBytes(_sbHashKey.ToString().Substring(2, 128)));
            _sbHashKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Create the AES Decryption Cipher 
            Aes _cryptCipher = Aes.Create();
            _cryptCipher.Mode = CipherMode.CBC;
            _cryptCipher.KeySize = 512;
            _cryptCipher.Padding = PaddingMode.ISO10126;
            _cryptCipher.Key = _hashResult;
            _cryptCipher.IV = ConvertStringEncodingTypeToBytes(useInitializationVectorValue, StringEncodingType.Base64url);

            //Create the Decryptor, convert source data to bytes, and Decrypt
            ICryptoTransform _cryptService = _cryptCipher.CreateDecryptor();
            byte[] _bytesToDecrypt = Encoding.UTF8.GetBytes(dataToDecrypt);
            byte[] _decryptedBytes = _cryptService.TransformFinalBlock(_bytesToDecrypt, 0, _bytesToDecrypt.Length);

            //Populate Class Properties with Decrypted results
            DecryptedValue = ConvertBytesToStringEncodingType(_decryptedBytes, encryptedDataEncodingType); 
            EncryptedValue = dataToDecrypt;
            EncryptedInitializationVector = ConvertBytesToStringEncodingType(_cryptCipher.IV, StringEncodingType.Base64url);
            EncryptedValueEncodingType = encryptedDataEncodingType;

        }

        /// <summary>
        /// Calculate a hash for the supplied value.
        /// </summary>
        /// <remarks>
        /// Calculates SHA512/HMACSHA512 hash.  
        /// NB: The Key Value as well as both salt values must be available to calculate a salted value for comparison.  
        /// Note: We don't have to worry about the key length.  
        /// The key can be any length. However, the recommended size is 64 bytes. 
        /// If the key is more than 64 bytes long, it is hashed (using SHA-256) to derive a 64-byte key, 
        /// if it is less than 64 bytes long, it is padded to 64 bytes.  
        /// </remarks>
        /// <param name="dataToHash">The source data/property value to hash.</param>
        /// <param name="hashDataSaltValue">The main Salt to be added to the actual data before hashing.</param>
        /// <param name="hashDataExtraSaltValue">An additional Salt to be added to the actual data before hashing.</param>
        /// <param name="hashSecretKeyValue">The securely stored key to be seeded and used to perform the hash in UTF8 format.  NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is stored in another format in your secured configuration.</param>
        /// <param name="hashKeySaltValue">A Salt value to be added to the key itself to perform the hash.  Added as an extra layer of security so that merely having access to the securely stored settings alone would not be enough to calculate the hash.</param>
        /// <param name="hashKeyExtraSaltValue">An additional Salt value to be added to the key itself to perform the hash.  Added as an extra layer of security so that merely having access to the securely stored settings alone would not be enough to calculate the hash.</param>
        /// <param name="hashStringOutputType">The StringEncodingType format in which to return the hashed value.</param>
        /// <returns>
        /// Returns the hashed value in the requested StringEncodingType string format.  
        /// Calculates 512 bits (8 bits/byte, 64 bytes) for the hash, actual string length returned is determined by the requested output format.
        /// By default using StringEncodingType.HexStringShort it returns a 128 character string (64 bytes x 2 for hex).  
        /// </returns>
        public string CreateHash(string dataToHash,
                                 string hashDataSaltValue = "",
                                 string hashDataExtraSaltValue = "",
                                 string hashSecretKeyValue = "",
                                 string hashKeySaltValue = "",
                                 string hashKeyExtraSaltValue = "",
                                 StringEncodingType hashStringOutputType = StringEncodingType.HexStringShort)
        {

            //Stringbuilder for performance 
            StringBuilder _sb = new StringBuilder(hashSecretKeyValue);
            _sb.Append(hashKeySaltValue);

            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sb.Append("@K3y");
            _sb.Append((char)93);
            _sb.Append((char)123);
            _sb.Append((char)72);
            _sb.Append("$");
            _sb.Append((char)92);
            _sb.Append((char)126);
            _sb.Append(".");
            _sb.Append((char)48);
            _sb.Append((char)122);
            _sb.Append("#");
            _sb.Append((char)95);
            _sb.Append((char)42);

            _sb.Append(hashKeyExtraSaltValue);

            //instantiate the class with supplied key and salts
            HashAlgorithm _hashService;
            if (hashSecretKeyValue == "")
            {
                _hashService = SHA512.Create(); //yep, aware that this code will not run but leaving it here as a placeholder should it be useful in future.
            }
            else
            {
                _hashService = new HMACSHA512(Encoding.UTF8.GetBytes(_sb.ToString()));
            }

            //Salt the hash data itself (only done with actual *data* when _hashing_)
            _sb.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            _sb.Append(dataToHash);
            _sb.Append(hashDataSaltValue);
            _sb.Append(hashDataExtraSaltValue);

            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sb.ToString()));
            _sb.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            return ConvertBytesToStringEncodingType(_hashResult, hashStringOutputType);
        }

        /// <summary>
        /// Use cryptographically strong random number generator to create a random salt.  
        /// </summary>
        /// <remarks>
        /// NB: This value should be stored when used for Hashing/Encryption as it is required to Validate the Hash/Decrypt the value.  
        /// </remarks>
        /// <param name="numberOfBytes">The number of bytes to use to generate the Salt.</param>
        /// <param name="stringOutputType">The format of the string to return.</param>
        /// <returns></returns>
        public string GenerateRandomSalt(int numberOfBytes = 16, 
                                         StringEncodingType stringOutputType = StringEncodingType.Base64url)
        {
            
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] newSalt = new byte[numberOfBytes];
            rng.GetBytes(newSalt);

            return ConvertBytesToStringEncodingType(newSalt, stringOutputType);

        }

    }
}
