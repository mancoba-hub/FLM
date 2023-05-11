using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    [TestClass]
    public class UnitTest_BranchController
    {
        #region Properties

        private Mock<ILogger<BranchController>> _branchControllerLoggerMock;
        private Mock<IBranchService> _branchServiceMock;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the page
        /// </summary>
        [TestInitialize]
        public void InitializePage()
        {
            _branchControllerLoggerMock = new Mock<ILogger<BranchController>>();
            _branchServiceMock = new Mock<IBranchService>(MockBehavior.Strict);
        }

        #endregion

        #region Unit Tests

        [TestMethod]
        public void TestMethod_GetBranchList()
        {
            //Arrange
            var branches = GetBranchList();
            _branchServiceMock.Setup(x => x.GetBranchListAsync()).Returns(Task.FromResult(branches));
            BranchController branchController = new BranchController(_branchControllerLoggerMock.Object, _branchServiceMock.Object);

            //Act
            var response = branchController.GetBranchList();

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        [TestMethod]
        public void TestMethod_GetBranch_ById()
        {
            //Arrange
            int branchId = 1004;
            var branch = GetBranch(branchId);
            _branchServiceMock.Setup(x => x.GetBranchAsync(It.IsAny<int>())).Returns(Task.FromResult(branch));
            BranchController branchController = new BranchController(_branchControllerLoggerMock.Object, _branchServiceMock.Object);

            //Act
            var response = branchController.GetBranch(branchId);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(branchId, result.Data.ID);
        }

        [TestMethod]
        public void TestMethod_CreateBranch()
        {
            //Arrange
            var branch = GetBranch();
            _branchServiceMock.Setup(x => x.CreateBranchAsync(It.IsAny<Branch>())).Returns(Task.CompletedTask);
            BranchController branchController = new BranchController(_branchControllerLoggerMock.Object, _branchServiceMock.Object);

            //Act
            var response = branchController.CreateBranch(branch);

            //Assert
            _branchControllerLoggerMock.VerifyAll();
            _branchServiceMock.VerifyAll();
        }

        [TestMethod]
        public void TestMethod_UpdateBranch()
        {
            //Arrange
            string branchName = "Unit Test Update Branch 1";
            string telephoneNumber = "27 21 546 8789";
            var branch = GetBranch();
            branch.Name = branchName;
            branch.TelephoneNumber = telephoneNumber;
            _branchServiceMock.Setup(x => x.UpdateBranchAsync(It.IsAny<Branch>())).Returns(Task.FromResult(branch));
            BranchController branchController = new BranchController(_branchControllerLoggerMock.Object, _branchServiceMock.Object);

            //Act
            var response = branchController.UpdateBranch(branch);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(branchName, result.Data.Name);
            Assert.AreEqual(telephoneNumber, result.Data.TelephoneNumber);
        }

        [TestMethod]
        public void TestMethod_DeleteBranch()
        {
            //Arrange
            bool deleted = true;
            int branchId = 1004;
            _branchServiceMock.Setup(x => x.DeleteBranchAsync(It.IsAny<int>())).Returns(Task.FromResult(deleted));
            BranchController branchController = new BranchController(_branchControllerLoggerMock.Object, _branchServiceMock.Object);

            //Act
            var response = branchController.DeleteBranch(branchId);

            //Assert
            Assert.IsNotNull(response);
            var result = response.Result;
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(deleted, result.Data);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the branch list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Branch> GetBranchList()
        {
            return new List<Branch>
            {
                new Branch { ID = 1004, Name = "Unit Test Branch 1", TelephoneNumber = "27 82 619 6884", OpenDate = System.DateTime.Now.AddMonths(-2)},
                new Branch { ID = 1005, Name = "Unit Test Branch 2", TelephoneNumber = "27 76 985 7845", OpenDate = System.DateTime.Now.AddMonths(-1)},
            };
        }

        /// <summary>
        /// Gets the branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        private Branch GetBranch(int branchId = 1004)
        {
            return GetBranchList().FirstOrDefault(x => x.ID == branchId);
        }

        #endregion
    }
}
