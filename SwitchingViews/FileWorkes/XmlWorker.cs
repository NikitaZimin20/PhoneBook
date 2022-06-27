using SwitchingViews.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace PhoneBook.FileWorkes
{
    internal static class XmlWorker
    {
        private static XmlDocument _xdoc;
         static XmlWorker()
        {
            _xdoc=new XmlDocument();
            _xdoc.Load(ConfigurationManager.ConnectionStrings["XmlPath"].ConnectionString);
        }
        public static ObservableCollection<UserModel> LoadFromXml(ObservableCollection<UserModel> user)
        {
            
            XmlElement? xRoot = _xdoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                user.Add(new UserModel { ID = xnode.Attributes.GetNamedItem("ID").Value, Name = xnode.ChildNodes.Item(0).InnerText.Trim(), Surname = xnode.ChildNodes.Item(1).InnerText.Trim(), Phone = xnode.ChildNodes.Item(2).InnerText.Trim() });
            }
            return user;

        }
        public static void DeleteFromXml(string id)
        {
            
            XmlElement? xRoot = _xdoc.DocumentElement;
            foreach (XmlElement item in xRoot)
            {
                if (item.Attributes.GetNamedItem("ID").Value == id)
                {
                    xRoot.RemoveChild(item);
                }
            }
            StartIdNumeration();
            _xdoc.Save(ConfigurationManager.ConnectionStrings["JsonPath"].ConnectionString);

        }


        public static void AddToXML(UserModel model)
        {
            XmlNode nl = _xdoc.SelectSingleNode("users");
            XmlDocument xd2 = new XmlDocument();
            xd2.LoadXml("<user ID='" + "1" + "'><name>" + model.Name + "</name><surname>" + model.Surname + "</surname><phone>" + model.Phone + "</phone></user>");
            XmlNode n = _xdoc.ImportNode(xd2.FirstChild, true);
            nl.AppendChild(n);
            _xdoc.Save(ConfigurationManager.ConnectionStrings["XmlPath"].ConnectionString);
            StartIdNumeration();

        }
        public static void ChangeXML(UserModel user)
        {
            
            XmlNodeList aNodes = _xdoc.SelectNodes("users/user");


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
            _xdoc.Save(ConfigurationManager.ConnectionStrings["XmlPath"].ConnectionString);
        }
        private static void StartIdNumeration()
        {
            XmlNodeList aNodes = _xdoc.SelectNodes("users/user");
            int count = aNodes.Count;

            for (int i = 0; i < count; i++)
            {
                aNodes[i].Attributes["ID"].Value = (i + 1).ToString();
            }
        }

       
    }
}