// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.44
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#pragma warning disable
namespace ThreatsManager.Extensions.Panels.ThreatSources.Capec
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType=true, Namespace="http://capec.mitre.org/capec-2")]
public partial class Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolProtocol_Header
{
    #region Private fields
    private List<string> _protocol_RFC;
    private List<string> _protocol_Field_Name;
    private List<string> _protocol_Field_Description;
    private List<string> _protocol_Flag_Description;
    private List<string> _protocol_Flag_Value;
    private List<string> _protocol_Operation_Code;
    private List<string> _protocol_Data;
    private string _id;
    private string _name;
    #endregion
    
    /// <summary>
    /// Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolProtocol_Header class constructor
    /// </summary>
    public Target_Attack_Surface_DescriptionTypeTarget_Functional_ServiceProtocolProtocol_Header()
    {
        _protocol_Data = new List<string>();
        _protocol_Operation_Code = new List<string>();
        _protocol_Flag_Value = new List<string>();
        _protocol_Flag_Description = new List<string>();
        _protocol_Field_Description = new List<string>();
        _protocol_Field_Name = new List<string>();
        _protocol_RFC = new List<string>();
    }
    
    [XmlElement("Protocol_RFC")]
    public List<string> Protocol_RFC
    {
        get => _protocol_RFC;
        set => _protocol_RFC = value;
    }
    
    [XmlElement("Protocol_Field_Name")]
    public List<string> Protocol_Field_Name
    {
        get => _protocol_Field_Name;
        set => _protocol_Field_Name = value;
    }
    
    [XmlElement("Protocol_Field_Description")]
    public List<string> Protocol_Field_Description
    {
        get => _protocol_Field_Description;
        set => _protocol_Field_Description = value;
    }
    
    [XmlElement("Protocol_Flag_Description")]
    public List<string> Protocol_Flag_Description
    {
        get => _protocol_Flag_Description;
        set => _protocol_Flag_Description = value;
    }
    
    [XmlElement("Protocol_Flag_Value")]
    public List<string> Protocol_Flag_Value
    {
        get => _protocol_Flag_Value;
        set => _protocol_Flag_Value = value;
    }
    
    [XmlElement("Protocol_Operation_Code")]
    public List<string> Protocol_Operation_Code
    {
        get => _protocol_Operation_Code;
        set => _protocol_Operation_Code = value;
    }
    
    [XmlElement("Protocol_Data")]
    public List<string> Protocol_Data
    {
        get => _protocol_Data;
        set => _protocol_Data = value;
    }
    
    [XmlAttribute(DataType="integer")]
    public string ID
    {
        get => _id;
        set => _id = value;
    }
    
    [XmlAttribute]
    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
}
#pragma warning restore