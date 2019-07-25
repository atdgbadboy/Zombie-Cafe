using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Health_Inspector
{
    class BinaryWriter
    {
        List<byte> file;

        public BinaryWriter()
        {
            file = new List<byte> { };
        }

        public bool AppendToFile(byte[] bytes)
        {
            foreach(byte b in bytes)
            {
                file.Add(b);
            }
            return true;
        }

        public bool writeShort(short value, bool reverse)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (reverse)
                bytes = bytes.Reverse().ToArray();

            return AppendToFile(bytes);
        }

        public bool writeString(string value)
        {
            byte[] textData = Encoding.UTF8.GetBytes(value);
            AppendToFile(new byte[] { (byte)textData.Length });
            AppendToFile(textData);
            return true;
        }

        public bool writeByte(byte value)
        {
            return AppendToFile(new byte[] { value });
        }

        public bool removeByte()
        {
            file.RemoveAt(file.Count - 1);
            return true;
        }

        public bool writeStruct(object _value, Type type)
        {
            dynamic value = Convert.ChangeType(_value, type);
            foreach (var field in type.GetFields(BindingFlags.Instance |
                                     BindingFlags.NonPublic |
                                     BindingFlags.Public))
            {
                switch (Type.GetTypeCode(field.FieldType))
                {
                    case TypeCode.Int16:
                        writeShort((short)field.GetValue(value), true);
                        break;
                    case TypeCode.String:
                        writeString((string)field.GetValue(value));
                        break;
                    case TypeCode.Byte:
                        writeByte((byte)field.GetValue(value));
                        break;
                    default:
                        throw new NotImplementedException("Struct Member type not yet implemented: " + field.FieldType.ToString());
                }
            }
            return true;
        }

        public bool writeStructArray(object[] value, Type type)
        {
            foreach(object item in value)
            {
                writeStruct(item, type);
            }
            return true;
        }


        public bool writeToFile(string path)
        {
            byte[] bytes = file.ToArray();
            File.WriteAllBytes(path, bytes);
            return true;
        }

    }
}
