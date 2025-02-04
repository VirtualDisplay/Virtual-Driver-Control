﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml;

//  I think these should be systemwide, since i need the apppath more then once
//  
//  string appPath = registryFilePath dont know how to access it
//  string xmlPath =  Path.Combine(appPath, "vdd_settings.xml");
//  string scriptPath = Path.Combine(appPath, "scripts"); */

public class VDDSettings
{

    public int Count { get; set; }
    public string Friendlyname { get; set; }
    public List<string> G_refresh_rate { get; set; }
    public List<Resolution> Resolutions { get; set; }
    public bool CustomEdid { get; set; }
    public bool PreventSpoof { get; set; }
    public bool EdidCeaOverride { get; set; }
    public bool HardwareCursor { get; set; }
    public bool SDR10bit { get; set; }
    public bool HDRPlus { get; set; }
    public bool Logging { get; set; }
    public bool DebugLogging { get; set; }

    public VDDSettings()
    {
        G_refresh_rate = new List<string>();
        Resolutions = new List<Resolution>();
    }

    // Inner class for Resolution
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float Refresh_rate { get; set; }
    }
    public void LoadFromXml(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("XML file not found", filePath);

        string xmlContent;
        using (StreamReader reader = new StreamReader(filePath))
        {
            xmlContent = reader.ReadToEnd();
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlContent);

        XmlNode countNode = xmlDoc.SelectSingleNode("//monitors/count");
        if (countNode != null)
            Count = int.Parse(countNode.InnerText);

        XmlNode friendlynameNode = xmlDoc.SelectSingleNode("//gpu/friendlyname");
        if (friendlynameNode != null)
            Friendlyname = friendlynameNode.InnerText;

        G_refresh_rate = new List<string>();
        XmlNodeList refreshRates = xmlDoc.SelectNodes("//global/g_refresh_rate");
        foreach (XmlNode rateNode in refreshRates)
        {
            G_refresh_rate.Add(rateNode.InnerText);
        }

        Resolutions = new List<Resolution>();
        XmlNodeList resolutionNodes = xmlDoc.SelectNodes("//resolutions/resolution");
        foreach (XmlNode resNode in resolutionNodes)
        {
            Resolution res = new Resolution
            {
                Width = int.Parse(resNode.SelectSingleNode("width").InnerText),
                Height = int.Parse(resNode.SelectSingleNode("height").InnerText),
                Refresh_rate = float.Parse(resNode.SelectSingleNode("refresh_rate").InnerText)
            };
            Resolutions.Add(res);
        }

        bool tempValue;
        ParseBooleanOption(xmlDoc, "options/CustomEdid", out tempValue);
        this.CustomEdid = tempValue;

        ParseBooleanOption(xmlDoc, "options/PreventSpoof", out tempValue);
        this.PreventSpoof = tempValue;

        ParseBooleanOption(xmlDoc, "options/EdidCeaOverride", out tempValue);
        this.EdidCeaOverride = tempValue;

        ParseBooleanOption(xmlDoc, "options/HardwareCursor", out tempValue);
        this.HardwareCursor = tempValue;

        ParseBooleanOption(xmlDoc, "options/SDR10bit", out tempValue);
        this.SDR10bit = tempValue;

        ParseBooleanOption(xmlDoc, "options/HDRPlus", out tempValue);
        this.HDRPlus = tempValue;

        ParseBooleanOption(xmlDoc, "options/logging", out tempValue);
        this.Logging = tempValue;

        ParseBooleanOption(xmlDoc, "options/debuglogging", out tempValue);
        this.DebugLogging = tempValue;

    }

    private void ParseBooleanOption(XmlDocument doc, string xpath, out bool value)
    {
        XmlNode node = doc.SelectSingleNode(xpath);
        if (node != null)
        {
            bool.TryParse(node.InnerText, out value);
        }
        else
        {
            value = false; // Default to false if node not found
        }
    }

    public List<string> GetResolutionsForDataGrid()
    {
        List<string> resolutionStrings = new List<string>();
        foreach (var resolution in Resolutions)
        {
            resolutionStrings.Add($"{resolution.Width},{resolution.Height},{resolution.Refresh_rate}");
        }
        return resolutionStrings;
    }


    public void SaveToXml(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("vdd_settings");
        doc.AppendChild(root);

        // Monitors
        XmlElement monitors = doc.CreateElement("monitors");
        XmlElement count = doc.CreateElement("count");
        count.InnerText = Count.ToString();
        monitors.AppendChild(count);
        root.AppendChild(monitors);

        // GPU
        XmlElement gpu = doc.CreateElement("gpu");
        XmlElement friendlyname = doc.CreateElement("friendlyname");
        friendlyname.InnerText = Friendlyname;
        gpu.AppendChild(friendlyname);
        root.AppendChild(gpu);

        // Global
        XmlElement global = doc.CreateElement("global");
        foreach (var rate in G_refresh_rate)
        {
            XmlElement rateElement = doc.CreateElement("g_refresh_rate");
            rateElement.InnerText = rate;
            global.AppendChild(rateElement);
        }
        root.AppendChild(global);

        // Resolutions
        XmlElement resolutions = doc.CreateElement("resolutions");
        foreach (var resolution in Resolutions)
        {
            XmlElement resolutionElement = doc.CreateElement("resolution");
            XmlElement width = doc.CreateElement("width");
            width.InnerText = resolution.Width.ToString();
            resolutionElement.AppendChild(width);

            XmlElement height = doc.CreateElement("height");
            height.InnerText = resolution.Height.ToString();
            resolutionElement.AppendChild(height);

            XmlElement refreshRate = doc.CreateElement("refresh_rate");
            refreshRate.InnerText = resolution.Refresh_rate.ToString();
            resolutionElement.AppendChild(refreshRate);

            resolutions.AppendChild(resolutionElement);
        }
        root.AppendChild(resolutions);

        // Options
        XmlElement options = doc.CreateElement("options");
        AddOptionElement(doc, options, "CustomEdid", CustomEdid);
        AddOptionElement(doc, options, "PreventSpoof", PreventSpoof);
        AddOptionElement(doc, options, "EdidCeaOverride", EdidCeaOverride);
        AddOptionElement(doc, options, "HardwareCursor", HardwareCursor);
        AddOptionElement(doc, options, "SDR10bit", SDR10bit);
        AddOptionElement(doc, options, "HDRPlus", HDRPlus);
        AddOptionElement(doc, options, "logging", Logging);
        AddOptionElement(doc, options, "debuglogging", DebugLogging);
        root.AppendChild(options);

        doc.Save(filePath);
    }

    private void AddOptionElement(XmlDocument doc, XmlElement parent, string name, bool value)
    {
        XmlElement element = doc.CreateElement(name);
        element.InnerText = value.ToString().ToLower();
        parent.AppendChild(element);
    }
}

namespace VDD_Control.Properties {
    using System;


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VDD_Control.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
    }

}
