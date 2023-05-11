using Moq;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLM.LisoMbiza
{
    [TestClass]
    public class UnitTest_ProductService
    {
        #region Properties

        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IBranchProductRepository> _branchProductRepositoryMock;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the page
        /// </summary>
        [TestInitialize]
        public void InitializePage()
        {
            _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            _branchProductRepositoryMock = new Mock<IBranchProductRepository>(MockBehavior.Strict);
        }

        #endregion

        #region Unit Tests

        [TestMethod]
        public void TestMethod_CreateProductAsync()
        {
            //Arrange
            var entity = GetProduct();
            _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>()));

            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            productService.CreateProductAsync(entity);

            //Assert
            _productRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void TestMethod_DeleteProductAsync_Success()
        {
            //Arrange
            bool success = true;
            var entity = GetProduct();
            var branchProducts = GetBranchProducts();
            _productRepositoryMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>())).Returns(Task.FromResult(success));
            _branchProductRepositoryMock.Setup(x => x.GetBranchProductListAsync()).Returns(Task.FromResult(branchProducts));
            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = productService.DeleteProductAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;
            Assert.AreEqual(success, result);
        }

        [TestMethod]
        public void TestMethod_DeleteProductAsync_BranchAssigned()
        {
            //Arrange
            var entity = GetProduct(10111);
            var branchProducts = GetBranchProducts();
            string errorMessage = "The Product cannot be deleted as it is linked to a branch";
            _productRepositoryMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>())).Throws<System.Exception>();
            _branchProductRepositoryMock.Setup(x => x.GetBranchProductListAsync()).Returns(Task.FromResult(branchProducts));
            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = productService.DeleteProductAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == TaskStatus.Faulted);
            Assert.IsNotNull(response.Exception.InnerException);
            Assert.AreEqual(errorMessage, response.Exception.InnerException.Message);
        }

        [TestMethod]
        public void TestMethod_GetProductAsync()
        {
            //Arrange
            var entity = GetProduct();
            _productRepositoryMock.Setup(x => x.GetProductAsync(It.IsAny<int>())).Returns(Task.FromResult(entity));

            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = productService.GetProductAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response.Result);
            var product = response.Result;
            Assert.AreEqual(entity.ID, product.ID);
            Assert.AreEqual(entity.Name, product.Name);
            Assert.AreEqual(entity.WeightedItem, product.WeightedItem);
            Assert.AreEqual(entity.SuggestedSellingPrice, product.SuggestedSellingPrice);
        }

        [TestMethod]
        public void TestMethod_UpdateProductAsync()
        {
            //Arrange
            var branchName = "Test 2";
            var entity = GetProduct();
            entity.Name = branchName;
            _productRepositoryMock.Setup(x => x.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.FromResult(entity));

            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = productService.UpdateProductAsync(entity);

            //Assert
            Assert.IsNotNull(response.Result);
            var product = response.Result;
            Assert.AreEqual(entity.ID, product.ID);
            Assert.AreEqual(branchName, product.Name);
            Assert.AreEqual(entity.WeightedItem, product.WeightedItem);
            Assert.AreEqual(entity.SuggestedSellingPrice, product.SuggestedSellingPrice);
        }

        [TestMethod]
        public void TestMethod_GetProductListAsync()
        {
            //Arrange
            var entityList = GetProductList();
            int numberOfProducts = entityList.ToList().Count;
            _productRepositoryMock.Setup(x => x.GetAllProductAsync()).Returns(Task.FromResult(entityList));

            IProductService productService = new ProductService(_productRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = productService.GetProductListAsync();

            //Assert
            Assert.IsNotNull(response.Result);
            var productList = response.Result.ToList();

            Assert.IsTrue(productList.Any());
            Assert.AreEqual(numberOfProducts, productList.Count);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the product
        /// </summary>
        /// <returns></returns>
        private Product GetProduct(int productId = 1)
        {
            return new Product { ID = productId, Name = "Test Product 1", WeightedItem = false, SuggestedSellingPrice = 129 };
        }

        /// <summary>
        /// Gets the product list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Product> GetProductList()
        {
            return new List<Product> {
                new Product { ID = 1, Name = "Test Product 1", WeightedItem = false, SuggestedSellingPrice = 129 },
                new Product { ID = 2, Name = "Test Product 2", WeightedItem = true, SuggestedSellingPrice = 529 }
            };
        }

        /// <summary>
        /// Gets the branch products
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BranchProduct> GetBranchProducts()
        {
            return new List<BranchProduct>
            {
                new BranchProduct { Id = 1, BranchID = 104, ProductID = 10111 }
            };
        }

        #endregion
    }
}
