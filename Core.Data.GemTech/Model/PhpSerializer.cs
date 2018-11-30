﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.GemTech.Model
{
    public class PhpSerializer
    {
        //types:
        // N = null
        // s = string
        // i = int
        // d = double
        // a = array (hashtable)

        private Dictionary<Hashtable, bool> seenHashtables; 
        private Dictionary<ArrayList, bool> seenArrayLists; 

        private int pos; 

        public bool XMLSafe = true; //This member tells the serializer wether or not to strip carriage returns from strings when serializing and adding them back in when deserializing
                                    //http://www.w3.org/TR/REC-xml/#sec-line-ends

        public Encoding StringEncoding = new System.Text.UTF8Encoding();

        private System.Globalization.NumberFormatInfo nfi;

        public PhpSerializer()
        {
            this.nfi = new System.Globalization.NumberFormatInfo();
            this.nfi.NumberGroupSeparator = "";
            this.nfi.NumberDecimalSeparator = ".";
        }

        public string Serialize(object obj)
        {
            this.seenArrayLists = new Dictionary<ArrayList, bool>();
            this.seenHashtables = new Dictionary<Hashtable, bool>();

            return this.serialize(obj, new StringBuilder()).ToString();
        }

        private StringBuilder serialize(object obj, StringBuilder sb)
        {
            if (obj == null)
            {
                return sb.Append("N;");
            }
            else if (obj is string)
            {
                string str = (string)obj;
                if (this.XMLSafe)
                {
                    str = str.Replace("\r\n", "\n");
                    str = str.Replace("\r", "\n");
                }
                return sb.Append("s:" + this.StringEncoding.GetByteCount(str) + ":\"" + str + "\";");
            }
            else if (obj is bool)
            {
                return sb.Append("b:" + (((bool)obj) ? "1" : "0") + ";");
            }
            else if (obj is int)
            {
                int i = (int)obj;
                return sb.Append("i:" + i.ToString(this.nfi) + ";");
            }
            else if (obj is double)
            {
                double d = (double)obj;

                return sb.Append("d:" + d.ToString(this.nfi) + ";");
            }
            else if (obj is ArrayList)
            {
                if (this.seenArrayLists.ContainsKey((ArrayList)obj))
                    return sb.Append("N;");//cycle detected
                else
                    this.seenArrayLists.Add((ArrayList)obj, true);

                ArrayList a = (ArrayList)obj;
                sb.Append("a:" + a.Count + ":{");
                for (int i = 0; i < a.Count; i++)
                {
                    this.serialize(i, sb);
                    this.serialize(a[i], sb);
                }
                sb.Append("}");
                return sb;
            }
            else if (obj is Hashtable)
            {
                if (this.seenHashtables.ContainsKey((Hashtable)obj))
                    return sb.Append("N;");//cycle detected
                else
                    this.seenHashtables.Add((Hashtable)obj, true);

                Hashtable a = (Hashtable)obj;
                sb.Append("a:" + a.Count + ":{");
                foreach (DictionaryEntry entry in a)
                {
                    this.serialize(entry.Key, sb);
                    this.serialize(entry.Value, sb);
                }
                sb.Append("}");
                return sb;
            }
            else
            {
                return sb;
            }
        }//Serialize(object obj)

        public object Deserialize(string str)
        {
            this.pos = 0;
            return deserialize(str);
        }//Deserialize(string str)

        private object deserialize(string str)
        {
            if (str == null || str.Length <= this.pos)
                return new Object();

            int start, end, length;
            string stLen;
            switch (str[this.pos])
            {
                case 'N':
                    this.pos += 2;
                    return null;
                case 'b':
                    char chBool;
                    chBool = str[pos + 2];
                    this.pos += 4;
                    return chBool == '1';
                case 'i':
                    string stInt;
                    start = str.IndexOf(":", this.pos) + 1;
                    end = str.IndexOf(";", start);
                    stInt = str.Substring(start, end - start);
                    this.pos += 3 + stInt.Length;
                    return Int32.Parse(stInt, this.nfi);
                case 'd':
                    string stDouble;
                    start = str.IndexOf(":", this.pos) + 1;
                    end = str.IndexOf(";", start);
                    stDouble = str.Substring(start, end - start);
                    this.pos += 3 + stDouble.Length;
                    return Double.Parse(stDouble, this.nfi);
                case 's':
                    start = str.IndexOf(":", this.pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    int bytelen = Int32.Parse(stLen);
                    length = bytelen;
                    if ((end + 2 + length) >= str.Length) length = str.Length - 2 - end;
                    string stRet = str.Substring(end + 2, length);
                    while (this.StringEncoding.GetByteCount(stRet) > bytelen)
                    {
                        length--;
                        stRet = str.Substring(end + 2, length);
                    }
                    this.pos += 6 + stLen.Length + length;
                    if (this.XMLSafe)
                    {
                        stRet = stRet.Replace("\n", "\r\n");
                    }
                    return stRet;
                case 'a':
                    start = str.IndexOf(":", this.pos) + 1;
                    end = str.IndexOf(":", start);
                    stLen = str.Substring(start, end - start);
                    length = Int32.Parse(stLen);
                    Hashtable htRet = new Hashtable(length);
                    ArrayList alRet = new ArrayList(length);
                    this.pos += 4 + stLen.Length; 
                    for (int i = 0; i < length; i++)
                    {
                        object key = deserialize(str);
                        object val = deserialize(str);

                        if (alRet != null)
                        {
                            if (key is int && (int)key == alRet.Count)
                                alRet.Add(val);
                            else
                                alRet = null;
                        }
                        htRet[key] = val;
                    }
                    this.pos++;
                    if (this.pos < str.Length && str[this.pos] == ';')
                        this.pos++;
                    if (alRet != null)
                        return alRet;
                    else
                        return htRet;
                default:
                    return "";
            }
        }
    }
}
