using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using Cognex.VS.Utility;

namespace CS_Note1
{
    class OpXMLFile
    { 
        private string xmlfilepath;
        XmlNode Xmlroot;

        XmlDocument opmldoc;

        // 实现对Opml频道文件的操作
        public bool OprationOpmlFile(string file, string[] xpathstr, string rootNode,string OprationMode, string Title, string Url)
        {
            try
            {
                XmlDocument opmldoc = new XmlDocument();
                opmldoc.PreserveWhitespace = true;
                opmldoc.Load(file);

                XmlNode body = opmldoc.SelectSingleNode("//"+ rootNode);
                switch (OprationMode)
                { 
                    case "AddML":
                        foreach (string s in xpathstr)
                        {
                            XmlNode AddML = body.SelectSingleNode(s.ToString());
                            body = AddML;
                        }

                        XmlElement appml = opmldoc.CreateElement(Title); 
                        body.AppendChild(appml);
                        

                        opmldoc.Save(file);
                        break;
                    case "Del":
                        foreach (string s in xpathstr)
                        {
                            XmlNode Deling = body.SelectSingleNode(s.ToString());                         
                            if(Deling==null)
                            {
                                string sExtName = Path.GetExtension(s);
                                // 如果有写扩展名 则删除
                                if (sExtName.Length != 0)
                                {
                                    Deling = body.SelectSingleNode(s.Remove(s.Length - sExtName.Length , sExtName.Length));
                                }
                            }
                          
                            body = Deling;
                        }
                        if (body == null)
                            break;
                        XmlNode parent = body.ParentNode;
                        parent.RemoveChild(body);
                        opmldoc.Save(file);

                        break;
                    case "Del_All":
                        foreach (string s in xpathstr)
                        {
                            XmlNode Deling = body.SelectSingleNode(s.ToString());
                            if (Deling == null)
                            {
                                string sExtName = Path.GetExtension(s);
                                // 如果有写扩展名 则删除
                                if (sExtName.Length != 0)
                                {
                                    Deling = body.SelectSingleNode(s.Remove(s.Length - sExtName.Length, sExtName.Length));
                                }
                            }

                            body = Deling;
                        }
                        if (body == null)
                            break;
                        XmlNode curparent = body;
                        curparent.RemoveAll();
                        opmldoc.Save(file);

                        break;
                    case "Rename":
                        foreach (string s in xpathstr)
                        {
                            XmlNode Rename = body.SelectSingleNode(s.ToString() );
                            if (Rename == null)
                            {
                                string sExtName = Path.GetExtension(s);
                                // 如果有写扩展名 则删除
                                if (sExtName.Length != 0)
                                {
                                    Rename = body.SelectSingleNode(s.Remove(s.Length - sExtName.Length, sExtName.Length));
                                }
                            }
                            body = Rename;
                        }
                        
                       // XmlElement oldNameNode= opmldoc.
                        XmlElement NewNameNode = opmldoc.CreateElement(Title);
                        XmlNode OldNodeParent = body.ParentNode;
                        NewNameNode.InnerXml = body.InnerXml;
                        // opmldoc.RemoveChild(body);
                        OldNodeParent.ReplaceChild((XmlNode)NewNameNode, body);
                       // opmldoc.AppendChild(NewNameNode);


                        opmldoc.Save(file);
                        break;
                }
                return true;
            }
            catch(Exception ex)
            {
                string ErrorLog = ex.ToString();
                return false;
            }
        }

        //创建顶层目录
        public bool CreateRootSon(string file, string Title)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);

                XmlNode body = doc.SelectSingleNode("//body");

                XmlElement appml = doc.CreateElement("outline");
                appml.SetAttribute("title", Title);
                body.AppendChild(appml);

                doc.Save(file);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool WriteXMLAbortRollback(string XMLFilePath,TreeView TheTreeView,string branchName,string inspectionNodeName, string OprationMode, string vppName,string verName)
        {
            opmldoc = new XmlDocument();
            XmlElement xmlnode;
            XmlElement xmlFirstNode;
            XmlElement xmlSecondNode;
            XmlElement xmlThirdNode;
            xmlfilepath = XMLFilePath;
            opmldoc.Load(XMLFilePath);
            XmlNode rollback = opmldoc.SelectSingleNode("//Rollback");

            switch(OprationMode)
            {
                case "AddNewBranch":
                    for (int i = 0; i < TheTreeView.Nodes.Count; i++)
                    {
                        i = TheTreeView.Nodes.Count - 1;//只增加最新添加的branch
                        xmlnode = opmldoc.CreateElement(TheTreeView.Nodes[i].Text);
                        for(int ii=0;ii<TheTreeView.Nodes[i].Nodes.Count;ii++)
                        {
                            if(TheTreeView.Nodes[i].Nodes[ii].Text == inspectionNodeName)
                            {
                                xmlSecondNode = opmldoc.CreateElement(TheTreeView.Nodes[i].Nodes[ii].Text);
                                for (int iii=0;iii< TheTreeView.Nodes[i].Nodes[ii].Nodes.Count;iii++)
                                {
                                    string InspName = TheTreeView.Nodes[i].Nodes[ii].Nodes[iii].Text;
                                    string sExtName = Path.GetExtension(InspName);
                                    // 如果有写扩展名 则删除
                                    if (sExtName.Length != 0)
                                    {
                                        InspName= InspName.Remove(InspName.Length- sExtName.Length, sExtName.Length) ;
                                    }
                                    XmlElement inspElement = opmldoc.CreateElement(InspName);
                                    xmlSecondNode.AppendChild(inspElement);
                                }
                                xmlnode.AppendChild(xmlSecondNode);
                            }
                        }                     
                        rollback.AppendChild(xmlnode);

                    }
                    opmldoc.Save(xmlfilepath);
                    break;
                case "Rename":

                    break;
                case "Del":
                    //string oldPath = TheTreeView.SelectedNode.FullPath.ToString().Replace("\\", "/");
                    string[] xpathstr = new string[]{branchName, inspectionNodeName, vppName, verName };//System.Text.RegularExpressions.Regex.Split(oldPath, "/");
                    foreach (string s in xpathstr)
                    {
                        XmlNode Deling = rollback.SelectSingleNode(s.ToString());
                        rollback = Deling;
                    }
                    if (rollback == null)
                        break;
                    XmlNode parent = rollback.ParentNode;
                    parent.RemoveChild(rollback);
                    //parent.RemoveAll();
                    opmldoc.Save(xmlfilepath);

                    break;
                case "Del_All":
                    //string oldPath = TheTreeView.SelectedNode.FullPath.ToString().Replace("\\", "/");
                    string[] pathstr = new string[] { branchName, inspectionNodeName, vppName};//System.Text.RegularExpressions.Regex.Split(oldPath, "/");
                    foreach (string s in pathstr)
                    {
                        XmlNode Deling = rollback.SelectSingleNode(s.ToString());
                        rollback = Deling;
                    }
                    if (rollback == null)
                        break;
                    XmlNode curparent = rollback;                   
                    curparent.RemoveAll();
                    opmldoc.Save(xmlfilepath);

                    break;
                case "AddNewVision":
                    //WriteXML(XMLFilePath, "Rollback", branchName, vppName, "Version_3.3.0.1", "FALSE");

                    break;
            }

            return true;
        }
        public bool UpdateOpmlFile(string XMLFilePath, TreeView TheTreeView, string Title,string Url)
        {

            opmldoc = new XmlDocument();
            xmlfilepath = XMLFilePath;
            opmldoc.Load(XMLFilePath);

            XmlNode body = opmldoc.SelectSingleNode("//body");

            //------选中根节点  
            XmlElement Xmlnode = opmldoc.CreateElement(TheTreeView.Nodes[0].Text);


            //------遍历原treeview控件，并生成相应的XML  
            body.RemoveAll();
            TransTreeSav(TheTreeView.Nodes, (XmlElement)body);
         
            return false;
        }

        private int TransTreeSav(TreeNodeCollection nodes, XmlElement ParXmlnode)
        {

            //-------遍历树的各个故障节点，同时添加节点至XML  
            XmlElement xmlnode;
            Xmlroot = opmldoc.SelectSingleNode("//body");

            foreach (TreeNode node in nodes)
            {
                xmlnode = opmldoc.CreateElement(node.Text);
               
                ParXmlnode.AppendChild(xmlnode);

                if (node.Nodes.Count > 0)
                {
                    TransTreeSav(node.Nodes, xmlnode);
                }
            }
            opmldoc.Save(xmlfilepath);
            return 0;
        }

        /// <summary>
        /// 读取XML配置文件的参数设置，获取下载的TXT文件路径与上传的数据文件路径
        /// </summary>
        /// <param name="Label_Fir">First Group</param>
        /// <param name="Label_Sec">Second Group</param>
        /// <param name="Label_Thir">Third Group</param>
        /// <returns>Value</returns>
        public string ReadXML(string file,string Label_Fir, string Label_Sec, string Label_Thir)
        {
            string Result = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //Load XML File
                xmlDoc.Load(file);

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
        public void WriteXML(string file,string Label_Fir, string Label_Sec, string Label_Thir, string Label_Value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

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
                xmlDoc.Save(file);
            }
            catch
            {

            };
        }
        public void WriteXML(string file, string Label_Fir, string Label_Sec, string Label_Thir, string Label_Fourth,string Label_Value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                XmlNode rootNode = xmlDoc.SelectSingleNode("//Rollback");
                rootNode = rootNode.SelectSingleNode(Label_Sec);//默认有第一节点，直接加载第二节点，如果为空，则创建二级节点。
                if (rootNode == null)//如果没有第二层节点
                {
                    XmlNode root = xmlDoc.SelectSingleNode("//Rollback");//查找根节点   
                    XmlElement SecNode = xmlDoc.CreateElement(Label_Sec);//创建二级节点   
                    XmlElement ThirdNode = xmlDoc.CreateElement(Label_Thir);//创建三级节点
                    XmlElement FourthNode = xmlDoc.CreateElement(Label_Fourth);//创建四级节点
                    FourthNode.InnerText = Label_Value;//设置文本节点
                    ThirdNode.AppendChild(FourthNode);
                    SecNode.AppendChild(ThirdNode);
                    root.AppendChild(SecNode);//添加到根节点   
                }
                else//有第二层节点，则更新第三层节点
                {
                    XmlElement XML_Value = (XmlElement)rootNode.SelectSingleNode(Label_Thir);//如果没有第三层节点，则XML_Value==null

                    if (XML_Value == null)//如果没有第三层节点
                    {
                        XmlNode root = rootNode;//当前根节点为第二层节点。   
                        XmlElement ThirdNode = xmlDoc.CreateElement(Label_Thir);
                        XmlElement FourthNode = xmlDoc.CreateElement(Label_Fourth);//创建四级节点
                        FourthNode.InnerText = Label_Value;//设置文本节点
                        ThirdNode.AppendChild(FourthNode);                        
                        root.AppendChild(ThirdNode);//添加到根节点   
                    }
                    else//如果有第三层节点，则更新第三层节点的值
                    {
                        XmlElement RoodElement = (XmlElement)XML_Value.SelectSingleNode(Label_Fourth);//如果没有第四层节点，RoolElement==null
                        if (RoodElement == null)//如果没有第四层节点
                        {
                            XmlNode root = XML_Value;//查找根节点   
                            XmlElement  FourthNode = xmlDoc.CreateElement(Label_Fourth);                            
                            FourthNode.InnerText = Label_Value;//设置文本节点   
                            root.AppendChild(FourthNode);//添加到根节点中   
                        }
                        else//更新第三层节点的值
                        {
                            XmlNode root = XML_Value;//查找根节点  
                            RoodElement.InnerText = Label_Value;
                            root.AppendChild(RoodElement);//添加到根节点中   
                        }

                        //XML_Value.InnerText = Label_Value;
                    }
                }
                xmlDoc.Save(file);
            }
            catch
            {

            };
        }
        public void WriteXML(string file, string Label_Fir, string Label_Sec, string Label_Thir, string Label_Fourth, string Label_Fifth, string Label_Value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                XmlNode rootNode = xmlDoc.SelectSingleNode("//"+ Label_Fir);
                rootNode = rootNode.SelectSingleNode(Label_Sec);//默认有第一节点，直接加载第二节点，如果为空，则创建二级节点。
                if (rootNode == null)//如果没有第二层节点
                {
                    XmlNode root = xmlDoc.SelectSingleNode("//"+ Label_Fir);//查找根节点   
                    XmlElement SecNode = xmlDoc.CreateElement(Label_Sec);//创建二级节点   
                    XmlElement ThirdNode = xmlDoc.CreateElement(Label_Thir);//创建三级节点
                    XmlElement FourthNode = xmlDoc.CreateElement(Label_Fourth);//创建四级节点
                    XmlElement FifthNode = xmlDoc.CreateElement(Label_Fifth);//创建五级节点
                    FifthNode.InnerText = Label_Value;//设置文本节点
                    FourthNode.AppendChild(FifthNode);
                    ThirdNode.AppendChild(FourthNode);
                    SecNode.AppendChild(ThirdNode);
                    root.AppendChild(SecNode);//添加到根节点   
                }
                else//有第二层节点，则更新第三层节点
                {
                    XmlElement XML_Value = (XmlElement)rootNode.SelectSingleNode(Label_Thir);//如果没有第三层节点，则XML_Value==null

                    if (XML_Value == null)//如果没有第三层节点
                    {
                        XmlNode root = rootNode;//当前根节点为第二层节点。   
                        XmlElement ThirdNode = xmlDoc.CreateElement(Label_Thir);
                        XmlElement FourthNode = xmlDoc.CreateElement(Label_Fourth);//创建四级节点
                        XmlElement FifthNode = xmlDoc.CreateElement(Label_Fifth);//创建五级节点
                        FifthNode.InnerText = Label_Value;//设置文本节点
                                                           
                        FourthNode.AppendChild(FifthNode);
                        ThirdNode.AppendChild(FourthNode);
                        root.AppendChild(ThirdNode);//添加到根节点   
                    }
                    else//如果有第三层节点，则更新第三层节点的值
                    {
                        XmlElement RoodElement = (XmlElement)XML_Value.SelectSingleNode(Label_Fourth);//如果没有第四层节点，RoolElement==null
                        if (RoodElement == null)//如果没有第四层节点
                        {
                            XmlNode root = XML_Value;//查找根节点   
                            XmlElement FourthNode = xmlDoc.CreateElement(Label_Fourth);
                            XmlElement FifthNode = xmlDoc.CreateElement(Label_Fifth);//创建五级节点
                            FifthNode.InnerText = Label_Value;//设置文本节点
                            FourthNode.AppendChild(FifthNode);  
                            root.AppendChild(FourthNode);//添加到根节点中   
                        }
                        else//更新第四层节点的值
                        {
                            XmlElement FifthRoodElement = (XmlElement)RoodElement.SelectSingleNode(Label_Fifth);//如果没有第五层节点，RoolElement==null
                            if (FifthRoodElement == null)//如果没有第五层节点
                            {
                                XmlNode root = RoodElement;//查找根节点   
                                XmlElement FifthNode = xmlDoc.CreateElement(Label_Fifth);
                                FifthNode.InnerText = Label_Value;//设置文本节点   
                                root.AppendChild(FifthNode);//添加到根节点中   
                            }
                            else//更新第五层节点的值
                            {
                                XmlNode root = RoodElement;//查找根节点   
                                FifthRoodElement.InnerText = Label_Value;
                                root.AppendChild(FifthRoodElement);//添加到根节点中   
                            }
                           
                        }

                        //XML_Value.InnerText = Label_Value;
                    }
                }
                xmlDoc.Save(file);
            }
            catch
            {

            };
        }

        public void DelXml(string file,string NodeName_Inspection)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(file);

            XmlNodeList xnl = xmlDoc.SelectSingleNode("VisionParam").ChildNodes; //查找节点

            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == NodeName_Inspection)
                {
                    xn.ParentNode.RemoveChild(xn);

                }
            }
            xmlDoc.Save(file);
        }

        public int ReadOutputNameList(string file,string OutputInspectionName, string[] outputdata)//Load the outputnamelist
        {
            int Count = int.Parse(ReadXML(file,"VisionParam", OutputInspectionName, "Count"));
            int ListCount = 0;
            for (int i = 1; i < Count + 1; i++)
            {
                string RawData = ReadXML(file,"VisionParam", OutputInspectionName, "Para_" + i.ToString());
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
