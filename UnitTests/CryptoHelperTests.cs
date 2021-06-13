using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RygDataModel;
using static RygDataModel.CryptoHelper;

namespace UnitTests
{ 
    [TestClass]
    public class CryptoHelperTests
    {
        //Decryption Tests
        [TestMethod]
        public void TestDecryptDatabaseValue()
        {
            // Configure
            string _encryptedData = @"zR2mQFB09xiNr_RhckgAsO59nQjKpQ4ial-SUHi-hr3IP_K-zwNOTsD9Pb0brtcVyNg7cGKSDLizIjJDXMiMcQ;lGcDQfn5EqQpI9N5ArPjlw"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"This is the Data to Encrypt on 2021-06-13 at 15:50:09...";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _decryptor.DecryptedValue, "Decrypted data does not match the expected result.");
        }
        [TestMethod]
        public void TestDecryptDefaultType()
        {
            // Configure
            string _encryptedData = @"zR2mQFB09xiNr_RhckgAsO59nQjKpQ4ial-SUHi-hr3IP_K-zwNOTsD9Pb0brtcVyNg7cGKSDLizIjJDXMiMcQ"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"lGcDQfn5EqQpI9N5ArPjlw";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"This is the Data to Encrypt on 2021-06-13 at 15:50:09...";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _decryptor.DecryptedValue, "Decrypted data does not match the expected result.");
        }
        [TestMethod]
        public void TestDecryptFailError()
        {
            // Configure
            string _encryptedData = @"zR2mQFB09xiNr_RhckgAsO59nQjKpQ4ial-SUHi-hr3IP_K-zwNOTsD9Pb0brtcVyNg7cGKSDLizIjJDXMiMcQ"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh_geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"VaBZY0MvHt0JOeA74UqHyA";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "Unable to Decrypt the data. Are you using the correct Keys, Salts and Initialization Vector");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }
        [TestMethod]
        public void TestDecryptWithDifferentVectorError()
        {
            // Configure
            string _encryptedData = @"ZQNya1STIIFAkAA4R6BF8tOX4nf-AR-v_G1DGeEQMWO6zBaFZB34rxJAt5g5uRE0n_8XFFDbv55efunfDlwF-Q;JUY57fZ7eh9A-r87R2BclA"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"Wm-FXk9YSqET8wbE0fLw5Q";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (ArgumentException ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "a different Initialization Vector was also found",
                                                           "An unexpected Error of type: ArgumentException was thrown, " +
                                                           "Message: " + ex.Message + ".");
                return;
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }
        [TestMethod]
        public void TestDecryptWithMissingKeysAndSaltsError()
        {
            // Configure
            string _encryptedData = @"ZQNya1STIIFAkAA4R6BF8tOX4nf-AR-v_G1DGeEQMWO6zBaFZB34rxJAt5g5uRE0n_8XFFDbv55efunfDlwF-Q"; //base64url is the default expected input
            string _dataSecretKeyValue = @"";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"";
            string _dataKeyPepperValue = @"";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "No Secret Key or Salt Values",
                                                           "An unexpected Error of type: ArgumentNullException was thrown, " +
                                                           "Message: " + ex.Message + ".");
                return;
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }
        [TestMethod]
        public void TestDecryptWithMissingVectorError()
        {
            // Configure
            string _encryptedData = @"ZQNya1STIIFAkAA4R6BF8tOX4nf-AR-v_G1DGeEQMWO6zBaFZB34rxJAt5g5uRE0n_8XFFDbv55efunfDlwF-Q"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            try
            {
                CryptoHelper _decryptor = new();
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "Cipher Initialization Vector Value not supplied",
                                                           "An unexpected Error of type: ArgumentNullException was thrown, " +
                                                           "Message: " + ex.Message + ".");
                return;
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        //Encrypt and Decrypt Tests 
        [TestMethod]
        public void TestEncryptDecryptDefaultType()
        {
            // Configure
            string _unEncryptedData = $"This is the Data to Encrypt on {DateTime.Now:yyyy-MM-dd} at {DateTime.Now:HH:mm:ss}...";
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            CryptoHelper _encryptor = new();
            try
            {
                _encryptor.EncryptData(_unEncryptedData,
                                       _dataSecretKeyValue,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                Assert.Fail("Encryption threw an unexpected exception: " + ex.Message);
                return;
            }

            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptor.EncryptedValue,
                                       _dataSecretKeyValue,
                                       _encryptor.EncryptedInitializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                Assert.Fail("Decryption threw an unexpected exception: " + ex.Message);
                return;
            }

            // Assert
            Assert.AreEqual<string>(_unEncryptedData, _decryptor.DecryptedValue, "Decrypted data does not match the data originaly encrypted.");
        }

        //Encryption Tests
        [TestMethod]
        public void TestEncryptWithMissingKeysAndSaltsError()
        {
            // Configure
            string _unEncryptedData = $"This is the Data to Encrypt on 2021-05-07 at 20:07:13...";
            string _dataSecretKeyValue = @"";
            string _dataKeySaltValue = @"";
            string _dataKeyPepperValue = @"";

            // Test
            try
            {
                CryptoHelper _encryptor = new();
                _encryptor.EncryptData(_unEncryptedData,
                                       _dataSecretKeyValue,
                                       _dataKeySaltValue,
                                       _dataKeyPepperValue);
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                StringAssert.Contains(ex.Message, "No Secret Key or Salt Values",
                                                           "An unexpected Error of type: ArgumentNullException was thrown, " +
                                                           "Message: " + ex.Message + ".");
                return;
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        //Hashing Tests
        [TestMethod]
        public void TestHash256()
        {
            // Configure
            string _unHashedData = @"This is the Data to Encrypt on 2021-06-13 at 16:09:53..."; //base64url is the default expected input
            string _dataSaltValue = @"sD°=+4~mAx*-12Eà";
            string _dataPepperValue = @"Sdf#4M.f%XµiSoBzGfen";
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"901BC88A9B47FF447E4C6A4519BCDCB7F7906B6904E63E1576FA82DD20739703";

            // Test
            string _actualResult = String.Empty;
            try
            {
                _actualResult = CreateHash256(_unHashedData,
                                              _dataSaltValue,
                                              _dataPepperValue,
                                              _dataSecretKeyValue,
                                              _dataKeySaltValue,
                                              _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Hashed data does not match the expected result.");
        }
        [TestMethod]
        public void TestHash512()
        {
            // Configure
            string _unHashedData = @"This is the Data to Encrypt on 2021-06-13 at 16:09:53..."; //base64url is the default expected input
            string _dataSaltValue = @"sD°=+4~mAx*-12Eà";
            string _dataPepperValue = @"Sdf#4M.f%XµiSoBzGfen";
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyPepperValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"473528746FFCED7B82D316F38AAFB8EA7B406B74E088273CBD1BA5D10BC6447FD2F8B1CA2A02A2ED3259DEB11D741633A9D14F7F3C881C7F8729F777D91A84C0";

            // Test
            string _actualResult = String.Empty;
            try
            {
                _actualResult = CreateHash512(_unHashedData,
                                              _dataSaltValue,
                                              _dataPepperValue,
                                              _dataSecretKeyValue,
                                              _dataKeySaltValue,
                                              _dataKeyPepperValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Hashed data does not match the expected result.");
        }
    }
}
