using SwitchingViews.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace PhoneBook.FileWorkes
{
    public class XmlWorker
    {
        public static ObservableCollection<UserModel> LoadFromXml(ObservableCollection<UserModel> user)
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Configuration.FilePath);
            XmlElement? xRoot = xDoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                user.Add(new UserModel { ID = xnode.Attributes.GetNamedItem("ID").Value, Name = xnode.ChildNodes.Item(0).InnerText.Trim(), Surname = xnode.ChildNodes.Item(1).InnerText.Trim(), Phone = xnode.ChildNodes.Item(2).InnerText.Trim() });
            }
            return user;

        }
        public static void DeleteFromXml(string id)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("XMLFile1.xml");
            XmlElement? xRoot = xDoc.DocumentElement;
            foreach (XmlElement item in xRoot)
            {
                if (item.Attributes.GetNamedItem("ID").Value == id)
                {
                    xRoot.RemoveChild(item);
                }
            }
            ChangeElement(xDoc);
            xDoc.Save(Configuration.FilePath);

        }


        public static void AddToXML(UserModel model)
        {

            var xd = new XmlDocument();
            xd.Load(Configuration.FilePath);
            XmlNode nl = xd.SelectSingleNode("users");
            XmlDocument xd2 = new XmlDocument();
            xd2.LoadXml("<user ID='" + GetLastID() + "'><name>" + model.Name + "</name><surname>" + model.Surname + "</surname><phone>" + model.Phone + "</phone></user>");
            XmlNode n = xd.ImportNode(xd2.FirstChild, true);
            nl.AppendChild(n);
            xd.Save(Configuration.FilePath);

        }
        public static void ChangeXML(UserModel user)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Configuration.FilePath);


            XmlNodeList aNodes = doc.SelectNodes("users/user");


            foreach (XmlNode aNode in aNodes)
            {
                XmlAttribute idAttribute = aNode.Attributes["ID"];
                if (idAttribute != null)
                {
                    string currentValue = idAttribute.Value;


                    if (currentValue == user.ID)
                    {
                        aNode.ChildNodes.Item(0).InnerText = user.Name;
                        aNode.ChildNodes.Item(1).InnerText = user.Surname;
                        aNode.ChildNodes.Item(2).InnerText = user.Phone;
                    }
                }
            }
            doc.Save(Configuration.FilePath);
        }
        private static void ChangeElement(XmlDocument doc)
        {
            XmlNodeList aNodes = doc.SelectNodes("users/user");
            int count = aNodes.Count;

            for (int i = 0; i < count; i++)
            {
                aNodes[i].Attributes["ID"].Value = (i + 1).ToString();
            }
        }

        private static string GetLastID()
        {
            var xdoc = XDocument.Load(Configuration.FilePath);
            XElement lastelement = xdoc.Root.Elements("user").LastOrDefault();
            if (lastelement is null)
            {
                return "0";
            }
            var value = int.Parse(lastelement.Attribute("ID").Value) + 1;
            return value.ToString();

        }
    }
}