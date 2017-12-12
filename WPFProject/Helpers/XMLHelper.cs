using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class XMLHelper
    {
        public static void WriteXML( List<List<string>> InputData )
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<List<string>>));

            System.IO.StreamWriter file = new System.IO.StreamWriter(ProductProperties.LastSessionFileName);
            writer.Serialize(file, InputData);
            file.Close();
        }

        public static List<List<string>> ReadXML(string InputFile)
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<List<string>>));
            System.IO.StreamReader file = new System.IO.StreamReader(
                InputFile);
            var result = new List<List<string>>();
            result = (List<List<string>>)reader.Deserialize(file);
            file.Close();
            return result;
        }

        public static string ReadXMLString(string InputFile) {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(string));
            System.IO.StreamReader file = new System.IO.StreamReader(
                InputFile);
            var result = "";
            result = (string)reader.Deserialize(file);
            file.Close();
            return result;
        }

        public static void WriteXMLString(string InputData)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(string));

            System.IO.StreamWriter file = new System.IO.StreamWriter(ProductProperties.LastLoggedUserFile);
            writer.Serialize(file, InputData);
            file.Close();
        }

    }
}
