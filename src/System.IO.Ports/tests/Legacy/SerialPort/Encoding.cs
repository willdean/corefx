// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO.Ports;
using System.IO.PortsTests;
using Legacy.Support;
using Xunit;

public class Encoding_Property : PortsTest
{
    //The default number of bytes to read/write to verify the speed of the port
    //and that the bytes were transfered successfully
    public static readonly int DEFAULT_CHAR_ARRAY_SIZE = 8;

    //The maximum time we will wait for all of encoded bytes to be received
    public static readonly int MAX_WAIT_TIME = 1250;

    private enum ThrowAt { Set, Open };

    #region Test Cases

    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_Default()
    {
        using (SerialPort com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            SerialPortProperties serPortProp = new SerialPortProperties();

            Debug.WriteLine("Verifying default Encoding");

            serPortProp.SetAllPropertiesToOpenDefaults();
            serPortProp.SetProperty("PortName", TCSupport.LocalMachineSerialInfo.FirstAvailablePortName);
            com1.Open();

            serPortProp.VerifyPropertiesAndPrint(com1);
            VerifyEncoding(com1);

            serPortProp.VerifyPropertiesAndPrint(com1);
        }
    }

    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_ASCIIEncoding_BeforeOpen()
    {
        Debug.WriteLine("Verifying ASCIIEncoding Encoding before open");
        VerifyEncodingBeforeOpen(new System.Text.ASCIIEncoding());
    }

    [ConditionalFact(nameof(HasNullModem), Skip= "UTF7 Not supported even on netfx")]
    public void Encoding_UTF7Encoding_BeforeOpen()
    {
        Debug.WriteLine("Verifying UTF7Encoding Encoding before open");
        VerifyEncodingBeforeOpen(new System.Text.UTF7Encoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UTF8Encoding_BeforeOpen()
    {
        Debug.WriteLine("Verifying UTF8Encoding Encoding before open");
        VerifyEncodingBeforeOpen(new System.Text.UTF8Encoding());
    }

    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UTF32Encoding_BeforeOpen()
    {
        Debug.WriteLine("Verifying UTF32Encoding Encoding before open");
        VerifyEncodingBeforeOpen(new System.Text.UTF32Encoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UnicodeEncoding_BeforeOpen()
    {
        Debug.WriteLine("Verifying UnicodeEncoding Encoding before open");
        VerifyEncodingBeforeOpen(new System.Text.UnicodeEncoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_ASCIIEncoding_AfterOpen()
    {
        Debug.WriteLine("Verifying ASCIIEncoding Encoding after open");
        VerifyEncodingAfterOpen(new System.Text.ASCIIEncoding());
    }
    
    [ConditionalFact(nameof(HasNullModem), Skip = "UTF7 Not supported even on netfx")]
    public void Encoding_UTF7Encoding_AfterOpen()
    {
        Debug.WriteLine("Verifying UTF7Encoding Encoding after open");
        VerifyEncodingAfterOpen(new System.Text.UTF7Encoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UTF8Encoding_AfterOpen()
    {
        Debug.WriteLine("Verifying UTF8Encoding Encoding after open");
        VerifyEncodingAfterOpen(new System.Text.UTF8Encoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UTF32Encoding_AfterOpen()
    {
        Debug.WriteLine("Verifying UTF32Encoding Encoding after open");
        VerifyEncodingAfterOpen(new System.Text.UTF32Encoding());
    }
    
    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_UnicodeEncoding_AfterOpen()
    {
        Debug.WriteLine("Verifying UnicodeEncoding Encoding after open");
        VerifyEncodingAfterOpen(new System.Text.UnicodeEncoding());
    }
    
    [ConditionalFact(nameof(HasOneSerialPort))]
    public void Encoding_ISCIIAssemese()
    {
        Debug.WriteLine("Verifying ISCIIAssemese Encoding");
        VerifyException(System.Text.Encoding.GetEncoding(57006), ThrowAt.Set, typeof(ArgumentException));
    }

    [ConditionalFact(nameof(HasOneSerialPort))]
    public void Encoding_UTF7()
    {
        Debug.WriteLine("Verifying UTF7Encoding Encoding");
        VerifyException(System.Text.Encoding.UTF7, ThrowAt.Set, typeof(ArgumentException));
    }

    [ConditionalFact(nameof(HasOneSerialPort))]
    public void Encoding_Null()
    {
        Debug.WriteLine("Verifying null Encoding");
        VerifyException(null, ThrowAt.Set, typeof(ArgumentNullException));
    }

    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_IBM_Latin1()
    {
        Debug.WriteLine("Verifying IBM Latin-1 Encoding before open");
        VerifyEncodingBeforeOpen(System.Text.Encoding.GetEncoding(1047));
    }

    [ConditionalFact(nameof(HasOneSerialPort))]
    public void Encoding_Japanese_JIS()
    {
        Debug.WriteLine("Verifying Japanese (JIS) Encoding before open");
        VerifyException(System.Text.Encoding.GetEncoding(50220), ThrowAt.Set, typeof(ArgumentException));
    }

    [ConditionalFact(nameof(HasNullModem))]
    public void Encoding_ChineseSimplified_GB18030()
    {
        Debug.WriteLine("Verifying Chinese Simplified (GB18030) Encoding before open");
        VerifyEncodingBeforeOpen(System.Text.Encoding.GetEncoding(54936));
    }

    [ConditionalFact(nameof(HasOneSerialPort))]
    public void Encoding_Custom()
    {
        Debug.WriteLine("Verifying Custom Encoding before open");
        VerifyException(new MyEncoding(1047), ThrowAt.Set, typeof(ArgumentException));
    }

    class MyEncoding : System.Text.Encoding
    {
        public MyEncoding(int codePage)
            : base(codePage)
        {
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw new NotSupportedException();
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new NotSupportedException();
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new NotSupportedException();
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new NotSupportedException();
        }

        public override int GetMaxByteCount(int charCount)
        {
            throw new NotSupportedException();
        }

        public override int GetMaxCharCount(int byteCount)
        {
            throw new NotSupportedException();
        }
    }

    #endregion

    #region Verification for Test Cases
    private void VerifyException(System.Text.Encoding encoding, ThrowAt throwAt, Type expectedException)
    {
        using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            VerifyExceptionAtOpen(com, encoding, throwAt, expectedException);

            if (com.IsOpen)
                com.Close();

            VerifyExceptionAfterOpen(com, encoding, expectedException);
        }
    }

    private void VerifyExceptionAtOpen(SerialPort com, System.Text.Encoding encoding, ThrowAt throwAt, Type expectedException)
    {
        System.Text.Encoding origEncoding = com.Encoding;
        SerialPortProperties serPortProp = new SerialPortProperties();

        serPortProp.SetAllPropertiesToDefaults();
        serPortProp.SetProperty("PortName", TCSupport.LocalMachineSerialInfo.FirstAvailablePortName);

        if (ThrowAt.Open == throwAt)
            serPortProp.SetProperty("Encoding", encoding);

        try
        {
            com.Encoding = encoding;

            if (ThrowAt.Open == throwAt)
                com.Open();

            Object myEncoding = com.Encoding;

            com.Encoding = origEncoding;

            if (null != expectedException)
            {
                Fail("ERROR!!! Expected Open() to throw {0} and nothing was thrown", expectedException);
            }
        }
        catch (Exception e)
        {
            if (null == expectedException)
            {
                Fail("ERROR!!! Expected Open() NOT to throw an exception and {0} was thrown", e.GetType());
            }
            else if (e.GetType() != expectedException)
            {
                Fail("ERROR!!! Expected Open() throw {0} and {1} was thrown", expectedException, e.GetType());
            }
        }

        serPortProp.VerifyPropertiesAndPrint(com);
        com.Encoding = origEncoding;
    }
    
    private void VerifyExceptionAfterOpen(SerialPort com, System.Text.Encoding encoding, Type expectedException)
    {
        SerialPortProperties serPortProp = new SerialPortProperties();

        com.Open();
        serPortProp.SetAllPropertiesToOpenDefaults();
        serPortProp.SetProperty("PortName", TCSupport.LocalMachineSerialInfo.FirstAvailablePortName);

        try
        {
            com.Encoding = encoding;

            if (null != expectedException)
            {
                Fail("ERROR!!! Expected setting the Encoding after Open() to throw {0} and nothing was thrown", expectedException);
            }
        }
        catch (Exception e)
        {
            if (null == expectedException)
            {
                Fail("ERROR!!! Expected setting the Encoding after Open() NOT to throw an exception and {0} was thrown", e.GetType());
            }
            else if (e.GetType() != expectedException)
            {
                Fail("ERROR!!! Expected setting the Encoding after Open() throw {0} and {1} was thrown", expectedException, e.GetType());
            }
        }

        serPortProp.VerifyPropertiesAndPrint(com);
    }


    private void VerifyEncodingBeforeOpen(System.Text.Encoding encoding)
    {
        using (SerialPort com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            SerialPortProperties serPortProp = new SerialPortProperties();

            serPortProp.SetAllPropertiesToOpenDefaults();
            serPortProp.SetProperty("PortName", TCSupport.LocalMachineSerialInfo.FirstAvailablePortName);

            com1.Encoding = encoding;
            com1.Open();
            serPortProp.SetProperty("Encoding", encoding);

            serPortProp.VerifyPropertiesAndPrint(com1);
            VerifyEncoding(com1);
            serPortProp.VerifyPropertiesAndPrint(com1);
        }
    }
    
    private void VerifyEncodingAfterOpen(System.Text.Encoding encoding)
    {
        using (SerialPort com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            SerialPortProperties serPortProp = new SerialPortProperties();

            serPortProp.SetAllPropertiesToOpenDefaults();
            serPortProp.SetProperty("PortName", TCSupport.LocalMachineSerialInfo.FirstAvailablePortName);

            com1.Open();
            com1.Encoding = encoding;
            serPortProp.SetProperty("Encoding", encoding);

            serPortProp.VerifyPropertiesAndPrint(com1);
            VerifyEncoding(com1);
            serPortProp.VerifyPropertiesAndPrint(com1);
        }
    }

    private void VerifyEncoding(SerialPort com1)
    {
        using (SerialPort com2 = new SerialPort(TCSupport.LocalMachineSerialInfo.SecondAvailablePortName))
        {
            int origReadTimeout = com1.ReadTimeout;
            char[] xmitChars = TCSupport.GetRandomChars(DEFAULT_CHAR_ARRAY_SIZE, true);
            byte[] xmitBytes;
            char[] rcvChars = new char[DEFAULT_CHAR_ARRAY_SIZE];
            char[] expectedChars;
            int waitTime = 0;

            xmitBytes = com1.Encoding.GetBytes(xmitChars);
            expectedChars = com1.Encoding.GetChars(xmitBytes);

            com2.Open();
            com2.Encoding = com1.Encoding;

            com2.Write(xmitChars, 0, xmitChars.Length);

            //		for(int i=0; i<xmitChars.Length; ++i) {
            //			Debug.WriteLine("{0},", (int)xmitChars[i]);
            //		}

            while (com1.BytesToRead < xmitBytes.Length)
            {
                System.Threading.Thread.Sleep(50);
                waitTime += 50;

                if (MAX_WAIT_TIME < waitTime)
                {
                    Debug.WriteLine("ERROR!!! Expected BytesToRead={0} actual={1}", xmitBytes.Length, com1.BytesToRead);
                    break;
                }
            }

            com1.Read(rcvChars, 0, rcvChars.Length);

            Assert.Equal(expectedChars.Length, rcvChars.Length);

            for (int i = 0; i < rcvChars.Length; i++)
            {
                Assert.Equal(expectedChars[i], rcvChars[i]);
            }
            com1.ReadTimeout = origReadTimeout;
        }
    }

    #endregion
}
