
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Mappings
{

    private MappingsBranchProduct[] branchProductField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("BranchProduct")]
    public MappingsBranchProduct[] BranchProduct
    {
        get
        {
            return this.branchProductField;
        }
        set
        {
            this.branchProductField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class MappingsBranchProduct
{

    private ushort branchIDField;

    private ushort productIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort BranchID
    {
        get
        {
            return this.branchIDField;
        }
        set
        {
            this.branchIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort ProductID
    {
        get
        {
            return this.productIDField;
        }
        set
        {
            this.productIDField = value;
        }
    }
}

