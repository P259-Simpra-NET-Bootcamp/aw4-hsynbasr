using AutoMapper;
using Serilog;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Context;
using SimApi.Data.Repository;
using SimApi.Data.Uow;
using SimApi.Schema;

namespace SimApi.Operation;

public class DapperCategoryService :IDapperCategoryService /*BaseService<Category, CategoryRequest, CategoryResponse>, IDapperCategoryService*/
{
    private readonly IDapperRepository<Category> dapperRepository;
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    public DapperCategoryService(IDapperRepository<Category> dapperRepository, IMapper mapper)
    {
        this.dapperRepository = dapperRepository;
        this.mapper = mapper;
    }

    public ApiResponse Delete(int Id)
    {
        try
        {
            dapperRepository.DeleteById(Id);
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Delete Exception");
            return new ApiResponse(ex.Message);
        }
    }
        
    

    public ApiResponse<List<CategoryResponse>> GetAll()
    {
        try
        {
            var entities = dapperRepository.GetAll();
            var mapped = mapper.Map<List<Category>, List<CategoryResponse>>(entities);
            return new ApiResponse<List<CategoryResponse>>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAll Exception");
            return new ApiResponse<List<CategoryResponse>>(ex.Message);
        }
       

    }

    public ApiResponse<CategoryResponse> GetById(int id)
    {
        try
        {
            var entity = dapperRepository.GetById(id);
            var mapped = mapper.Map<Category,CategoryResponse>(entity);
            return new ApiResponse<CategoryResponse>(mapped);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetById Exception");
            return new ApiResponse<CategoryResponse>(ex.Message);
        }
    }

    public ApiResponse Insert(CategoryRequest request)
    {
        try
        {
            var mapped = mapper.Map<CategoryRequest, Category>(request);
            dapperRepository.Insert(mapped);
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetById Exception");
            return new ApiResponse(ex.Message);
        }
    }

    public ApiResponse Update(int Id, CategoryRequest request)
    {
        try
        {
            var mapped = mapper.Map<CategoryRequest, Category>(request);
            var entity = dapperRepository.GetById(Id);
            if (entity is null)
            {
                Log.Warning("Record not found for Id " + Id);
                return new ApiResponse("Record not found");
            }

            entity.Id = Id;
            entity.UpdatedAt = DateTime.UtcNow;

            dapperRepository.Update(entity);
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Update Exception");
            return new ApiResponse(ex.Message);
        }
    }
}
