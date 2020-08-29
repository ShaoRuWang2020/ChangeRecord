using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Cognex.VS.Utility;
namespace XMLOpera
{
    class XmlHelper
    {
        private String XML_Path;

        /// <summary>
        /// </summary>
        /// <param name="XmlPath"></param>

        public XmlHelper(string xmlPath)//构造函数 负责初始化 XMLDocument 对象
        {
            XML_Path = xmlPath;

        }
        /// <summary>
        /// 读取XML配置文件的参数设置，获取下载的TXT文件路径与上传的数据文件路径
        /// </summary>
        /// <param name="Label_Fir">First Group</param>
        /// <param name="Label_Sec">Second Group</param>
        /// <param name="Label_Thir">Third Group</param>
        /// <returns>Value</returns>
        public string ReadXML(string Label_Fir, string Label_Sec, string Label_Thir)
        {
            string Result = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //Load XML File
                xmlDoc.Load(XML_Path);

                XmlNode rootNode = xmlDoc.SelectSingleNode(Label_Fir).SelectSingleNode(Label_Sec);
                if (rootNode == null)
                {
                    throw new ApplicationException("Exception Occured,XML配置文件信息异常!");
                }

                XmlElement XML_Value = (XmlElement)rootNode.SelectSingleNode(Label_Thir);
                if (XML_Value == null)
                {
                    throw new Exception("Exception Occured,XML配置文件信息异常!");
                }
                Result = XML_Value.InnerText;
            }
            catch
            {
              
            }
            return Result;
        }

        /// <summary>
        /// 读取XML配置文件的参数设置，获取下载的TXT文件路径与上传的数据文件路径
        /// </summary>
        /// <param name="Label_Fir">First Group</param>
        /// <param name="Label_Sec">Second Group</param>
        /// <param name="Label_Thir">Third Group</param>
        /// <param name="Label_Value">Value</param>
        public void WriteXML(string Label_Fir, string Label_Sec, string Label_Thir, string Label_Value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XML_Path);

                XmlNode rootNode = xmlDoc.SelectSingleNode(Label_Fir).SelectSingleNode(Label_Sec);
                if (rootNode == null)//如果没有第二层节点
                {

                    XmlNode root = xmlDoc.SelectSingleNode(Label_Fir);//查找根节点   
                    XmlElement xe1 = xmlDoc.CreateElement(Label_Sec);//创建一个sub节点   

                    XmlElement xesub1 = xmlDoc.CreateElement(Label_Thir);
                    xesub1.InnerText = Label_Value;//设置文本节点   
                    xe1.AppendChild(xesub1);
                    root.AppendChild(xe1);//添加到根节点中   

                }
                else//更新第三层节点
                {
                    XmlElement XML_Value = (XmlElement)rootNode.SelectSingleNode(Label_Thir);


                    if (XML_Value == null)//如果没有第三层节点
                    {
                        XmlNode root = xmlDoc.SelectSingleNode(Label_Fir);//查找根节点   
                        XmlNode rootNodeInsert = xmlDoc.SelectSingleNode(Label_Fir).SelectSingleNode(Label_Sec);
                        XmlElement xe1 = (XmlElement)rootNodeInsert;
                        XmlElement xesub1 = xmlDoc.CreateElement(Label_Thir);
                        xesub1.InnerText = Label_Value;//设置文本节点   
                        xe1.AppendChild(xesub1);
                        root.AppendChild(xe1);//添加到根节点中   

                    }
                    else//更新第三层节点的值
                    {
                        XML_Value.InnerText = Label_Value;
                    }
                }
                xmlDoc.Save(XML_Path);
            }
            catch
            {
               
            };
        }
        public void DelXml(string NodeName_Inspection)//"T32_OutPutString".... delete 节点
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(XML_Path);

            XmlNodeList xnl = xmlDoc.SelectSingleNode("VisionParam").ChildNodes; //查找节点

            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == NodeName_Inspection)
                {
                    xn.ParentNode.RemoveChild(xn);

                }
            }
            xmlDoc.Save(XML_Path);
        }

        public int ReadOutputNameList(string OutputInspectionName, string[] outputdata)//Load the outputnamelist
        {
            int Count = int.Parse(ReadXML("VisionParam", OutputInspectionName, "Count"));
            int ListCount = 0;
            for (int i = 1; i < Count + 1; i++)
            {
                string RawData = ReadXML("VisionParam", OutputInspectionName, "Para_" + i.ToString());
                String strCmd = RawData.Trim();
                string[] commandParts = RawData.Split('=');
                if (commandParts[1] != "0")
                {
                    outputdata[ListCount] = commandParts[0];
                    ListCount++;
                }

            }
            return ListCount;
        }

        public int GetNodeNameList(XmlNode FNode, List<string> ListofName, List<string> ListofType)
        {

            if (FNode.HasChildNodes)//如果有孩子节点
            {
                XmlNodeList ChildNodeList = FNode.ChildNodes;

                foreach (XmlNode Node in ChildNodeList)
                {
                    ListofName.Add(Node.Name.ToString());
                    XmlElement XML_Value = (XmlElement)Node;

                    string[] strArray = Node.InnerText.Split('.');

                    ListofType.Add(strArray[1].ToString());

                }
                return ListofName.ToList().Count;
            }
            else
                return 0;//如果父节点没有这个 节点 查询 返回0        
        }
  
  
    }
}
