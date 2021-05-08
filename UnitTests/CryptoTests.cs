using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RygDataModel;

namespace UnitTests
{ 
    [TestClass]
    public class CryptoTests
    {

        //Decryption Tests
        [TestMethod]
        public void TestDecryptDatabaseValue()
        {
            // Configure
            string _encryptedData = @"YCIB5g4KRcHNEtxPEbR487nqFgS43_4Ko60FyqC5Qc0vkxsb6tcn71E4velwc1d1fm5WlSU36NjjgOMr4Xb48w;VpBZYZMvHtiJOeA74UqHyA"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"This is the Data to Encrypt on 2021-05-08 at 13:40:50...";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("DecryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _decryptor.DecryptedValue, "Decrypted data does not match the expected result.");
        }

        [TestMethod]
        public void TestDecryptDefaultType()
        {
            // Configure
            string _encryptedData = @"yhLc1Bo9pQ-9f1VxXHpoDwXheV6IW3XPsJOF7QGGulnuGhLxj1OF6_Rsql8z-QetatKFY8-mpmJiSGxqf5T9Cw"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"jb2zeFCeJX1c6Iifachckw";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";
            string _expectedResult = @"This is the Data to Encrypt on 2021-05-08 at 14:00:29...";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("DecryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _decryptor.DecryptedValue, "Decrypted data does not match the expected result.");
        }

        [TestMethod]
        public void TestDecryptFail()
        {
            // Configure
            string _encryptedData = @"GjBNy3jPYzNpUPCaWTVZaF-kauoL2r4s2m23nB_7jr2SEeXRdsenTZQsIA95jEAiEX82IICL66z5S_gUUHEbXg"; //base64url is the default expected input
            string _dataSecretKeyValue = @"cAh*geMP#EzU8*nT_N&kcWn'6bQNàP4Wc¤uZS_§qazD^p-d9*zIGT#A0Sd'aoMP£";
            string _initializationVector = @"VpBZYZMvHtiJOeA74UqHyA";
            string _dataKeySaltValue = @"8af2996f-d627-40ac-8f84-c5532ace5a9e";
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptedData,
                                       _dataSecretKeyValue,
                                       _initializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
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
        public void TestDecryptWithDifferentVector()
        {
            // Configure
            string _encryptedData = @"YCIB5g4KRcHNEtxPEbR487nqFgS43_4Ko60FyqC5Qc0vkxsb6tcn71E4velwc1d1fm5WlSU36NjjgOMr4Xb48w;VpBZYZMvHtiJOeA74UqHyA"; //base64url is the default expected input
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
                Assert.Fail("DecryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestDecryptWithMissingKeysAndSalts()
        {
            // Configure
            string _encryptedData = @"YCIB5g4KRcHNEtxPEbR487nqFgS43_4Ko60FyqC5Qc0vkxsb6tcn71E4velwc1d1fm5WlSU36NjjgOMr4Xb48w"; //base64url is the default expected input
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
                Assert.Fail("DecryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void TestDecryptWithMissingVector()
        {
            // Configure
            string _encryptedData = @"YCIB5g4KRcHNEtxPEbR487nqFgS43_4Ko60FyqC5Qc0vkxsb6tcn71E4velwc1d1fm5WlSU36NjjgOMr4Xb48w"; //base64url is the default expected input
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
                Assert.Fail("DecryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
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
            string _dataKeyExtraSaltValue = @"MoEeUjtSp6c6OEELlMO1WA";

            // Test
            CryptoHelper _encryptor = new();
            try
            {
                _encryptor.EncryptData(_unEncryptedData,
                                       _dataSecretKeyValue,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                Assert.Fail("Encryption threw an exception: " + ex.Message);
                return;
            }

            CryptoHelper _decryptor = new();
            try
            {
                _decryptor.DecryptData(_encryptor.EncryptedValue,
                                       _dataSecretKeyValue,
                                       _encryptor.EncryptedInitializationVector,
                                       _dataKeySaltValue,
                                       _dataKeyExtraSaltValue);
            }
            catch (Exception ex)
            {
                Assert.Fail("Decryption threw an exception: " + ex.Message);
                return;
            }

            // Assert
            Assert.AreEqual<string>(_unEncryptedData, _decryptor.DecryptedValue, "Decrypted data does not match the data originaly encrypted.");
        }

        //Encryption Tests
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
                Assert.Fail("EncryptData threw an unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
            }

            // Assert FAIL
            Assert.Fail("The expected exception was not thrown.");
        }

    }
}
