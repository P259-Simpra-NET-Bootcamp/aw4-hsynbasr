using Microsoft.AspNetCore.Mvc;
using SimApi.Base;
using SimApi.Operation;
using SimApi.Schema;

namespace SimApi.Service.Category
{
    [Route("simapi/v1/[controller]")]
    [ApiController]
    public class DapperCategoryController : ControllerBase
    {
        private readonly IDapperCategoryService categoryService;
        public DapperCategoryController(IDapperCategoryService CategoryService)
        {
            this.categoryService = CategoryService;
        }


        [HttpGet]
        public ApiResponse<List<CategoryResponse>> GetAll()
        {
            var accountList = categoryService.GetAll();
            return accountList;
        }

        [HttpGet("{id}")]
        public ApiResponse<CategoryResponse> GetById(int id)
        {
            var account = categoryService.GetById(id);
            return account;
        }

        [HttpPost]
        public ApiResponse Post([FromBody] CategoryRequest request)
        {
            return categoryService.Insert(request);
        }

        [HttpPut("{id}")]
        public ApiResponse Put(int id, [FromBody] CategoryRequest request)
        {
            return categoryService.Update(id, request);
        }

        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            return categoryService.Delete(id);
        }
    }
}
