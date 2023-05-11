
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Branches
{

    private BranchesBranch[] branchField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Branch")]
    public BranchesBranch[] Branch
    {
        get
        {
            return this.branchField;
        }
        set
        {
            this.branchField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class BranchesBranch
{

    private ushort idField;

    private string nameField;

    private string telephoneNumberField;

    private string openDateField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort ID
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TelephoneNumber
    {
        get
        {
            return this.telephoneNumberField;
        }
        set
        {
            this.telephoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string OpenDate
    {
        get
        {
            return this.openDateField;
        }
        set
        {
            this.openDateField = value;
        }
    }
}

