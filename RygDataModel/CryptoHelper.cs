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
        /// <param name="dataKeySaltValue">
        /// A Salt value to be added to the key itself to perform the encryption.  
        /// Typically a value stored in the database unique to the record.
        /// Added as an extra layer of security so that merely having access to the securely stored settings alone 
        /// would not be enough to decrypt the data.
        /// </param>
        /// <param name="dataKeyPepperValue">
        /// An Additional Salt value to be added to the key itself to perform the encryption.  
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
                                string dataKeyPepperValue = "",
                                StringEncodingType outputEncodingType = StringEncodingType.Base64url)
        {
            //require at least one of the following
            if ((dataSecretKeyValue.Trim().Length + dataKeySaltValue.Trim().Length + dataKeyPepperValue.Trim().Length) < 1)
            {
                throw new ArgumentNullException(nameof(dataSecretKeyValue),
                                                  "No Secret Key or Salt Values supplied for encryption.");
            }

            //If there is no data to encrypt, don't encrypt it
            if (dataToEncrypt.Length > 0)
            {
                //Calculate the weights to apply to the various keys and salts
                double _keysLen = dataSecretKeyValue.Length + dataKeySaltValue.Length + dataKeyPepperValue.Length;
                double _keyLen = Math.Floor((dataSecretKeyValue.Length / _keysLen) * 24D);
                double _saltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);
                double _extraSaltLen = Math.Floor((dataKeyPepperValue.Length / _keysLen) * 24D);

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
                        if (dataKeyPepperValue.Length >= ((int)_extraSaltLen + _xtraLen))
                        {
                            _extraSaltLen += _xtraLen;
                        }
                        else if (_xtraLen > 2 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 3))
                        {
                            _extraSaltLen += 3;
                        }
                        else if (_xtraLen > 1 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 2))
                        {
                            _extraSaltLen += 2;
                        }
                        else if (_xtraLen > 0 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 1))
                        {
                            _extraSaltLen += 1;
                        }
                    }
                }

                //Generate the Encryption Key that will use the obfuscated key ensuring a 256 bit/32 char key length.
                StringBuilder _dFiller = new(@"-tCLµrjETY_A8Kkq²nLkPh*sKgIç3RG7!IoDz%Wp2Tl§t1pWàORXwWµ9XLlKùkqIr_3Q9D°a1AiJ±StQQTùfZmQ§zqUQV§cImr@OqukPôWQwcr²cpRko_9OwM±lNW5X±JgFZ%wxPq=rLRs6~UvBy-+R77LwètXQgF<E87C!MD9w°jfPgùuW6fA(tMVwçSjP6<RjIbçaTnMw°UmHtJèCRrAi(FKay_S2Pqdàjp9IYàMKgUa±6wcW~QAt9h@7jie=MQsEa%haRe=CgqJT!H8-yC*Q7HMèJj1Jp§B5b7%gqmd(67Ry_ùDLsqç1ZS8<80gm_%wavaQ²uXosRçj_RiG%647Ro_5Iw9ô2zrrT+BxFr+p2ISI~H_df_zGwY_iQ8J,5Y7s<n3WZ°Vahux+KelQX@iT7QI(v2gG+utH4w!C8zSV~NQ6c±FIlW=VZ0C(721Nrµ6gMw!hiwL4_bwbg*9s79@6PILt§Sgy3t_egF1~VGXZàbX64=p7vn§TRCzr!0QgT*");

                //Stringbuilder for performance
                StringBuilder _sbDataKey = new();
                if ((int)_saltLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataKeySaltValue, (int)_saltLen, 0, false, false, true));
                }

                //add hardcoded salt for an extra layer of abstraction/obfuscation security
                //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
                _sbDataKey.Append('>');
                _sbDataKey.Append((char)87);
                if ((int)_keyLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataSecretKeyValue, (int)_keyLen, 0, true));
                }
                _sbDataKey.Append((char)106);
                _sbDataKey.Append((char)57);
                _sbDataKey.Append(ReverseString(_dFiller.ToString().Substring(231, 31)));
                if ((int)_extraSaltLen > 0)
                {
                    _sbDataKey.Insert(11, TrimOrPadText(dataKeyPepperValue, (int)_extraSaltLen, 0, true));
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
        /// <param name="dataToDecrypt">
        /// The string to Decrypt in UTF8 format.
        /// </param>
        /// <param name="dataSecretKeyValue">
        /// The securely stored key to be seeded and used to perform the Decryption in UTF8 format.  
        /// NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 
        /// if it is stored in another format in your secured configuration.
        /// </param>
        /// <param name="useInitializationVectorValue">
        /// The StringEncodingType.Base64url formatted string cipher IV value to use to Decrypt the data.  
        /// Leave blank for the value to be calculated if the encrypted data contains the IV, typically the 
        /// case when using encrypted values read from the Database.
        /// </param>
        /// <param name="dataKeySaltValue">
        /// The Salt value to be added to the key itself to perform the Decryption.  
        /// Typically a value stored in the database unique to the record.
        /// NB: This must match the value originally used when encrypting the data.
        /// </param>
        /// <param name="dataKeyPepperValue">
        /// The Additional Salt value to be added to the key itself to perform the Decryption.  
        /// NB: This must match the value originally used when encrypting the data.
        /// </param>
        /// <param name="encryptedDataEncodingType">
        /// The string format of the Encrypted data.  
        /// Note that the standard to be used when reading/writing from/to the Database is StringEncodingType.Base64url 
        /// as it is most compatible with the majority of DB engines and is both URL and File Name safe.
        /// </param>
        public void DecryptData(string dataToDecrypt,
                                string dataSecretKeyValue = "",
                                string useInitializationVectorValue = "",
                                string dataKeySaltValue = "",
                                string dataKeyPepperValue = "",
                                StringEncodingType encryptedDataEncodingType = StringEncodingType.Base64url)
        {

            //require at least one of the following
            if ((dataSecretKeyValue.Trim().Length + dataKeySaltValue.Trim().Length + dataKeyPepperValue.Trim().Length) < 1)
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
                double _keysLen = dataSecretKeyValue.Length + dataKeySaltValue.Length + dataKeyPepperValue.Length;
                double _keyLen = Math.Floor((dataSecretKeyValue.Length / _keysLen) * 24D);
                double _saltLen = Math.Floor((dataKeySaltValue.Length / _keysLen) * 24D);
                double _extraSaltLen = Math.Floor((dataKeyPepperValue.Length / _keysLen) * 24D);

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
                        if (dataKeyPepperValue.Length >= ((int)_extraSaltLen + _xtraLen))
                        {
                            _extraSaltLen += _xtraLen;
                        }
                        else if (_xtraLen > 2 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 3))
                        {
                            _extraSaltLen += 3;
                        }
                        else if (_xtraLen > 1 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 2))
                        {
                            _extraSaltLen += 2;
                        }
                        else if (_xtraLen > 0 && dataKeyPepperValue.Length >= ((int)_extraSaltLen + 1))
                        {
                            _extraSaltLen += 1;
                        }
                    }
                }

                //Generate the Encryption Key that will use the obfuscated key ensuring a 256 bit/32 char key length.
                StringBuilder _dFiller = new(@"-tCLµrjETY_A8Kkq²nLkPh*sKgIç3RG7!IoDz%Wp2Tl§t1pWàORXwWµ9XLlKùkqIr_3Q9D°a1AiJ±StQQTùfZmQ§zqUQV§cImr@OqukPôWQwcr²cpRko_9OwM±lNW5X±JgFZ%wxPq=rLRs6~UvBy-+R77LwètXQgF<E87C!MD9w°jfPgùuW6fA(tMVwçSjP6<RjIbçaTnMw°UmHtJèCRrAi(FKay_S2Pqdàjp9IYàMKgUa±6wcW~QAt9h@7jie=MQsEa%haRe=CgqJT!H8-yC*Q7HMèJj1Jp§B5b7%gqmd(67Ry_ùDLsqç1ZS8<80gm_%wavaQ²uXosRçj_RiG%647Ro_5Iw9ô2zrrT+BxFr+p2ISI~H_df_zGwY_iQ8J,5Y7s<n3WZ°Vahux+KelQX@iT7QI(v2gG+utH4w!C8zSV~NQ6c±FIlW=VZ0C(721Nrµ6gMw!hiwL4_bwbg*9s79@6PILt§Sgy3t_egF1~VGXZàbX64=p7vn§TRCzr!0QgT*");

                //Stringbuilder for performance
                StringBuilder _sbDataKey = new();
                if ((int)_saltLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataKeySaltValue, (int)_saltLen, 0, false, false, true));
                }

                //add hardcoded salt for an extra layer of abstraction/obfuscation security
                //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
                _sbDataKey.Append('>');
                _sbDataKey.Append((char)87);
                if ((int)_keyLen > 0)
                {
                    _sbDataKey.Append(TrimOrPadText(dataSecretKeyValue, (int)_keyLen, 0, true));
                }
                _sbDataKey.Append((char)106);
                _sbDataKey.Append((char)57);
                _sbDataKey.Append(ReverseString(_dFiller.ToString().Substring(231, 31)));
                if ((int)_extraSaltLen > 0)
                {
                    _sbDataKey.Insert(11, TrimOrPadText(dataKeyPepperValue, (int)_extraSaltLen, 0, true));
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
        /// Calculate a SHA256/HMACSHA256 hash for the supplied value.
        /// </summary>
        /// <remarks>
        /// NB: The Key Value as well as both salt values must be available to calculate a salted value for comparison.  
        /// Note: We don't have to worry about the key length.  
        /// The key can be any length. However, the recommended size is 64 bytes. 
        /// If the key is more than 64 bytes long, it is hashed (using SHA-256) to derive a 64-byte key, 
        /// if it is less than 64 bytes long, it is padded to 64 bytes.  
        /// </remarks>
        /// <param name="dataToHash">
        /// The source data/property value to hash in UTF8 format.
        /// </param>
        /// <param name="hashDataSaltValue">
        /// The main Salt to be added to the actual data before hashing.
        /// This value is typically stored in the database and is unique to the record.
        /// </param>
        /// <param name="hashDataPepperValue">
        /// An additional Salt to be added to the actual data before hashing.
        /// </param>
        /// <param name="hashSecretKeyValue">
        /// The securely stored key to be seeded and used to perform the hash in UTF8 format.  
        /// NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is 
        /// stored in another format in your secured configuration.
        /// </param>
        /// <param name="hashKeySaltValue">
        /// A Salt value to be added to the key itself to perform the hash.  
        /// This value is typically stored in the database and is unique to the record.
        /// Added as an extra layer of security so that merely having access to the securely stored 
        /// settings alone would not be enough to calculate the hash.
        /// </param>
        /// <param name="hashKeyPepperValue">
        /// An additional Salt value to be added to the key itself to perform the hash.  
        /// Added as an extra layer of security so that merely having access to the securely 
        /// stored settings alone would not be enough to calculate the hash.
        /// </param>
        /// <param name="hashStringOutputType">
        /// The StringEncodingType format in which to return the hashed value.
        /// </param>
        /// <returns>
        /// Returns the hashed value in the requested StringEncodingType string format.  
        /// Calculates 256 bits (8 bits/byte, 32 bytes) for the hash, actual string length returned is determined by the requested output format.
        /// By default using StringEncodingType.HexStringShort it returns a 64 character string (32 bytes x 2 for hex).  
        /// </returns>
        public static string CreateHash256(string dataToHash,
                                           string hashDataSaltValue = "",
                                           string hashDataPepperValue = "",
                                           string hashSecretKeyValue = "",
                                           string hashKeySaltValue = "",
                                           string hashKeyPepperValue = "",
                                           StringEncodingType hashStringOutputType = StringEncodingType.HexStringShort)
        {
            //Calculate the weights to apply to the various keys and salts
            double _keysLen = hashSecretKeyValue.Length + hashKeySaltValue.Length + hashKeyPepperValue.Length;
            double _keyLen = Math.Floor((hashSecretKeyValue.Length / _keysLen) * 24D);
            double _saltLen = Math.Floor((hashKeySaltValue.Length / _keysLen) * 24D);
            double _extraSaltLen = Math.Floor((hashKeyPepperValue.Length / _keysLen) * 24D);

            //Add any rounding lefovers back to try and ensure that the non-code key is 24 characters long
            if (((int)_keyLen + (int)_saltLen + (int)_extraSaltLen) < 24)
            {
                int _xtraLen = 24 - ((int)_keyLen + (int)_saltLen + (int)_extraSaltLen);
                if (hashSecretKeyValue.Length >= ((int)_keyLen + _xtraLen))
                {
                    _keyLen += _xtraLen;
                    _xtraLen = 0;
                }
                else if (_xtraLen > 2 && hashSecretKeyValue.Length >= ((int)_keyLen + 3))
                {
                    _keyLen += 3;
                    _xtraLen -= 3;
                }
                else if (_xtraLen > 1 && hashSecretKeyValue.Length >= ((int)_keyLen + 2))
                {
                    _keyLen += 2;
                    _xtraLen -= 2;
                }
                else if (_xtraLen > 0 && hashSecretKeyValue.Length >= ((int)_keyLen + 1))
                {
                    _keyLen += 1;
                    _xtraLen -= 1;
                }
                if (_xtraLen > 0)
                {
                    if (hashKeySaltValue.Length >= ((int)_saltLen + _xtraLen))
                    {
                        _saltLen += _xtraLen;
                        _xtraLen = 0;
                    }
                    else if (_xtraLen > 2 && hashKeySaltValue.Length >= ((int)_saltLen + 3))
                    {
                        _saltLen += 3;
                        _xtraLen -= 3;
                    }
                    else if (_xtraLen > 1 && hashKeySaltValue.Length >= ((int)_saltLen + 2))
                    {
                        _saltLen += 2;
                        _xtraLen -= 2;
                    }
                    else if (_xtraLen > 0 && hashKeySaltValue.Length >= ((int)_saltLen + 1))
                    {
                        _saltLen += 1;
                        _xtraLen -= 1;
                    }
                }
                if (_xtraLen > 0)
                {
                    if (hashKeyPepperValue.Length >= ((int)_extraSaltLen + _xtraLen))
                    {
                        _extraSaltLen += _xtraLen;
                    }
                    else if (_xtraLen > 2 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 3))
                    {
                        _extraSaltLen += 3;
                    }
                    else if (_xtraLen > 1 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 2))
                    {
                        _extraSaltLen += 2;
                    }
                    else if (_xtraLen > 0 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 1))
                    {
                        _extraSaltLen += 1;
                    }
                }
            }

            //Generate the Encryption Key that will use the obfuscated key ensuring a 512 bit/64 byte char key length.
            StringBuilder _hFiller = new(@"dgS<zmAW=_UGH=GYmJ!Qfiv§zog2ùYfSJ²04zXàOLmWùA6vV!9oH6àg9Nh!k66p<pcnj!2TN4èqpIA@-Ogs%ist8ç6y8F§Ai6uôfB_IùQITx~wXkièrCTTµRhIP_LaI5è_CiF°WiwF,v3ai%R9hQ±0sY__I60O*glatçAJFY!mWh2<VsrWçC3fVùkZMt,_PQZôO9p4èUcG0èy-9Sè5gYV²kLza§ggyd±absp°O_H7+u0wBùyaqAôBVAu,V9Y5*gjKcôzAnl_xJ8RèYpYi%gdsp<1L-B%WHMJ!Ngix<xj4j±bnAl+UKMV±vvjwçkLUPç0u8f±mzBDçwFcdçUhdm²yPIQàqID1°KQlY_FjgU*nrOS§VJB4àzvQL~ZxGv<cEPN_gvrf±OJxZçFjy1çRWmBù5TFs<7gArçak26°7k05*QfN8~M4pmè7cfd§gSWA+IPgC(9l5F<OXqN§u_lP=IcQj§sdREµYfn1§wuH-,pMpP§Wx98§AZDQùVJ_6_1w9u²226");

            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new();
            if ((int)_saltLen > 0)
            {
                _sbDataKey.Append(TrimOrPadText(hashKeySaltValue, (int)_saltLen, 0, false, false, true));
            }
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append('ù');
            _sbDataKey.Append((char)99);
            if ((int)_keyLen > 0)
            {
                _sbDataKey.Append(TrimOrPadText(hashSecretKeyValue, (int)_keyLen, 0, true));
            }
            _sbDataKey.Append((char)174);
            _sbDataKey.Append('ö');
            _sbDataKey.Append((char)83);
            _sbDataKey.Append(ReverseString(_hFiller.ToString().Substring(92, 31)));
            _hFiller.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            if ((int)_extraSaltLen > 0)
            {
                _sbDataKey.Insert(9, TrimOrPadText(hashKeyPepperValue, (int)_extraSaltLen, 0, true));
            }

            //instantiate the class with supplied key and salts
            HashAlgorithm _hashService;
            if (hashSecretKeyValue == "")
            {
                _hashService = SHA256.Create(); //yep, aware that this code will not run but leaving it here as a placeholder should it be useful in future.
            }
            else
            {
                _hashService = new HMACSHA256(Encoding.UTF8.GetBytes(_sbDataKey.ToString().Substring(0, 31)));
            }

            //Salt the hash data itself (only done with actual *data* when _hashing_)
            _sbDataKey.Clear();
            _sbDataKey.Append(hashDataSaltValue);
            _sbDataKey.Append(dataToHash);
            _sbDataKey.Append(hashDataPepperValue);

            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            return ConvertBytesToStringEncodingType(_hashResult, hashStringOutputType);
        }
        /// <summary>
        /// Calculate a SHA512/HMACSHA512 hash for the supplied value.
        /// </summary>
        /// <remarks>
        /// NB: The Key Value as well as both salt values must be available to calculate a salted value for comparison.  
        /// Note: We don't have to worry about the key length.  
        /// The key can be any length. However, the recommended size is 128 bytes. 
        /// If the key is more than 128 bytes long, it is hashed (using SHA-512) to derive a 128-byte key, 
        /// if it is less than 128 bytes long, it is padded to 128 bytes.  
        /// </remarks>
        /// <param name="dataToHash">
        /// The source data/property value to hash in UTF8 format.
        /// </param>
        /// <param name="hashDataSaltValue">
        /// The main Salt to be added to the actual data before hashing.  
        /// Typically a value unique to the record stored in the database.
        /// </param>
        /// <param name="hashDataPepperValue">
        /// An additional Salt to be added to the actual data before hashing.
        /// </param>
        /// <param name="hashSecretKeyValue">
        /// The securely stored key to be seeded and used to perform the hash in UTF8 format.  
        /// NB: Use the appropriate ModelHelper/Text conversion method to translate the value to UTF8 if it is stored 
        /// in another format in your secured configuration.
        /// </param>
        /// <param name="hashKeySaltValue">
        /// A Salt value to be added to the key itself to perform the hash.  
        /// Typically a value stored in the database unique to the record.
        /// Added as an extra layer of security so that merely having access to the securely stored settings alone would not be enough to calculate the hash.
        /// </param>
        /// <param name="hashKeyPepperValue">
        /// An additional Salt value to be added to the key itself to perform the hash.  
        /// Added as an extra layer of security so that merely having access to the securely stored 
        /// settings alone would not be enough to calculate the hash.
        /// </param>
        /// <param name="hashStringOutputType">
        /// The StringEncodingType format in which to return the hashed value.
        /// </param>
        /// <returns>
        /// Returns the hashed value in the requested StringEncodingType string format.  
        /// Calculates 512 bits (8 bits/byte, 64 bytes) for the hash, actual string length returned is determined by the requested output format.
        /// By default using StringEncodingType.HexStringShort it returns a 128 character string (64 bytes x 2 for hex).  
        /// </returns>
        public static string CreateHash512(string dataToHash,
                                           string hashDataSaltValue = "",
                                           string hashDataPepperValue = "",
                                           string hashSecretKeyValue = "",
                                           string hashKeySaltValue = "",
                                           string hashKeyPepperValue = "",
                                           StringEncodingType hashStringOutputType = StringEncodingType.HexStringShort)
        {
            //Calculate the weights to apply to the various keys and salts
            double _keysLen = hashSecretKeyValue.Length + hashKeySaltValue.Length + hashKeyPepperValue.Length;
            double _keyLen = Math.Floor((hashSecretKeyValue.Length / _keysLen) * 48D);
            double _saltLen = Math.Floor((hashKeySaltValue.Length / _keysLen) * 48D);
            double _extraSaltLen = Math.Floor((hashKeyPepperValue.Length / _keysLen) * 48D);

            //Add any rounding lefovers back to try and ensure that the non-code key is 24 characters long
            if (((int)_keyLen + (int)_saltLen + (int)_extraSaltLen) < 48)
            {
                int _xtraLen = 48 - ((int)_keyLen + (int)_saltLen + (int)_extraSaltLen);
                if (hashSecretKeyValue.Length >= ((int)_keyLen + _xtraLen))
                {
                    _keyLen += _xtraLen;
                    _xtraLen = 0;
                }
                else if (_xtraLen > 2 && hashSecretKeyValue.Length >= ((int)_keyLen + 3))
                {
                    _keyLen += 3;
                    _xtraLen -= 3;
                }
                else if (_xtraLen > 1 && hashSecretKeyValue.Length >= ((int)_keyLen + 2))
                {
                    _keyLen += 2;
                    _xtraLen -= 2;
                }
                else if (_xtraLen > 0 && hashSecretKeyValue.Length >= ((int)_keyLen + 1))
                {
                    _keyLen += 1;
                    _xtraLen -= 1;
                }
                if (_xtraLen > 0)
                {
                    if (hashKeySaltValue.Length >= ((int)_saltLen + _xtraLen))
                    {
                        _saltLen += _xtraLen;
                        _xtraLen = 0;
                    }
                    else if (_xtraLen > 2 && hashKeySaltValue.Length >= ((int)_saltLen + 3))
                    {
                        _saltLen += 3;
                        _xtraLen -= 3;
                    }
                    else if (_xtraLen > 1 && hashKeySaltValue.Length >= ((int)_saltLen + 2))
                    {
                        _saltLen += 2;
                        _xtraLen -= 2;
                    }
                    else if (_xtraLen > 0 && hashKeySaltValue.Length >= ((int)_saltLen + 1))
                    {
                        _saltLen += 1;
                        _xtraLen -= 1;
                    }
                }
                if (_xtraLen > 0)
                {
                    if (hashKeyPepperValue.Length >= ((int)_extraSaltLen + _xtraLen))
                    {
                        _extraSaltLen += _xtraLen;
                    }
                    else if (_xtraLen > 2 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 3))
                    {
                        _extraSaltLen += 3;
                    }
                    else if (_xtraLen > 1 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 2))
                    {
                        _extraSaltLen += 2;
                    }
                    else if (_xtraLen > 0 && hashKeyPepperValue.Length >= ((int)_extraSaltLen + 1))
                    {
                        _extraSaltLen += 1;
                    }
                }
            }

            //Generate the Encryption Key that will use the obfuscated key ensuring a 512 bit/64 byte char key length.
            StringBuilder _hFiller = new(@"N0Oç0TPB^qK6N-sChQ^v-K0#F5wI#I76J*CVDA#gYjs_78xX=h2CKâwhre-yiAqËnBIr_Ulh9!5Fpv!0eQDéNyLg-j4SS!3SyKµfsQQ$9sZN²gkrK^tB54^HakF!hiwpéoVGAâhzAY_O1yp°xXVr%aUa-#IwgH£iAUvèQydI!65Ir%T4Iv-1B2w-mCC5#3EAC=TK45èYc1n!p9ZX!K_hD.-8snéQHst$sEfE#5gEo-RCYCËBgeOéurgEµsv3y^-PzX$1DPn^L-h3µwd5J²g2BN^I55gè8CK-!cJe7é_KQF-Rfgd£qpTv_kmnR£x-7wç-iDKçin9R¹gYFr#SVGKèW8yxçMowWµvRzQçV_wG#Fr_u%gcOd!FtR4-kPr4£3bL9*wUax!-fSi%blffµqQRZË1tc1_oJwX£nXik£HlQO*TYSV=h7ve°9fXL^QdJ9ËQnsiçqlIy#1CAx$DPBp-DHAN*vZsJ%B1RAçQrpc+rt3E£T2piµgqb8-yWesèEDCJ-7YE");

            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new();
            if ((int)_saltLen > 0)
            {
                _sbDataKey.Append(TrimOrPadText(hashKeySaltValue, (int)_saltLen, 0, false, false, true));
            }
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append('¤');
            _sbDataKey.Append((char)72);
            if ((int)_keyLen > 0)
            {
                _sbDataKey.Append(TrimOrPadText(hashSecretKeyValue, (int)_keyLen, 0, true));
            }
            _sbDataKey.Append((char)151);
            _sbDataKey.Append('ö');
            _sbDataKey.Append((char)42);
            _sbDataKey.Append(ReverseString(_hFiller.ToString().Substring(92, 63)));
            _hFiller.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            if ((int)_extraSaltLen > 0)
            {
                _sbDataKey.Insert(11, TrimOrPadText(hashKeyPepperValue, (int)_extraSaltLen, 0, true));
            }

            //instantiate the class with supplied key and salts
            HashAlgorithm _hashService;
            if (hashSecretKeyValue == "")
            {
                _hashService = SHA512.Create(); //yep, aware that this code will not run but leaving it here as a placeholder should it be useful in future.
            }
            else
            {
                _hashService = new HMACSHA512(Encoding.UTF8.GetBytes(_sbDataKey.ToString().Substring(0, 63)));
            }

            //Salt the hash data itself (only done with actual *data* when _hashing_)
            _sbDataKey.Clear();
            _sbDataKey.Append(hashDataSaltValue);
            _sbDataKey.Append(dataToHash);
            _sbDataKey.Append(hashDataPepperValue);

            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

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