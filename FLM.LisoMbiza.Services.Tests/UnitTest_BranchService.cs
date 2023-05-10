using Moq;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLM.LisoMbiza
{
    [TestClass]
    public class UnitTest_BranchService
    {
        #region Properties

        private Mock<IBranchService> _branchServiceMock;
        private Mock<IBranchRepository> _branchRepositoryMock;
        private Mock<IBranchProductRepository> _branchProductRepositoryMock;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the page
        /// </summary>
        [TestInitialize]
        public void InitializePage()
        {
            _branchServiceMock = new Mock<IBranchService>(MockBehavior.Strict);
            _branchRepositoryMock = new Mock<IBranchRepository>(MockBehavior.Strict);
            _branchProductRepositoryMock = new Mock<IBranchProductRepository>(MockBehavior.Strict);
        }

        #endregion

        #region Unit Tests

        [TestMethod]
        public void TestMethod_CreateBranchAsync()
        {
            //Arrange
            var entity = GetBranch();
            _branchRepositoryMock.Setup(x => x.CreateBranchAsync(It.IsAny<Branch>()));

            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            branchService.CreateBranchAsync(entity);

            //Assert
            _branchRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void TestMethod_DeleteBranchAsync_Success()
        {
            //Arrange
            bool success = true;
            var entity = GetBranch(104);
            var branchProducts = GetBranchProducts();
            _branchRepositoryMock.Setup(x => x.DeleteBranchAsync(It.IsAny<int>())).Returns(Task.FromResult(success));
            _branchProductRepositoryMock.Setup(x => x.GetBranchProductListAsync()).Returns(Task.FromResult(branchProducts));
            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = branchService.DeleteBranchAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;
            Assert.AreEqual(success, result);
        }

        [TestMethod]
        public void TestMethod_DeleteBranchAsync_ProductAssigned()
        {
            //Arrange
            var entity = GetBranch(1);
            var branchProducts = GetBranchProducts();
            string errorMessage = "The Branch cannot be deleted as it is linked to a product";
            _branchRepositoryMock.Setup(x => x.DeleteBranchAsync(It.IsAny<int>())).Throws<System.Exception>();
            _branchProductRepositoryMock.Setup(x => x.GetBranchProductListAsync()).Returns(Task.FromResult(branchProducts));
            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = branchService.DeleteBranchAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == TaskStatus.Faulted);
            Assert.IsNotNull(response.Exception.InnerException);
            Assert.AreEqual(errorMessage, response.Exception.InnerException.Message);
        }

        [TestMethod]
        public void TestMethod_GetBranchAsync()
        {
            //Arrange
            var entity = GetBranch();
            _branchRepositoryMock.Setup(x => x.GetBranchAsync(It.IsAny<int>())).Returns(Task.FromResult(entity));

            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = branchService.GetBranchAsync(entity.ID);

            //Assert
            Assert.IsNotNull(response.Result);
            var branch = response.Result;
            Assert.AreEqual(entity.ID, branch.ID);
            Assert.AreEqual(entity.Name, branch.Name);
            Assert.AreEqual(entity.TelephoneNumber, branch.TelephoneNumber);
            Assert.AreEqual(entity.OpenDate, branch.OpenDate);
        }

        [TestMethod]
        public void TestMethod_UpdateBranchAsync()
        {
            //Arrange
            var branchName = "Test 2";
            var entity = GetBranch();
            entity.Name = branchName;
            _branchRepositoryMock.Setup(x => x.UpdateBranchAsync(It.IsAny<Branch>())).Returns(Task.FromResult(entity));

            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = branchService.UpdateBranchAsync(entity);

            //Assert
            Assert.IsNotNull(response.Result);
            var branch = response.Result;
            Assert.AreEqual(entity.ID, branch.ID);
            Assert.AreEqual(branchName, branch.Name);
            Assert.AreEqual(entity.TelephoneNumber, branch.TelephoneNumber);
            Assert.AreEqual(entity.OpenDate, branch.OpenDate);
        }

        [TestMethod]
        public void TestMethod_GetBranchListAsync()
        {
            //Arrange
            var entityList = GetBranchList();
            int numberOfBranches = entityList.ToList().Count;
            _branchRepositoryMock.Setup(x => x.GetAllBranchAsync()).Returns(Task.FromResult(entityList));

            IBranchService branchService = new BranchService(_branchRepositoryMock.Object, _branchProductRepositoryMock.Object);

            //Act
            var response = branchService.GetBranchListAsync();

            //Assert
            Assert.IsNotNull(response.Result);
            var branchList = response.Result.ToList();

            Assert.IsTrue(branchList.Any());
            Assert.AreEqual(numberOfBranches, branchList.Count);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the branch
        /// </summary>
        /// <returns></returns>
        private Branch GetBranch(int branchId = 104)
        {
            return new Branch { ID = branchId, Name = "Test 1", TelephoneNumber = "0215465323", OpenDate = System.DateTime.Now };
        }

        /// <summary>
        /// Gets the branch list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Branch> GetBranchList()
        {
            return new List<Branch> {
                new Branch { ID = 104, Name = "Test 1", TelephoneNumber = "0215465323", OpenDate = System.DateTime.Now },
                new Branch { ID = 205, Name = "Test 2", TelephoneNumber = "0217856124", OpenDate = System.DateTime.Now.AddMonths(-1) }
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
                new BranchProduct { Id = 1, BranchID = 1, ProductID = 10111 }
            };
        }

        #endregion
    }
}
