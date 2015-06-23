using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.IO;
using System.Security;
public class XmlLorder 
{    
    public void Read()
    {       
        SecurityParser SP = new SecurityParser();       
        // 假设xml文件路径为 Resources/test.xml
        string xmlPath = "test.xml";
        SP.LoadXml(Resources.Load( xmlPath ).ToString()); 
        SecurityElement SE = SP.ToXml();
        foreach (SecurityElement child in SE.Children)
        {           //比对下是否使自己所需要得节点            
            if(child.Tag == "table")            
            {               //获得节点得属性               
                string wave = child.Attribute("wave");
                string level = child.Attribute("level");
                string name = child.Attribute("name");
                Debug.Log("wave:" + wave + " level:" + level + " name:" + name); 
            }
        }
    }
}