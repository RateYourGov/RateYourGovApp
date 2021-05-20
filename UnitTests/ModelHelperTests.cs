using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RygDataModel.ModelHelper;

namespace UnitTests
{
    [TestClass]
    public class ModelHelperTests
    {
        //CompareByteArray
        [TestMethod]
        public void TestCompareByteArrayValues()
        {
            // Configure
            byte[] _initialValue = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };
            byte[] _matchedValue = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };
            byte[] _unMatchedValue = new byte[] { 67, 111, 110, 118, 64, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 85, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };

            // Test
            try
            {
                // Assert
                Assert.AreEqual(true, CompareByteArrayValues(_initialValue, _matchedValue), "Matching Byte Array not Matched.");
                Assert.AreEqual(false, CompareByteArrayValues(_initialValue, _unMatchedValue), "Unmatched Byte Array not Identified.");
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }
        }

        //CompareByteArraySecurely
        [TestMethod]
        public void TestCompareByteArrayValuesSecurely()
        {
            // Configure
            byte[] _initialValue = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };
            byte[] _matchedValue = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };
            byte[] _unMatchedValue = new byte[] { 67, 111, 110, 118, 64, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 85, 114, 114, 97, 121, 58, 32, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 44, 32, 87, 101, 108, 99, 111, 109, 101, 32, 116, 111, 32, 50, 48, 50, 49, 33 };

            // Test
            try
            {
                // Assert
                Assert.AreEqual(true, CompareByteArrayValuesSecurely(_initialValue, _matchedValue), "Matching Byte Array not Matched.");
                Assert.AreEqual(false, CompareByteArrayValuesSecurely(_initialValue, _unMatchedValue), "Unmatched Byte Array not Identified.");
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }
        }
        [TestMethod]
        public void TestCompareByteArrayValuesSecurelyTiming()
        {
            //Test to ensure that comparison of two byte arrays is within a margin of 0.25% difference between comparing byte arrays
            //that match or that do not match.
            //This function is used to counter timing attacks, so it is important that testing byte arrays that match takes about the 
            //same amount of time as testing byte arrays that do not match.
            //Using a 64 byte length array as this is what length we would get from a tyical SHA512 hash encryption.
            //Adding several differences early on which should highlight any disparity quite easily.

            //Configure
            byte[] _testBytesOriginal = new byte[64];
            byte[] _testBytesMatched = new byte[64];
            byte[] _testBytesUnMatched = new byte[64];
            long _matchedTicks;
            long _unMatchedTicks;
            bool _actualResultMatchedWithinTolerance;
            const double _varianceTolerancePercent = 0.25D; //The tolerance/difference allowed between ticks taken for the test to be successful
            const int _runIterations = 150; //The number of iterations of the test to run to get a more average result
            double _actualVariancePercent;

            try
            {
                //Set up the process priority to try and ensure consistency of resources between the two tests
                System.IntPtr _saveIntPtr = Process.GetCurrentProcess().ProcessorAffinity;
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);        // Uses the second Core or Processor for the Test
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;      // Prevents "Normal" processes from interrupting Threads
                Thread.CurrentThread.Priority = ThreadPriority.Highest;                     // Prevents "Normal" Threads from interrupting this thread
                bool _unMatched;
                bool _matched;

                //populate the byte strings
                Random _rnd = new();
                for (int i = 0; i < 64; i++)
                {
                    int _newChr = _rnd.Next(32, 255);
                    _testBytesOriginal[i] = Convert.ToByte((char)_newChr);
                    _testBytesMatched[i] = Convert.ToByte((char)_newChr);
                    //sprinkle in a few differences in the unmatched array
                    if (i == 2 || i == 3 || i == 5 || i == 10 || i == 17 || i == 24 || i == 37 || i == 41 || i == 43 || i == 44 || i == 49 || i == 50 || i == 63)
                    {
                        _newChr = _rnd.Next(32, 255);
                        _testBytesUnMatched[i] = Convert.ToByte((char)_newChr);
                    }
                    else
                    {
                        _testBytesUnMatched[i] = Convert.ToByte((char)_newChr);
                    }
                }
                //warm up
                for (int i = 0; i < _runIterations; i++)
                {
                    _matched = CompareByteArrayValuesSecurely(_testBytesOriginal, _testBytesMatched);
                    _unMatched = CompareByteArrayValuesSecurely(_testBytesOriginal, _testBytesUnMatched);
                }

                //Run the test
                Stopwatch _unMatchedStopwatch = new();
                _unMatchedTicks = 0;
                long[] _unMatchedResults = new long[_runIterations];
                Stopwatch _matchedStopwatch = new();
                _matchedTicks = 0;
                long[] _matchedResults = new long[_runIterations];
                //run the test for the specified number of iterations
                for (int i = 0; i < _runIterations; i++)
                {
                    //measure matched time
                    _matchedStopwatch.Reset();
                    _matchedStopwatch.Start();
                    _matched = CompareByteArrayValuesSecurely(_testBytesOriginal, _testBytesMatched);
                    _matchedStopwatch.Stop();
                    _matchedResults[i] = _matchedStopwatch.ElapsedTicks;

                    //Measure unmatched time
                    _unMatchedStopwatch.Reset();
                    _unMatchedStopwatch.Start();
                    _unMatched = CompareByteArrayValuesSecurely(_testBytesOriginal, _testBytesUnMatched);
                    _unMatchedStopwatch.Stop();
                    _unMatchedResults[i] = _unMatchedStopwatch.ElapsedTicks;
                }

                //Use the modal (the value that occurred the most often) value as the result
                _matchedTicks = _matchedResults.GroupBy(v => v)
                                               .OrderByDescending(g => g.Count())
                                               .First()
                                               .Key;
                _unMatchedTicks = _unMatchedResults.GroupBy(v => v)
                                                   .OrderByDescending(g => g.Count())
                                                   .First()
                                                   .Key;

                //Reset the process priorities back to normal
                Process.GetCurrentProcess().ProcessorAffinity = _saveIntPtr;                  // Resets the Core Processor for the thread
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;      // Resets allowing "Normal" processes to interrupt Threads
                Thread.CurrentThread.Priority = ThreadPriority.Normal;                        // Resets to "Normal" 

                //Check if the results are within the tolerance allowed
                if (_matchedTicks == _unMatchedTicks)
                {
                    _actualResultMatchedWithinTolerance = true;
                    _actualVariancePercent = 0.00D;
                }
                else if (_matchedTicks > _unMatchedTicks)
                {
                    _actualVariancePercent = (((double)(_matchedTicks - _unMatchedTicks) / (double)_matchedTicks)) * 100D;
                    _actualResultMatchedWithinTolerance = _actualVariancePercent <= _varianceTolerancePercent;
                }
                else  //(_matchedTicks < _unMatchedTicks)
                {
                    _actualVariancePercent = (((double)(_unMatchedTicks - _matchedTicks) / (double)_unMatchedTicks)) * 100D;
                    _actualResultMatchedWithinTolerance = _actualVariancePercent <= _varianceTolerancePercent;
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ",  Message: " + ex.Message + ".");
                return;
            }

            Assert.AreEqual<bool>(true, _actualResultMatchedWithinTolerance,
                                  $"Matching of Matched and Unmatched Byte Arrays did not complete within the tolerance of {_varianceTolerancePercent}%. " +
                                  $"{_runIterations} iterations of Matched took mostly {_matchedTicks} ticks, whilst Unmatched took mostly {_unMatchedTicks} ticks, amounting to a variance of {_actualVariancePercent}%.");
        }

        //TrimOrPadText
        [TestMethod]
        public void TestTrimOrPadText()
        {
            // Test
            try
            {
                // Assert
                Assert.AreEqual<string>("12312345ôB§¤c45", TrimOrPadText("12345", 15, 15), "Pad In String failed.");
                Assert.AreEqual<string>("12345ôB§¤c12345", TrimOrPadText("12345", 15, 15, false, true), "Pad Left failed.");
                Assert.AreEqual<string>("1234512345ôB§¤c", TrimOrPadText("12345", 15, 15, false, false, true), "Pad Right failed.");
                Assert.AreEqual<string>("123123¼Hù%£éI^ê", TrimOrPadText("123", 15, 15), "Pad In String for a Short string failed.");
                Assert.AreEqual<string>("123¼Hù%£éI^ê123", TrimOrPadText("123", 15, 15, false, true), "Pad Left for a Short string failed.");
                Assert.AreEqual<string>("123123¼Hù%£éI^ê", TrimOrPadText("123", 15, 15, false, false, true), "Pad Right for a Short string failed.");
                Assert.AreEqual<string>("12890", TrimOrPadText("1234567890", 5, 5), "Trim In String failed.");
                Assert.AreEqual<string>("67890", TrimOrPadText("1234567890", 5, 5, false, true), "Trim Left failed.");
                Assert.AreEqual<string>("12345", TrimOrPadText("1234567890", 5, 5, false, false, true), "Trim Right failed.");
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }
        }
        [TestMethod]
        public void TestTrimOrPadTextMultipleTrimOrPadTypesError()
        {
            // Test
            try
            {
                TrimOrPadText("123", 15, 15, true, false, true);
            }
            catch (ArgumentException ex)
            {
                // Assert 
                StringAssert.Contains(ex.Message, "You may only specify true for one of the Arguments: trimOrPadInString, trimOrPadLeft and trimOrPadRight.",
                                                           "Unexpected Error of type: ArgumentException was thrown, " +
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

        //Convert Byte Array From/To Hex string
        [TestMethod]
        public void TestConvertByteArrayToHexString()
        {
            // Configure
            byte[] _initialBytes = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            string _expectedResult = @"43-6F-6E-76-65-72-74-20-54-6F-20-42-79-74-65-20-41-72-72-61-79-3A-20-48-65-6C";
            string _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayToHexString(_initialBytes);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexString()
        {
            // Configure
            string _initialString = @"43-6F-6E-76-65-72-74-20-54-6F-20-42-79-74-65-20-41-72-72-61-79-3A-20-48-65-6C";
            byte[] _expectedResult = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromHexString(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromAndToHexString()
        {
            // Configure
            string _expectedResult = @"43-6F-6E-76-65-72-74-20-54-6F-20-42-79-74-65-20-41-72-72-61-79-3A-20-48-65-6C";
            string _actualResult;

            // Test
            try
            {
                byte[] _intertimResult = ConvertByteArrayFromHexString(_expectedResult);
                _actualResult = ConvertByteArrayToHexString(_intertimResult);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringSingleByte()
        {
            // Configure
            string _initialString = @"43";
            byte[] _expectedResult = new byte[] { 67 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromHexString(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringTooShortError()
        {
            //Single character string
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"3");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "must be 2 characters in length",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringElementTooShortError()
        {
            //One of the elements has only one character
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"36-37-2C-31-31-31-2C-31-31-30-2-31-31-38-2C-31-33");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "must be 2 characters in length",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
                                           "Message: " + ex.Message + ".");
                return;
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Larger than 2 Character Element Test Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert FAIL
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringElementTooLongError()
        {
            //One of the elements has three characters for a hex value
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"36-37-2C-31-31-31-2C-31-31-30-2CA-31-31-38-2C-31-33");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "must be 2 characters in length",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringEmptyElementError()
        {
            //One of the elements has no characters for a hex value
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"36-37-2C-31-31-31-2C-31--30-30-31-31-38-2C-31-33");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "There is an empty element at position",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringEmptyFirstElementError()
        {
            //The first element has no characters for a hex value
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"-37-2C-31-31-31-2C-31-30-30-31-31-38-2C-31-33");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "first element is empty",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringEmptyLastElementError()
        {
            //The last element has no characters for a hex value
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"36-37-2C-31-31-31-2C-31-30-30-31-31-38-2C-31-");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "last element is empty",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringInvalidCharactersError()
        {
            //One of the elements has no characters for a hex value
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexString(@"36-37-2C-31-31-31-2C-31-P2-30-30-31-31-38-2C-31-33");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Invalid Hex string element value",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
            Assert.Fail("The expected exceptions were not thrown.");
        }

        //Convert Byte Array From/To Short Hex string
        [TestMethod]
        public void TestConvertByteArrayToHexStringShort()
        {
            // Configure
            byte[] _initialBytes = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            string _expectedResult = @"436F6E7665727420546F20427974652041727261793A2048656C";
            string _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayToHexStringShort(_initialBytes);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringShort()
        {
            // Configure
            string _initialString = @"436F6E7665727420546F20427974652041727261793A2048656C";
            byte[] _expectedResult = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromHexStringShort(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromAndToHexStringShort()
        {
            // Configure
            string _expectedResult = @"436F6E7665727420546F20427974652041727261793A2048656C";
            string _actualResult;

            // Test
            try
            {
                byte[] _interimResult = ConvertByteArrayFromHexStringShort(_expectedResult);
                _actualResult = ConvertByteArrayToHexStringShort(_interimResult);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromHexStringShortUnevenCharactersError()
        {
            // Test
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexStringShort(@"436F6E7665727420546F20427974652041727261793A2048656");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Hex string values must be multiples of 2 characters in length",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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
        public void TestConvertByteArrayFromHexStringShortInvalidCharactersError()
        {
            // Test
            try
            {
                byte[] _actualResult = ConvertByteArrayFromHexStringShort(@"436F6E7665727420546Y20427974652041727261793A2048656C");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Invalid Hex string value",
                                           "Unexpected Error of type: ArgumentException was thrown, " +
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

        //Convert Byte Array From/To ASCII
        [TestMethod]
        public void TestConvertByteArrayToASCII()
        {
            // Configure
            byte[] _initialBytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            string _expectedResult = @"Hello World!";
            string _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayToASCII(_initialBytes);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromASCII()
        {
            // Configure
            string _initialString = @"Hello World!";
            byte[] _expectedResult = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromASCII(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromAndToASCII()
        {
            // Configure
            byte[] _expectedResult = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            byte[] _actualResult;

            // Test
            try
            {
                string _interimResult = ConvertByteArrayToASCII(_expectedResult);
                _actualResult = ConvertByteArrayFromASCII(_interimResult);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }

        //Convert Byte Array From/To UTF8
        [TestMethod]
        public void TestConvertByteArrayToUTF8()
        {
            // Configure
            byte[] _initialBytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            string _expectedResult = @"Hello World!";
            string _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayToUTF8(_initialBytes);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromUTF8()
        {
            // Configure
            string _initialString = @"Hello World!";
            byte[] _expectedResult = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromUTF8(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromAndToUTF8()
        {
            // Configure
            byte[] _expectedResult = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            byte[] _actualResult;

            // Test
            try
            {
                string _interimResult = ConvertByteArrayToUTF8(_expectedResult);
                _actualResult = ConvertByteArrayFromUTF8(_interimResult);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }

        //Convert Byte Array From/To Unicode (UTF16)
        [TestMethod]
        public void TestConvertByteArrayToUnicode()
        {
            // Configure
            byte[] _initialBytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            string _expectedResult = @"效汬⁯潗汲Ⅴ";
            string _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayToUnicode(_initialBytes);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<string>(_expectedResult, _actualResult, "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromUnicode()
        {
            // Configure
            string _initialString = @"效汬⁯潗汲Ⅴ";
            byte[] _expectedResult = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            byte[] _actualResult;

            // Test
            try
            {
                _actualResult = ConvertByteArrayFromUnicode(_initialString);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
        [TestMethod]
        public void TestConvertByteArrayFromAndToUnicode()
        {
            // Configure
            byte[] _expectedResult = new byte[] { 67, 111, 110, 118, 101, 114, 116, 32, 84, 111, 32, 66, 121, 116, 101, 32, 65, 114, 114, 97, 121, 58, 32, 72, 101, 108 };
            byte[] _actualResult;

            // Test
            try
            {
                string _interimResult = ConvertByteArrayToUnicode(_expectedResult);
                _actualResult = ConvertByteArrayFromUnicode(_interimResult);
            }
            catch (Exception ex)
            {
                // Assert FAIL
                Assert.Fail("Unexpected Exception of Type: " + ex.GetType().ToString() + ", Message: " + ex.Message + ".");
                return;
            }

            // Assert
            Assert.AreEqual<bool>(true, CompareByteArrayValues(_expectedResult, _actualResult), "Converted data does not match the expected result.");
        }
    }
}
