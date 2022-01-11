// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 5.2.97.0. www.xsd2code.com
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace ThreatsManager.Mitre.Cwe
{
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// The EffectivenessEnumeration simple type defines the different values related to how effective a mitigation may be in preventing the weakness. A value of "High" means the mitigation is frequently successful in eliminating the weakness entirely. A value of "Moderate" means the mitigation will prevent the weakness in multiple forms, but it does not have complete coverage of the weakness. A value of "Limited" means the mitigation may be useful in limited circumstances, or it is only applicable to a subset of potential errors of this weakness type. A value of "Incidental" means the mitigation is generally not effective and will only provide protection by chance, rather than in a reliable manner. A value of "Defense in Depth" means the mitigation may not necessarily prevent the weakness, but it may help to minimize the potential impact of an attacker exploiting the weakness.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
[Serializable]
[XmlTypeAttribute(Namespace="http://cwe.mitre.org/cwe-6")]
public enum EffectivenessEnumeration
{
    High,
    Moderate,
    Limited,
    Incidental,
    [XmlEnumAttribute("Defense in Depth")]
    DefenseinDepth,
    None,
}
}
#pragma warning restore