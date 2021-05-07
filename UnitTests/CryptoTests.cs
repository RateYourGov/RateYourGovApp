using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RygDataModel;

namespace UnitTests
{ 
    [TestClass]
    public class CryptoTests
    {

        [TestMethod]
        public void TestEncryptDecryptDefaultType()
        {
            // Configure
            string _unEncryptedData = $"This is the Data to Encrypt on {DateTime.Now:yyyy-MM-dd} at {DateTime.Now:HH:mm:ss}...";
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            CryptoHelper _encryptor = new();
            _encryptor.EncryptData(_unEncryptedData,
                                   _dataSecretKeyValue,
                                   _dataKeySaltValue,
                                   _dataKeyExtraSaltValue);

            CryptoHelper _decryptor = new();
            _decryptor.DecryptData(_encryptor.EncryptedValue,
                                   _dataSecretKeyValue,
                                   _encryptor.EncryptedInitializationVector,
                                   _dataKeySaltValue,
                                   _dataKeyExtraSaltValue);

            // Assert
            Assert.AreEqual<string>(_unEncryptedData, _decryptor.DecryptedValue, "Decrypted data does not match the data originaly encrypted.");
        }

        [TestMethod]
        public void TestDecryptDefaultTypeWithVector()
        {
            // Configure
            string _encryptedData = @"GjBNy3jPYzNpUPCaWTVZaF-kauoL2r4s2m23nB_7jr2SEeXRdsenTZQsIA95jEAiEX82IICL66z5S_gUUHEbXg"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"nOKM0-N8aQ1pXY8f-yi8OQ";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"This is the Data to Encrypt on 2021-05-07 at 20:24:59...";

            // Test
            CryptoHelper _decryptor = new();
            _decryptor.DecryptData(_encryptedData,
                                   _dataSecretKeyValue,
                                   _initializationVector,
                                   _dataKeySaltValue,
                                   _dataKeyExtraSaltValue);

            // Assert
            Assert.AreEqual<string>(_expectedResult, _decryptor.DecryptedValue, "Decrypted data does not match the expected result.");
        }

        [TestMethod]
        public void TestEncryptWithMissingKeysAndSalts()
        {
            // Configure
            string _unEncryptedData = $"This is the Data to Encrypt on 2021-05-07 at 20:07:13...";
            string _dataSecretKeyValue = @"";
            string _dataKeySaltValue = @"";
            string _dataKeyExtraSaltValue = @"";

            // Test
            try
            {
                CryptoHelper _encryptor = new();
                _encryptor.EncryptData(_unEncryptedData,
                                       _dataSecretKeyValue,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "No Secret Key or Salt Values");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestDecryptWithMissingKeysAndSalts()
        {
            // Configure
            string _encryptedData = @"GjBNy3jPYzNpUPCaWTVZaF-kauoL2r4s2m23nB_7jr2SEeXRdsenTZQsIA95jEAitMxr6ysXXNxJ_1GekoWkig"; //base64url is the default expected input
            string _dataSecretKeyValue = @"";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"";
            string _dataKeyExtraSaltValue = @"";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "No Secret Key or Salt Values");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestDecryptWithMissingVector()
        {
            // Configure
            string _encryptedData = @"GjBNy3jPYzNpUPCaWTVZaF-kauoL2r4s2m23nB_7jr2SEeXRdsenTZQsIA95jEAiEX82IICL66z5S_gUUHEbXg"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "Cipher Initialization Vector Value not supplied");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestDecryptWithDifferentVector()
        {
            // Configure
            string _encryptedData = @"GjBNy3jPYzNpUPCaWTVZaF-kauoL2r4s2m23nB_7jr2SEeXRdsenTZQsIA95jEAiEX82IICL66z5S_gUUHEbXg;nOKM0-N8aQ1pXY8f-yi8OQ"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"Wm-FXk9YSqET8wbE0fLw5Q";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "a different Initialization Vector was also found");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

    }
}
