using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    public class ProductController : ControllerBase
    {
        #region Properties

        private readonly ILogger<ProductController> _productLogger;
        private readonly IProductService _productService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productLogger"></param>
        /// <param name="productService"></param>
        public ProductController(ILogger<ProductController> productLogger, IProductService productService)
        {
            _productLogger = productLogger;
            _productService = productService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all the product list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all/")]
        public async Task<Response<IEnumerable<Product>>> GetProductList()
        {
            try
            {
                _productLogger.LogInformation("Get product list");
                var products = await _productService.GetProductListAsync();
                return ResponseMessage.ToResponseHelper(products);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Gets the product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getById/")]
        public async Task<Response<Product>> GetProduct(int productId)
        {
            try
            {
                _productLogger.LogInformation($"Get product by id {productId}");
                var product = await _productService.GetProductAsync(productId);
                return ResponseMessage.ToResponseHelper(product);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Creates the product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/")]
        public async Task CreateProduct([FromBody] Product product)
        {
            try
            {
                await _productService.CreateProductAsync(product);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/")]
        public async Task<Response<Product>> UpdateProduct([FromBody] Product product)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(product);
                return ResponseMessage.ToResponseHelper(updatedProduct);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Deletes the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("delete/")]
        public async Task<Response<bool>> DeleteProduct(int id)
        {
            try
            {
                var response = await _productService.DeleteProductAsync(id);
                return ResponseMessage.ToResponseHelper(response);
            }
            catch (FLMException exc)
            {
                return ResponseMessage.ToResponseHelper(false, exc.Message, null, false);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Gets the product branches by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getProductBranchById/")]
        public async Task<Response<IEnumerable<BranchProduct>>> GetProductBranch(int productId)
        {
            var product = await _productService.GetBranchProductAsync(productId);
            return ResponseMessage.ToResponseHelper(product);
        }

        /// <summary>
        /// Deletes the product branches by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteProductBranchById/")]
        public async Task DeleteProductBranch(int productId)
        {
            await _productService.DeleteProductBranch(productId);
        }

        [HttpPost]
        [Route("assign/")]
        public async Task AssignBranchProduct([FromBody] List<BranchProduct> branchProduct)
        {
            try
            {
                await _productService.CreateBranchProductListAsync(branchProduct);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        [HttpGet]
        [Route("report/")]
        public async Task<Response<IEnumerable<BranchProduct>>> GetBranchProduct()
        {
            try
            {
                var list = await _productService.GetBranchProductListAsync();
                return ResponseMessage.ToResponseHelper(list);
            }
            catch (Exception exc)
            {
                _productLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        #endregion
    }
}
