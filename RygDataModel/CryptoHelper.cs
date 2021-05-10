using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static RygDataModel.ModelHelper;

namespace RygDataModel
{
    /// <summary>
    /// Symmetric (Private-key) Cryptography functions used for long term data storage.  
    /// Data Encryption, Decryption and Hash calculation Functionality.  
    /// This class is built using Microsoft System.Security.Cryptography but adding an extra layer of abstraction 
    /// and wrapped into easier functions for use in shared projects to ensure a standardized approach.
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
        #region Cryptography Functions
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
        /// <param name="dataToEncrypt">
        /// The string to encrypt in UTF8 format.
        /// </param>
        /// <param name="dataSecretKeyValue">
        /// The securely stored key to be seeded and used to perform the encryption in UTF8 format.  
        /// NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is stored 
        /// in another format in your secured configuration.
        /// </param>
        /// <param name="dataKeySaltValue">A Salt value to be added to the key itself to perform the encryption.  
        /// Added as an extra layer of security so that merely having access to the securely stored settings alone 
        /// would not be enough to decrypt the data.
        /// </param>
        /// <param name="dataKeyExtraSaltValue">An Additional Salt value to be added to the key itself to perform the encryption.  
        /// Added as an extra layer of security so that merely having access to the securely stored settings alone would 
        /// not be enough to decrypt the data.
        /// </param>
        /// <param name="outputEncodingType">
        /// The string format in which to return the encrypted data.  
        /// Note that the standard to be used when writing to the Database is StringEncodingType.Base64url 
        /// as it is most compatible with the majority of DB engines and is both URL and File Name safe.
        /// </param>
        public void EncryptData(string dataToEncrypt,
                                string dataSecretKeyValue = "",
                                string dataKeySaltValue = "",
                                string dataKeyExtraSaltValue = "",
                                StringEncodingType outputEncodingType = StringEncodingType.Base64url)
        {
            //require at least one of the following
            if ((dataSecretKeyValue.Trim().Length + dataKeySaltValue.Trim().Length + dataKeyExtraSaltValue.Trim().Length) < 1)
            {
                throw new ArgumentNullException(nameof(dataSecretKeyValue),
                                                  "No Secret Key or Salt Values supplied for encryption.");
            }

            //If there is no data to encrypt, don't encrypt it
            if (dataToEncrypt.Length > 0)
            {
                //Calculate the weights to apply to the various keys and salts
                double _keysLen = dataSecretKeyValue.Length + dataKeySaltValue.Length + dataKeyExtraSaltValue.Length;
                double _keyLen = Math.Floor((dataSecretKeyValue.Length / _keysLen) * 24D);
                double _saltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);
                double _extraSaltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);

                //Add any rounding lefovers back to try and ensure that the non-code key is 24 characters long
                if (((int)_keyLen + (int)_saltLen + (int)_extraSaltLen) < 24)
                {
                    int _xtraLen = 24 - ((int)_keyLen + (int)_saltLen + (int)_extraSaltLen);
                    if (dataSecretKeyValue.Length >= ((int)_keyLen + _xtraLen))
                    {
                        _keyLen += _xtraLen;
                        _xtraLen = 0;
                    }
                    else if (_xtraLen > 2 && dataSecretKeyValue.Length >= ((int)_keyLen + 3))
                    {
                        _keyLen += 3;
                        _xtraLen -= 3;
                    }
                    else if (_xtraLen > 1 && dataSecretKeyValue.Length >= ((int)_keyLen + 2))
                    {
                        _keyLen += 2;
                        _xtraLen -= 2;
                    }
                    else if (_xtraLen > 0 && dataSecretKeyValue.Length >= ((int)_keyLen + 1))
                    {
                        _keyLen += 1;
                        _xtraLen -= 1;
                    }
                    if (_xtraLen > 0)
                    {
                        if (dataKeySaltValue.Length >= ((int)_saltLen + _xtraLen))
                        {
                            _saltLen += _xtraLen;
                            _xtraLen = 0;
                        }
                        else if (_xtraLen > 2 && dataKeySaltValue.Length >= ((int)_saltLen + 3))
                        {
                            _saltLen += 3;
                            _xtraLen -= 3;
                        }
                        else if (_xtraLen > 1 && dataKeySaltValue.Length >= ((int)_saltLen + 2))
                        {
                            _saltLen += 2;
                            _xtraLen -= 2;
                        }
                        else if (_xtraLen > 0 && dataKeySaltValue.Length >= ((int)_saltLen + 1))
                        {
                            _saltLen += 1;
                            _xtraLen -= 1;
                        }
                    }
                    if (_xtraLen > 0)
                    {
                        if (dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + _xtraLen))
                        {
                            _extraSaltLen += _xtraLen;
                        }
                        else if (_xtraLen > 2 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 3))
                        {
                            _extraSaltLen += 3;
                        }
                        else if (_xtraLen > 1 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 2))
                        {
                            _extraSaltLen += 2;
                        }
                        else if (_xtraLen > 0 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 1))
                        {
                            _extraSaltLen += 1;
                        }
                    }
                }

                //Generate the Encryption Key that will use the obfuscated key ensuring a 256 bit/64 byte/32 char key length.
                StringBuilder _dFiller = new(@"*TFTzsSoU7dYDqHf8T#pOrPw9TL7rrm*I_éàT<à@wcfl-1i#y9v7*à@1à6bb3711-éé3*20a5-é446=7à-b4*8c-f2a577_ac=b4''''cbmg<I9*à0éVWgnX-xyéfuàc*DV1jQY_XQO=éséLqA2S8ER*wgPGE4_b3àc=cd2a-abé5éeé-4488-96d6-6*ccè2=bc49fà6é43JSfè2W-zfPé=Yp9Vàhgé4*_UOsce10ENK=OWrzAQNRymI-*WObzPwfETAhgw");
                //Stringbuilder for performance

                StringBuilder _sbDataKey = new();
                if ((int)_keyLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataSecretKeyValue, (int)_keyLen, 0, true));
                }
                //add hardcoded salt for an extra layer of abstraction/obfuscation security
                //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
                _sbDataKey.Append('>');
                _sbDataKey.Append((char)87);
                if ((int)_saltLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataKeySaltValue, (int)_saltLen, 0, false, false, true));
                }
                _sbDataKey.Append((char)106);
                _sbDataKey.Append((char)57);
                _sbDataKey.Append(_dFiller.ToString().Substring(231, 31));
                if ((int)_extraSaltLen > 0)
                {
                    _sbDataKey.Insert(11, TrimOrPadText(dataKeyExtraSaltValue, (int)_extraSaltLen, 0, true));
                }
                _dFiller.Clear();   //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

                byte[] _useKey = Encoding.UTF8.GetBytes(_sbDataKey.ToString().Substring(0, 31));
                _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

                //Create the AES Encryption Cipher 
                Aes _cryptCipher = Aes.Create();
                _cryptCipher.Mode = CipherMode.CBC;
                _cryptCipher.KeySize = 256;
                _cryptCipher.Padding = PaddingMode.ISO10126;
                _cryptCipher.Key = _useKey;
                _cryptCipher.BlockSize = 128;

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
            else
            {
                //Populate Class Properties with defaults
                DecryptedValue = "";
                EncryptedValue = "";
                EncryptedInitializationVector = "";
                EncryptedValueEncodingType = outputEncodingType;
            }
        }

        /// <summary>
        /// Decrypt the requested data.
        /// </summary>
        /// <remarks>
        /// Returns the data in the Class Property fields.  
        /// NB: When reading from the Database, use the StringEncodingType.Base64url string format as a standard. 
        /// </remarks>
        /// <param name="dataToDecrypt">The string to Decrypt in UTF8 format.</param>
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

            //require at least one of the following
            if ((dataSecretKeyValue.Trim().Length + dataKeySaltValue.Trim().Length + dataKeyExtraSaltValue.Trim().Length) < 1)
            {
                throw new ArgumentNullException(nameof(dataSecretKeyValue),
                                                 "No Secret Key or Salt Values supplied for decryption.");
            }

            //Validate that we have an IV to perform the decryption
            if (useInitializationVectorValue?.ToString().Length == 0)
            {
                if (dataToDecrypt.Contains(";", StringComparison.CurrentCulture))
                {
                    useInitializationVectorValue = dataToDecrypt.Substring(dataToDecrypt.IndexOf(";") + 1, dataToDecrypt.Length - dataToDecrypt.IndexOf(";") - 1);
                    dataToDecrypt = dataToDecrypt.Substring(0, dataToDecrypt.IndexOf(";"));
                }
                else
                {
                    ArgumentNullException argumentNullException = new(nameof(useInitializationVectorValue),
                                                                       "Cipher Initialization Vector Value not supplied or found in requested data to decrypt, decryption of data not possible.");
                    throw argumentNullException;
                }
            }
            else if (dataToDecrypt.Contains(";", StringComparison.CurrentCulture))
            {
                if (useInitializationVectorValue == dataToDecrypt.Substring(dataToDecrypt.IndexOf(";") + 1, dataToDecrypt.Length - dataToDecrypt.IndexOf(";") - 1))
                {
                    dataToDecrypt = dataToDecrypt.Substring(0, dataToDecrypt.IndexOf(";"));
                }
                else
                {
                    throw new ArgumentException("Cipher Initialization Vector Value was supplied, but a different Initialization Vector was also found in the data to decrypt.  Decryption of data not possible.");
                }
            }

            //if there is no date to decrypt, don't decrypt it
            if (dataToDecrypt.Length > 0)
            {
                //Calculate the weights to apply to the various keys and salts
                double _keysLen = dataSecretKeyValue.Length + dataKeySaltValue.Length + dataKeyExtraSaltValue.Length;
                double _keyLen = Math.Floor((dataSecretKeyValue.Length / _keysLen) * 24D);
                double _saltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);
                double _extraSaltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);

                //Add any rounding lefovers back to try and ensure that the non-code key is 24 characters long
                if (((int)_keyLen + (int)_saltLen + (int)_extraSaltLen) < 24)
                {
                    int _xtraLen = 24 - ((int)_keyLen + (int)_saltLen + (int)_extraSaltLen);
                    if (dataSecretKeyValue.Length >= ((int)_keyLen + _xtraLen))
                    {
                        _keyLen += _xtraLen;
                        _xtraLen = 0;
                    }
                    else if (_xtraLen > 2 && dataSecretKeyValue.Length >= ((int)_keyLen + 3))
                    {
                        _keyLen += 3;
                        _xtraLen -= 3;
                    }
                    else if (_xtraLen > 1 && dataSecretKeyValue.Length >= ((int)_keyLen + 2))
                    {
                        _keyLen += 2;
                        _xtraLen -= 2;
                    }
                    else if (_xtraLen > 0 && dataSecretKeyValue.Length >= ((int)_keyLen + 1))
                    {
                        _keyLen += 1;
                        _xtraLen -= 1;
                    }
                    if (_xtraLen > 0)
                    {
                        if (dataKeySaltValue.Length >= ((int)_saltLen + _xtraLen))
                        {
                            _saltLen += _xtraLen;
                            _xtraLen = 0;
                        }
                        else if (_xtraLen > 2 && dataKeySaltValue.Length >= ((int)_saltLen + 3))
                        {
                            _saltLen += 3;
                            _xtraLen -= 3;
                        }
                        else if (_xtraLen > 1 && dataKeySaltValue.Length >= ((int)_saltLen + 2))
                        {
                            _saltLen += 2;
                            _xtraLen -= 2;
                        }
                        else if (_xtraLen > 0 && dataKeySaltValue.Length >= ((int)_saltLen + 1))
                        {
                            _saltLen += 1;
                            _xtraLen -= 1;
                        }
                    }
                    if (_xtraLen > 0)
                    {
                        if (dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + _xtraLen))
                        {
                            _extraSaltLen += _xtraLen;
                        }
                        else if (_xtraLen > 2 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 3))
                        {
                            _extraSaltLen += 3;
                        }
                        else if (_xtraLen > 1 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 2))
                        {
                            _extraSaltLen += 2;
                        }
                        else if (_xtraLen > 0 && dataKeyExtraSaltValue.Length >= ((int)_extraSaltLen + 1))
                        {
                            _extraSaltLen += 1;
                        }
                    }
                }

                //Generate the Encryption Key that will use the obfuscated key ensuring a 256 bit/64 byte/32 char key length.
                StringBuilder _dFiller = new(@"*TFTzsSoU7dYDqHf8T#pOrPw9TL7rrm*I_éàT<à@wcfl-1i#y9v7*à@1à6bb3711-éé3*20a5-é446=7à-b4*8c-f2a577_ac=b4''''cbmg<I9*à0éVWgnX-xyéfuàc*DV1jQY_XQO=éséLqA2S8ER*wgPGE4_b3àc=cd2a-abé5éeé-4488-96d6-6*ccè2=bc49fà6é43JSfè2W-zfPé=Yp9Vàhgé4*_UOsce10ENK=OWrzAQNRymI-*WObzPwfETAhgw");
                //Stringbuilder for performance

                StringBuilder _sbDataKey = new();
                if ((int)_keyLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataSecretKeyValue, (int)_keyLen, 0, true));
                }
                //add hardcoded salt for an extra layer of abstraction/obfuscation security
                //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
                _sbDataKey.Append('>');
                _sbDataKey.Append((char)87);
                if ((int)_saltLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataKeySaltValue, (int)_saltLen, 0, false, false, true));
                }
                _sbDataKey.Append((char)106);
                _sbDataKey.Append((char)57);
                _sbDataKey.Append(_dFiller.ToString().Substring(231, 31));
                if ((int)_extraSaltLen > 0)
                {
                    _sbDataKey.Insert(11, TrimOrPadText(dataKeyExtraSaltValue, (int)_extraSaltLen, 0, true));
                }
                _dFiller.Clear();   //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

                byte[] _useKey = Encoding.UTF8.GetBytes(_sbDataKey.ToString().Substring(0, 31));
                _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

                //Create the AES Decryption Cipher 
                Aes _cryptCipher = Aes.Create();
                _cryptCipher.Mode = CipherMode.CBC;
                _cryptCipher.KeySize = 256;
                _cryptCipher.Padding = PaddingMode.ISO10126;
                _cryptCipher.Key = _useKey;
                _cryptCipher.BlockSize = 128;
                _cryptCipher.IV = ConvertStringEncodingTypeToBytes(useInitializationVectorValue, StringEncodingType.Base64url);

                //Create the Decryptor, convert source data to bytes, and Decrypt
                ICryptoTransform _cryptService = _cryptCipher.CreateDecryptor();
                byte[] _bytesToDecrypt = ConvertStringEncodingTypeToBytes(dataToDecrypt, encryptedDataEncodingType);  //Encoding.UTF8.GetBytes(dataToDecrypt);
                try
                {
                    byte[] _decryptedBytes = _cryptService.TransformFinalBlock(_bytesToDecrypt, 0, _bytesToDecrypt.Length);
                    DecryptedValue = ConvertBytesToStringEncodingType(_decryptedBytes, StringEncodingType.UTF8);
                }
                catch (Exception ex)
                {
                    DecryptedValue = "";
                    if (ex.Message.ToLower().Contains("Padding is invalid and cannot be removed".ToLower(), StringComparison.CurrentCulture))
                    {
                        throw new Exception("Unable to Decrypt the data. Are you using the correct Keys, Salts and Initialization Vector?", ex);
                    }
                }

                //Populate Class Properties with Decrypted results
                EncryptedValue = dataToDecrypt;
                EncryptedInitializationVector = ConvertBytesToStringEncodingType(_cryptCipher.IV, StringEncodingType.Base64url);
                EncryptedValueEncodingType = encryptedDataEncodingType;
            }
            else
            {
                //Populate Class Properties with defaults
                DecryptedValue = "";
                EncryptedValue = "";
                EncryptedInitializationVector = "";
                EncryptedValueEncodingType = encryptedDataEncodingType;
            }
        }
        #endregion

        #region Cryptography Helpers
        /// <summary>
        /// Calculate a hash for the supplied value.
        /// </summary>
        /// <remarks>
        /// Calculates SHA512/HMACSHA512 hash.  
        /// NB: The Key Value as well as both salt values must be available to calculate a salted value for comparison.  
        /// Note: We don't have to worry about the key length.  
        /// The key can be any length. However, the recommended size is 64 bytes. 
        /// If the key is more than 64 bytes long, it is hashed (using SHA-512) to derive a 64-byte key, 
        /// if it is less than 64 bytes long, it is padded to 64 bytes.  
        /// </remarks>
        /// <param name="dataToHash">The source data/property value to hash in UTF8 format.</param>
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
        public static string CreateHash(string dataToHash,
                                            string hashDataSaltValue = "",
                                            string hashDataExtraSaltValue = "",
                                            string hashSecretKeyValue = "",
                                            string hashKeySaltValue = "",
                                            string hashKeyExtraSaltValue = "",
                                            StringEncodingType hashStringOutputType = StringEncodingType.HexStringShort)
            {

                StringBuilder _hFiller = new(@"UeQqg_NRqEàGtSP2oihULwĘgWbFTUBg9_DéeUNfDKssLZ_ßQgIQVcp6lvU°_MpEscATc1Eàg_oyqiVwYhE-ueFU6p6XZxA©wsR1fOJNkHG¾lim-kHUjPB1ĘAmzVaRnwzlf+NHFhiAZUD_Hàwj9ahg_4nsq¦rB7Sk4xhnoB§wd6di229YlXàJeLiZy-Og6B*w8ZZRUVEZMg¾90h3wplQzkAĘgd20tXgSqND*Rctcs2audIaéAahQDCAj0FY±UUcyzfMVu0p#w1DxqYSCiv3^rH-ttLP49Aq¦AbY5Y6KtJiC±C4Y3grs3LMS^AzlgLbTkreZ§8BghU0JcpzP±ACFkgcRx2IS.j8rTjq4eiDI^gV0IMxoT_80~hCb_Q86-bUd¢QXWY39p91O7ĘCgExIeUS_Ho~w-algWsMBrIĘl-nwM1-rWGbégx3MS1NzwShØaojo9tOtQUQ-A3L3--K87W3#VvUNBEDYg05±g4CWm7TIr49=WsS_YHuKQ5k_A1G9Va8Ff");

                //Stringbuilder for performance 
                StringBuilder _sb = new(hashSecretKeyValue);
                _sb.Append(hashKeySaltValue);

                //add hardcoded salt for an extra layer of abstraction/obfuscation security
                //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
                _sb.Append(@"_42*L²=@^ç");
                _sb.Append((char)95);
                _sb.Append((char)122);
                _sb.Append('è');
                _sb.Append((char)126);
                _sb.Append('&');
                _sb.Append((char)42);
                _sb.Append((char)92);
                _sb.Append((char)123);
                _sb.Append('-');
                _sb.Append((char)93);
                _sb.Append((char)72);
                _sb.Append((char)48);
                _sb.Append(hashKeyExtraSaltValue);
                _sb.Append(_hFiller.ToString().Substring(256, 128));
                _hFiller.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

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
        /// Use cryptographically strong random number generator to create a random salt text value.  
        /// </summary>
        /// <remarks>
        /// NB: This value should be stored when used for Hashing/Encryption as it is required to Validate the Hash/Decrypt the value.  
        /// </remarks>
        /// <param name="generatedStringLength">
        /// Force the generated string to this length.  
        /// Omit or 0 value outputs the string length based on the NumberOfBytes parameter.
        /// </param>
        /// <param name="insertSpecialCharacterEveryNumberOfCharacters">
        /// Optionally insert a random special character after every X number of characters in the string.  
        /// If randomizeNumberOfCharacters is true this is the maximum number for the random number range. 
        /// </param>
        /// <param name="randomizeNumberOfCharacters">
        /// Insert at random intervals between every 1 and insertSpecialCharacterEveryNumberOfCharacters characters.
        /// </param>
        /// <param name="minimumRandomNumberOfCharacters">
        /// The minimum number of characters to use when inserting characters art random intervals.  
        /// Useful because having a random character inserted ever 1-3 characters of a limited length string 
        /// actually makes is less secure.
        /// </param>
        /// <param name="SpecialCharactersToUseForInsert">
        /// Optional list of special characters to use for the insert.  e.g. @"$.#_^".  
        /// If not specified and Insert was requested, the preconfigured list will be used.
        /// </param>
        /// <param name="overrideOutputType">
        /// Add the special inserted characters to the output string.  
        /// NB: the selected stringOutputType will be unconvertable.
        /// If false and insertSpecialCharacterEveryNumberOfCharacters is true, 
        /// the special characters will be added to the salt before converting to the output type.
        /// </param>
        /// <param name="numberOfBytes">
        /// The number of bytes to use to generate the Salt.
        /// </param>
        /// <param name="stringOutputType">The format of the string to return.
        /// </param>
        /// <returns>
        /// The Random Salt generated as a string.
        /// </returns>
        public static string GenerateRandomSalt(StringEncodingType stringOutputType = StringEncodingType.Base64url,
                                                int generatedStringLength = 0,
                                                int insertSpecialCharacterEveryNumberOfCharacters = 0,
                                                bool randomizeNumberOfCharacters = false,
                                                int minimumRandomNumberOfCharacters = 0,
                                                string SpecialCharactersToUseForInsert = "",
                                                bool overrideOutputType = false,
                                                int numberOfBytes = 16)
        {
            byte[] newSalt = new byte[numberOfBytes];
            string _retStr;

            //validate parameters
            if (generatedStringLength < 0)
            {
                throw new Exception("generatedStringLength can't be negative.");
            }
            if (generatedStringLength > 0 && stringOutputType == StringEncodingType.HexStringShort)
            {
                if (generatedStringLength % 2 != 0)
                {
                    throw new Exception("The value of generatedStringLength must be an even number for stringOutputType StringEncodingType.HexStringShort.");
                }
            }
            if (generatedStringLength > 0 && stringOutputType == StringEncodingType.HexString)
            {
                if (generatedStringLength != 2)
                {
                    if ((generatedStringLength - 2) % 3 != 0)
                    {
                        throw new Exception("The value of generatedStringLength must be (2 + a multiple of 3) for stringOutputType StringEncodingType.HexString.");
                    }
                }
            }
            if (randomizeNumberOfCharacters && insertSpecialCharacterEveryNumberOfCharacters < 1)
            {
                throw new Exception("insertSpecialCharacterEveryNumberOfCharacters must have a positive value if randomizeNumberOfCharacters is true.");
            }
            if (generatedStringLength != 0 && (insertSpecialCharacterEveryNumberOfCharacters >= generatedStringLength))
            {
                throw new Exception("insertSpecialCharacterEveryNumberOfCharacters must be less than generatedStringLength.");
            }
            if (minimumRandomNumberOfCharacters > 0 && !randomizeNumberOfCharacters)
            {
                throw new Exception("minimumRandomNumberOfCharacters specified but randomizeNumberOfCharacters is false.");
            }
            if ((minimumRandomNumberOfCharacters > 0) &&
                ((generatedStringLength > 0) &&
                 (minimumRandomNumberOfCharacters >= generatedStringLength)))
            {
                throw new Exception("minimumRandomNumberOfCharacters must be greater than generatedStringLength.");
            }

            //set up special character list
            if (insertSpecialCharacterEveryNumberOfCharacters > 0 && SpecialCharactersToUseForInsert.Trim() == "")
            {
                SpecialCharactersToUseForInsert = @"é±&*à+çù€_^°è$¤²§%~`#£µ";
            }

            //generate the cryptographicqally strong salt value
            if (generatedStringLength > 0)
            {
                _retStr = "";
                do
                {
                    RNGCryptoServiceProvider rng = new();
                    rng.GetBytes(newSalt);
                    if (overrideOutputType)
                    {
                        _retStr += ConvertBytesToStringEncodingType(newSalt, stringOutputType);
                    }
                    else
                    {
                        _retStr += ConvertBytesToStringEncodingType(newSalt, StringEncodingType.UTF8);
                    }
                } while (_retStr.Length < generatedStringLength);
                _retStr = _retStr.Substring(0, generatedStringLength);
            }
            else
            {
                RNGCryptoServiceProvider rng = new();
                rng.GetBytes(newSalt);
                if (overrideOutputType)
                {
                    _retStr = ConvertBytesToStringEncodingType(newSalt, stringOutputType);
                }
                else
                {
                    _retStr = ConvertBytesToStringEncodingType(newSalt, StringEncodingType.UTF8);
                }
            }

            //insert special characters
            if (insertSpecialCharacterEveryNumberOfCharacters > 0)
            {
                int i = 0;
                Random _rndFrom = new();
                Random _rndTo = new((DateTime.Now.Millisecond * 100) +
                                    (DateTime.Now.Second * 10) +
                                    (DateTime.Now.Minute * 5) -
                                    (DateTime.Now.Hour * 2) +
                                    (DateTime.Now.Year - DateTime.Now.Month - DateTime.Now.DayOfYear));
                do
                {
                    if (randomizeNumberOfCharacters)
                    {
                        if (minimumRandomNumberOfCharacters > 0)
                        {
                            i += _rndTo.Next(minimumRandomNumberOfCharacters, insertSpecialCharacterEveryNumberOfCharacters);
                        }
                        else
                        {
                            i += _rndTo.Next(1, insertSpecialCharacterEveryNumberOfCharacters);
                        }
                    }
                    else
                    {
                        i += insertSpecialCharacterEveryNumberOfCharacters;
                    }
                    _retStr = _retStr.Insert(i - 1, SpecialCharactersToUseForInsert.Substring(_rndFrom.Next(0, SpecialCharactersToUseForInsert.Length - 1), 1));
                    i++;
                } while (i < (_retStr.Length - insertSpecialCharacterEveryNumberOfCharacters));

                if (!overrideOutputType)
                {
                    _retStr = ConvertStringEncodingType(_retStr, StringEncodingType.UTF8, stringOutputType);
                }

                if (generatedStringLength > 0)
                {
                    _retStr = _retStr.Substring(0, generatedStringLength);
                }
            }

            return _retStr;
        }
        #endregion
    }
}