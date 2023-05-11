using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FLM.LisoMbiza
{
    public class ImportService : IImportService
    {
        #region Properties

        private readonly IBranchService _branchService;
        private readonly IProductService _productService;
        private readonly IBranchProductService _branchProductService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportService"/> class.
        /// </summary>
        /// <param name="branchService"></param>
        /// <param name="productService"></param>
        /// <param name="branchProductService"></param>
        public ImportService(IBranchService branchService, IProductService productService, IBranchProductService branchProductService)
        {
            _branchService = branchService;
            _productService = productService;
            _branchProductService = branchProductService;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Imports the branch list
        /// </summary>
        /// <param name="stream"></param>
        public async Task ImportBranchList(Stream stream, string contentType)
        {
            List<Branch> branchList = new List<Branch>();
            switch (contentType)
            {
                case "text/xml":
                    branchList = GetXmlBranchList(stream);
                    break;
                case "application/json":
                    branchList = GetJsonBranchList(stream);
                    break;
                default:
                    branchList = GetCsvBranchList(stream);
                    break;
            }
            await _branchService.CreateBranchListAsync(branchList);
        }

        /// <summary>
        /// Imports the product list
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task ImportProductList(Stream stream, string contentType)
        {
            List<Product> productList = new List<Product>();
            switch (contentType)
            {
                case "text/xml":
                    productList = GetXmlProductList(stream);
                    break;
                case "application/json":
                    productList = GetJsonProductList(stream);
                    break;
                default:
                    productList = GetCsvProductList(stream);
                    break;
            }
            await _productService.CreateProductListAsync(productList);
        }

        /// <summary>
        /// Imports the branch product list
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task ImportBranchProductList(Stream stream, string contentType)
        {
            List<BranchProduct> branchProductList = new List<BranchProduct>();
            switch (contentType)
            {
                case "text/xml":
                    branchProductList = GetXmlBranchProductList(stream);
                    break;
                case "application/json":
                    branchProductList = GetJsonBranchProductList(stream);
                    break;
                default:
                    branchProductList = GetCsvBranchProductList(stream);
                    break;
            }
            await _branchProductService.CreateBranchProduct(branchProductList);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the XML branch list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Branch> GetXmlBranchList(Stream stream)
        {
            List<Branch> branches = new List<Branch>();
            XmlSerializer sFormater = new XmlSerializer(typeof(Branches), new XmlRootAttribute("Branches"));
            var itemList = (Branches)sFormater.Deserialize(stream);

            foreach (var item in itemList.Branch)
            {
                branches.Add(
                    new Branch
                    {
                        ID = item.ID,
                        Name = item.Name,
                        OpenDate = item.OpenDate.ToDateTime() == DateTime.MinValue ? null : item.OpenDate.ToDateTime(),
                        TelephoneNumber = item.TelephoneNumber
                    });
            }
            return branches;
        }

        /// <summary>
        /// Gets the XML product list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Product> GetXmlProductList(Stream stream)
        {
            List<Product> products = new List<Product>();
            XmlSerializer sFormater = new XmlSerializer(typeof(Products), new XmlRootAttribute("Products"));
            var itemList = (Products)sFormater.Deserialize(stream);

            foreach(var item in itemList.Product)
            {
                products.Add(
                    new Product
                    {
                        ID = item.ID,
                        Name = item.Name,
                        WeightedItem = item.WeightedItem == "Y" ? true : false,
                        SuggestedSellingPrice = item.SuggestedSellingPrice.ToDecimal()
                });
            }
            return products;
        }

        /// <summary>
        /// Imports the branch product list 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<BranchProduct> GetXmlBranchProductList(Stream stream)
        {
            List<BranchProduct> branchProducts = new List<BranchProduct>();
            XmlSerializer sFormater = new XmlSerializer(typeof(Mappings), new XmlRootAttribute("Mappings"));
            var itemList = (Mappings)sFormater.Deserialize(stream);

            foreach (var item in itemList.BranchProduct)
            {
                branchProducts.Add(
                    new BranchProduct
                    {
                        BranchID = item.BranchID,
                        ProductID = item.ProductID
                    });
            }
            return branchProducts;
        }

        /// <summary>
        /// Gets the JSON branch list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Branch> GetJsonBranchList(Stream stream)
        {
            List<Branch> branches = new List<Branch>();
            int count = -1;
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    jsonReader.SupportMultipleContent = true;
                    
                    while (jsonReader.Read())
                    {
                        count++;

                        if (jsonReader.LineNumber == 1)
                            continue;

                        if (count == 0)
                            continue;

                        branches.Add(serializer.Deserialize<Branch>(jsonReader));
                    }
                }
            }

            return branches;
        }

        /// <summary>
        /// Gets the JSON product list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Product> GetJsonProductList(Stream stream)
        {
            List<Product> products = new List<Product>();
            int count = -1;
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    jsonReader.SupportMultipleContent = true;

                    while (jsonReader.Read())
                    {
                        count++;

                        if (jsonReader.LineNumber == 1)
                            continue;

                        if (count == 0)
                            continue;

                        products.Add(serializer.Deserialize<Product>(jsonReader));
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Gets the JSON branch product list 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<BranchProduct> GetJsonBranchProductList(Stream stream)
        {
            List<BranchProduct> branchProducts = new List<BranchProduct>();
            int count = -1;
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    jsonReader.SupportMultipleContent = true;

                    while (jsonReader.Read())
                    {
                        count++;

                        if (jsonReader.LineNumber == 1)
                            continue;

                        if (count == 0)
                            continue;

                        branchProducts.Add(serializer.Deserialize<BranchProduct>(jsonReader));
                    }
                }
            }

            return branchProducts;
        }

        /// <summary>
        /// Gets the CSV branch list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Branch> GetCsvBranchList(Stream stream)
        {
            int count = -1;
            List<Branch> branches = new List<Branch>();
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    count++;
                    string[] rows = sr.ReadLine().Split(',');

                    if (count == 0)
                        continue;

                    branches.Add(new Branch { ID = rows[0].ToInt32(), Name = rows[1], TelephoneNumber = rows[2], OpenDate = rows[3].ToDateTime() });
                }
            }
            return branches;
        }

        /// <summary>
        /// Gets the CSV product list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<Product> GetCsvProductList(Stream stream)
        {
            int count = -1;
            List<Product> products = new List<Product>();
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    count++;
                    string[] rows = sr.ReadLine().Split(',');

                    if (count == 0)
                        continue;

                    products.Add(new Product { ID = rows[0].ToInt32(), Name = rows[1], WeightedItem = rows[2].ToString() == "Y" ? true : false, SuggestedSellingPrice = rows[3].ToDecimal() });
                }
            }
            return products;
        }

        /// <summary>
        /// Gets the CSV branch product list
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<BranchProduct> GetCsvBranchProductList(Stream stream)
        {
            int count = -1;
            List<BranchProduct> products = new List<BranchProduct>();
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    count++;
                    string[] rows = sr.ReadLine().Split(',');

                    if (count == 0)
                        continue;

                    products.Add(new BranchProduct { BranchID = rows[0].ToInt32(), ProductID = rows[1].ToInt32() });
                }
            }
            return products;
        }

        #endregion
    }
}
