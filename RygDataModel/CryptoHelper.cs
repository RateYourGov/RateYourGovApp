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
        /// <param name="dataToEncrypt">The string to encrypt in UTF8 format.</param>
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
            StringBuilder _dFiller = new(@"95qG~U6lRy_x6y0R-82GjK§jYghL-Gbsbj°mUcgq@BSu8G,Brguw#xTM2--DBAHr©lNDmd©nBVkVØ8QeoT@giNqA~Y69-9°CEIAUØcPpwa±Z5OEG@HSB6ZßqqYASØVoEug,gmlfY²JIyra»Ghpuc§85WV3©r8gnK+BfXVa°zZ4Jf*byTDSßQxdIQ°4LBHV,XFuWEßmKhmR°k8kPB²6gnwq+fuSu3±OMosFĘTE5WeØrd_g9#72WZwØXpjCC»CqZND±QpFwl~wYq-o*b2HDR^yxSoB±70X2_,oEA2U+m1wh4_CY52T~LNdHY»ofzAQ+X7scC~NHFrf,HmXMv©vXw-y@Cwfkw,nuEkV#QcslI©3N-nl~vTtAm²AXd67~Fz4do^uAmBz»n-SSV^g-83hØ1HMuN@Y73wO,G0qKo@jbQ0C*lLFxbØVGKc3°l3BSr@5nGrA@CYPF5ĘJKxC-@ZUirp^X7r5r_AQFqp^txqiV+RcgqX,MIwMO~i14AA_m1usJ-yxw");
            StringBuilder _hFiller = new(@"symJ6#YyYXPSà5lWk5F©oOrYwgßI7mIh7ßgaBY_o©bp2DNC©GaQq64°Px8Uml*t5y8AiĘ00X_ml@QgxfRK±hSEi5m.l-wW1U©ZWTMQRĘ6wgXWr°2q7fruĘjDeCEQ²8bg8Kg-g3vh-y~De9C41àHUwHjW*gybL5-éIh57wc,iObr6MĘCVErw7ØCTaCygĘ2iPOVW_SgROIr°vMg1ufà4pkqpAØ03ov1W^jk0Lff°wSMxiT_hQD5lw,PdGfFy.bb90gAĘFockKI§DF-4ab-KmVzGO°QOAZuq_xgF7jQ°d2ylly#HRb2DF+Aqnz-1»qVoheG§ODnFed+tObegv.1RWSJK#kzqsWy±SryAtyß_RA6cC+qV1CjJ§JVltK1ØcJ49OMàgWHojK©yhfCgT°NVCrFS@Nj4yAMĘNO6VVE@sPrFh-©JneoBW@dmwLF0#Nomzdv.399829éaloiks^wd0VTM»3AYUuwØxkxUGz@roQOw6_nUqVdn°pAz8bW©o02V4eàk2");

            //Generate the *actual* Encryption Key that will be used using a hashed value of the obfuscated password to ensure 256 bit key length
            //1. Create the Key value to be hashed
            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new(dataSecretKeyValue);
            _sbDataKey.Append(dataKeySaltValue);
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append(@"aZb_|@#é*.dD");
            _sbDataKey.Append((char)87);
            _sbDataKey.Append((char)37);
            _sbDataKey.Append((char)44);
            _sbDataKey.Append('5');
            _sbDataKey.Append((char)115);
            _sbDataKey.Append((char)59);
            _sbDataKey.Append('^');
            _sbDataKey.Append((char)101);
            _sbDataKey.Append((char)125);
            _sbDataKey.Append((char)77);
            _sbDataKey.Append((char)83);
            _sbDataKey.Append('_');
            _sbDataKey.Append((char)57);
            _sbDataKey.Append((char)106);
            _sbDataKey.Append('@');
            _sbDataKey.Append((char)45);
            _sbDataKey.Append((char)66);
            _sbDataKey.Append(dataKeyExtraSaltValue);
            _sbDataKey.Append(_dFiller.ToString().Substring(72, 128));
            _dFiller.Clear();   //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //2. Create the Key value to be used by the hashing function 
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            StringBuilder _sbHashKey = new(@"&&~§y_xO^..*-+=");
            _sbHashKey.Append((char)118);
            _sbHashKey.Append((char)62);
            _sbHashKey.Append((char)69);
            _sbHashKey.Append('-');
            _sbHashKey.Append((char)55);
            _sbHashKey.Append((char)47);
            _sbHashKey.Append('%');
            _sbHashKey.Append((char)78);
            _sbHashKey.Append((char)49);
            _sbHashKey.Append((char)70);
            _sbHashKey.Append('²');
            _sbHashKey.Append((char)99);
            _sbHashKey.Append((char)101);
            _sbHashKey.Append((char)87);
            _sbHashKey.Append('|');
            _sbHashKey.Append((char)111);
            _sbHashKey.Append((char)91);
            _sbHashKey.Append(dataSecretKeyValue);
            _sbHashKey.Append(_hFiller.ToString().Substring(128, 128));
            _hFiller.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Hash the result to create a tamper proof 256bit value using a 128 byte key for efficiency to avoid SHA-256 hashing to derive the correct key length
            HashAlgorithm _hashService = new HMACSHA256(Encoding.UTF8.GetBytes(_sbHashKey.ToString().Substring(2, 128)));
            _sbHashKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Create the AES Encryption Cipher 
            Aes _cryptCipher = Aes.Create();
            _cryptCipher.Mode = CipherMode.CBC;
            _cryptCipher.KeySize = 256;
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
                    throw new ApplicationException("Cipher Initialization Vector Value not supplied or found in requested data to decrypt, decryption of data not possible.");
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
                    throw new ApplicationException("Cipher Initialization Vector Value was supplied, but a different Initialization Vector was also found in the data to decrypt.  Decryption of data not possible.");
                }
            }
            StringBuilder _dFiller = new(@"95qG~U6lRy_x6y0R-82GjK§jYghL-Gbsbj°mUcgq@BSu8G,Brguw#xTM2--DBAHr©lNDmd©nBVkVØ8QeoT@giNqA~Y69-9°CEIAUØcPpwa±Z5OEG@HSB6ZßqqYASØVoEug,gmlfY²JIyra»Ghpuc§85WV3©r8gnK+BfXVa°zZ4Jf*byTDSßQxdIQ°4LBHV,XFuWEßmKhmR°k8kPB²6gnwq+fuSu3±OMosFĘTE5WeØrd_g9#72WZwØXpjCC»CqZND±QpFwl~wYq-o*b2HDR^yxSoB±70X2_,oEA2U+m1wh4_CY52T~LNdHY»ofzAQ+X7scC~NHFrf,HmXMv©vXw-y@Cwfkw,nuEkV#QcslI©3N-nl~vTtAm²AXd67~Fz4do^uAmBz»n-SSV^g-83hØ1HMuN@Y73wO,G0qKo@jbQ0C*lLFxbØVGKc3°l3BSr@5nGrA@CYPF5ĘJKxC-@ZUirp^X7r5r_AQFqp^txqiV+RcgqX,MIwMO~i14AA_m1usJ-yxw");
            StringBuilder _hFiller = new(@"symJ6#YyYXPSà5lWk5F©oOrYwgßI7mIh7ßgaBY_o©bp2DNC©GaQq64°Px8Uml*t5y8AiĘ00X_ml@QgxfRK±hSEi5m.l-wW1U©ZWTMQRĘ6wgXWr°2q7fruĘjDeCEQ²8bg8Kg-g3vh-y~De9C41àHUwHjW*gybL5-éIh57wc,iObr6MĘCVErw7ØCTaCygĘ2iPOVW_SgROIr°vMg1ufà4pkqpAØ03ov1W^jk0Lff°wSMxiT_hQD5lw,PdGfFy.bb90gAĘFockKI§DF-4ab-KmVzGO°QOAZuq_xgF7jQ°d2ylly#HRb2DF+Aqnz-1»qVoheG§ODnFed+tObegv.1RWSJK#kzqsWy±SryAtyß_RA6cC+qV1CjJ§JVltK1ØcJ49OMàgWHojK©yhfCgT°NVCrFS@Nj4yAMĘNO6VVE@sPrFh-©JneoBW@dmwLF0#Nomzdv.399829éaloiks^wd0VTM»3AYUuwØxkxUGz@roQOw6_nUqVdn°pAz8bW©o02V4eàk2");

            //Generate the *actual* Decryption Key that will be used using a hashed value of the obfuscated password to ensure 256 bit key length
            //1. Create the Key value to be hashed
            //Stringbuilder for performance 
            StringBuilder _sbDataKey = new(dataSecretKeyValue);
            _sbDataKey.Append(dataKeySaltValue);
            //add hardcoded salt for an extra layer of abstraction/obfuscation security
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            _sbDataKey.Append(@"aZb_|@#é*.dD");
            _sbDataKey.Append((char)87);
            _sbDataKey.Append((char)37);
            _sbDataKey.Append((char)44);
            _sbDataKey.Append('5');
            _sbDataKey.Append((char)115);
            _sbDataKey.Append((char)59);
            _sbDataKey.Append('^');
            _sbDataKey.Append((char)101);
            _sbDataKey.Append((char)125);
            _sbDataKey.Append((char)77);
            _sbDataKey.Append((char)83);
            _sbDataKey.Append('_');
            _sbDataKey.Append((char)57);
            _sbDataKey.Append((char)106);
            _sbDataKey.Append('@');
            _sbDataKey.Append((char)45);
            _sbDataKey.Append((char)66);
            _sbDataKey.Append(dataKeyExtraSaltValue);
            _sbDataKey.Append(_dFiller.ToString().Substring(72, 128));
            _dFiller.Clear();   //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //2. Create the Key value to be used by the hashing function 
            //NB: Do NOT change this code as it will render data generated by open source users of the software useless.
            StringBuilder _sbHashKey = new(@"&&~§y_xO^..*-+=");
            _sbHashKey.Append((char)118);
            _sbHashKey.Append((char)62);
            _sbHashKey.Append((char)69);
            _sbHashKey.Append('-');
            _sbHashKey.Append((char)55);
            _sbHashKey.Append((char)47);
            _sbHashKey.Append('%');
            _sbHashKey.Append((char)78);
            _sbHashKey.Append((char)49);
            _sbHashKey.Append((char)70);
            _sbHashKey.Append('²');
            _sbHashKey.Append((char)99);
            _sbHashKey.Append((char)101);
            _sbHashKey.Append((char)87);
            _sbHashKey.Append('|');
            _sbHashKey.Append((char)111);
            _sbHashKey.Append((char)91);
            _sbHashKey.Append(dataSecretKeyValue);
            _sbHashKey.Append(_hFiller.ToString().Substring(128, 128));
            _hFiller.Clear();    //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Hash the result to create a tamper proof 256 bit value using a 128 byte key for efficiency to avoid SHA-256 hashing to derive the correct key length
            HashAlgorithm _hashService = new HMACSHA256(Encoding.UTF8.GetBytes(_sbHashKey.ToString().Substring(2, 128)));
            _sbHashKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.
            byte[] _hashResult = _hashService.ComputeHash(Encoding.UTF8.GetBytes(_sbDataKey.ToString()));
            _sbDataKey.Clear();  //clearing in case of a crash, prefer not to have the value hanging around in memory any longer than needed.

            //Create the AES Decryption Cipher 
            Aes _cryptCipher = Aes.Create();
            _cryptCipher.Mode = CipherMode.CBC;
            _cryptCipher.KeySize = 256;
            _cryptCipher.Padding = PaddingMode.ISO10126;
            _cryptCipher.Key = _hashResult;
            _cryptCipher.IV = ConvertStringEncodingTypeToBytes(useInitializationVectorValue, StringEncodingType.Base64url);

            //Create the Decryptor, convert source data to bytes, and Decrypt
            ICryptoTransform _cryptService = _cryptCipher.CreateDecryptor();
            byte[] _bytesToDecrypt = ConvertStringEncodingTypeToBytes(dataToDecrypt, encryptedDataEncodingType);  //Encoding.UTF8.GetBytes(dataToDecrypt);
            byte[] _decryptedBytes = _cryptService.TransformFinalBlock(_bytesToDecrypt, 0, _bytesToDecrypt.Length);

            //Populate Class Properties with Decrypted results
            DecryptedValue = ConvertBytesToStringEncodingType(_decryptedBytes, StringEncodingType.UTF8);
            EncryptedValue = dataToDecrypt;
            EncryptedInitializationVector = ConvertBytesToStringEncodingType(_cryptCipher.IV, StringEncodingType.Base64url);
            EncryptedValueEncodingType = encryptedDataEncodingType;

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
        /// <param name="numberOfBytes">The number of bytes to use to generate the Salt.</param>
        /// <param name="stringOutputType">The format of the string to return.</param>
        /// <param name="generatedStringLength">Force the generated string to this length.  Omit or 0 value outputs the string length based on the NumberOfBytes parameter.</param>
        /// <param name="insertSpecialCharacterEveryNumberOfCharacters">Optionally insert a random special character after every X number of characters in the string.</param>
        /// <param name="overrideOutputType">Show the special inserted characters in the output string, NB: the selected stringOutputType will be unconvertable.</param>
        /// <param name="SpecialCharactersToUseForInsert">Optional list of special characters to use for the insert.  e.g. @"$.#_^".  If not specified and Insert was requested, the preconfigured list will be used.</param>
        /// <returns></returns>
        public static string GenerateRandomSalt(int numberOfBytes = 16,
                                                StringEncodingType stringOutputType = StringEncodingType.Base64url,
                                                int generatedStringLength = 0,
                                                int insertSpecialCharacterEveryNumberOfCharacters = 0,
                                                bool overrideOutputType = false,
                                                string SpecialCharactersToUseForInsert = "")
        {
            byte[] newSalt = new byte[numberOfBytes];
            string _retStr;

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

            if (insertSpecialCharacterEveryNumberOfCharacters > 0 && SpecialCharactersToUseForInsert.Trim() == "")
            {
                SpecialCharactersToUseForInsert = @"=#^@é¾ß¦§à*+~²±%_°©ØàßĘ¢";
            }

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

            if (insertSpecialCharacterEveryNumberOfCharacters > 0 &&
                _retStr.Length > 0 &&
                _retStr.Length >= insertSpecialCharacterEveryNumberOfCharacters)
            {
                int i = 0;
                Random _rnd = new((DateTime.Now.Millisecond * 100) + (DateTime.Now.Second * 10) + (DateTime.Now.Minute * 5) - (DateTime.Now.Hour * 2) + (DateTime.Now.Year - DateTime.Now.Month));
                do
                {
                    i += insertSpecialCharacterEveryNumberOfCharacters;
                    _retStr = _retStr.Insert(i - 1, SpecialCharactersToUseForInsert.Substring(_rnd.Next(0, SpecialCharactersToUseForInsert.Length - 1), 1));
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