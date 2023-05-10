
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Products
{

    private ProductsProduct[] productField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Product")]
    public ProductsProduct[] Product
    {
        get
        {
            return this.productField;
        }
        set
        {
            this.productField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ProductsProduct
{

    private ushort idField;

    private string nameField;

    private string weightedItemField;

    private string suggestedSellingPriceField;

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
    public string WeightedItem
    {
        get
        {
            return this.weightedItemField;
        }
        set
        {
            this.weightedItemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SuggestedSellingPrice
    {
        get
        {
            return this.suggestedSellingPriceField;
        }
        set
        {
            this.suggestedSellingPriceField = value;
        }
    }
}
