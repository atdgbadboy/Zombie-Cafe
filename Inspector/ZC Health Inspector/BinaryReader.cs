using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Health_Inspector
{
    class BinaryReader
    {
        byte[] file;
        public int index;

        public BinaryReader(byte[] File)
        {
            file = File;
            index = 0;
        }

        public short readShort(bool flip)
        {
            byte[] shortbytes = new byte[2];
            Buffer.BlockCopy(file, index, shortbytes, 0, 2);

            if (flip)
                shortbytes = shortbytes.Reverse().ToArray();

            index += 2;
            return BitConverter.ToInt16(shortbytes, 0);
        }

        public int readInt(bool flip)
        {
            byte[] intbytes = new byte[4];
            Buffer.BlockCopy(file, index, intbytes, 0, 4);
            int num = BitConverter.ToInt32(intbytes, 0);
            if (flip)
            {
                num = (intbytes[0] << 24) | (intbytes[1] << 16) | (intbytes[2] << 8) | intbytes[3];
            }

            index += 4;
            return num;
        }

        public T ReadStruct<T>() where T : struct
        {
            object data = new T();
            foreach (var field in typeof(T).GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
            {
                switch (Type.GetTypeCode(field.FieldType))
                {
                    case TypeCode.Int16:
                        field.SetValue(data, readShort(true));
                        break;
                    case TypeCode.String:
                        field.SetValue(data, ReadString());
                        break;
                    case TypeCode.Byte:
                        field.SetValue(data, readByte());
                        break;
                    case TypeCode.Boolean:
                        field.SetValue(data, ReadBool());
                        break;
                    case TypeCode.Int32:
                        field.SetValue(data, readInt(true));
                        break;
                    default:
                        throw new NotImplementedException("Struct Member type not yet implemented: " + field.FieldType.ToString());
                }
            }
            return (T)data;
        }

        public List<T> ReadStructArray<T>() where T : struct
        {
            List<T> items = new List<T> { };
            while (true)
            {
                try
                {
                    items.Add(ReadStruct<T>());
                }
                catch
                {
                    break;
                }

            }
            return items;
        }



        public string ReadString()
        {
            byte length = file[index];
            index++;
            byte[] buffer = new byte[length];
            Buffer.BlockCopy(file, index, buffer, 0, length);
            index += length;
            string s = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(s + " " + buffer.Length);
            return s;
        }

        public bool ReadBool()
        {
            byte b = file[index];
            index++;
            return b == 0x1;
        }

        public byte readByte()
        {
            byte b = file[index];
            index++;
            return b;
        }
    }
}
