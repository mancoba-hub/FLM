using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    [TestClass]
    public class UnitTest_ProductController
    {
        #region Properties

        private Mock<ILogger<ProductController>> _productControllerLoggerMock;
        private Mock<IProductService> _productServiceMock;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the page
        /// </summary>
        [TestInitialize]
        public void InitializePage()
        {
            _productControllerLoggerMock = new Mock<ILogger<ProductController>>();
            _productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
        }

        #endregion

        #region Unit Tests

        [TestMethod]
        public void TestMethod_GetProductList()
        {
            //Arrange
            var products = GetProductList();
            _productServiceMock.Setup(x => x.GetProductListAsync()).Returns(Task.FromResult(products));
            ProductController productController = new ProductController(_productControllerLoggerMock.Object, _productServiceMock.Object);

            //Act
            var response = productController.GetProductList();

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        [TestMethod]
        public void TestMethod_GetProduct_ById()
        {
            //Arrange
            int productId = 45682;
            var branch = GetProduct(productId);
            _productServiceMock.Setup(x => x.GetProductAsync(It.IsAny<int>())).Returns(Task.FromResult(branch));
            ProductController productController = new ProductController(_productControllerLoggerMock.Object, _productServiceMock.Object);

            //Act
            var response = productController.GetProduct(productId);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(productId, result.Data.ID);
        }

        [TestMethod]
        public void TestMethod_CreateProduct()
        {
            //Arrange
            var product = GetProduct();
            _productServiceMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            ProductController productController = new ProductController(_productControllerLoggerMock.Object, _productServiceMock.Object);

            //Act
            var response = productController.CreateProduct(product);

            //Assert
            _productControllerLoggerMock.VerifyAll();
            _productServiceMock.VerifyAll();
        }

        [TestMethod]
        public void TestMethod_UpdateProduct()
        {
            //Arrange
            string productName = "Unit Test Update Product 1";
            bool weightedItem = false;
            decimal price = (decimal)299.99;
            var product = GetProduct();
            product.Name = productName;
            product.WeightedItem = weightedItem;
            product.SuggestedSellingPrice = price;
            _productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.FromResult(product));
            ProductController productController = new ProductController(_productControllerLoggerMock.Object, _productServiceMock.Object);

            //Act
            var response = productController.UpdateProduct(product);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(productName, result.Data.Name);
            Assert.AreEqual(weightedItem, result.Data.WeightedItem);
            Assert.AreEqual(price, result.Data.SuggestedSellingPrice);
        }

        [TestMethod]
        public void TestMethod_DeleteProduct()
        {
            //Arrange
            bool deleted = true;
            int productId = 45682;
            _productServiceMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>())).Returns(Task.FromResult(deleted));
            ProductController productController = new ProductController(_productControllerLoggerMock.Object, _productServiceMock.Object);

            //Act
            var response = productController.DeleteProduct(productId);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(deleted, result.Data);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the product list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Product> GetProductList()
        {
            return new List<Product>
            {
                new Product { ID = 45682, Name = "Unit Test Product 1", WeightedItem = true, SuggestedSellingPrice = (decimal)199.99 },
                new Product { ID = 89453, Name = "Unit Test Product 2", WeightedItem = false, SuggestedSellingPrice = (decimal)599.99 },
            };
        }

        /// <summary>
        /// Gets the product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private Product GetProduct(int productId = 45682)
        {
            return GetProductList().FirstOrDefault(x => x.ID == productId);
        }

        #endregion
    }
}
