using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimApi.Data;
using SimApi.Data.Context;
using SimApi.Schema;

namespace SimApi.Service.Controllers;

[Route("simapi/v1/[controller]")]
[ApiController]
[NonController]
public class Category1Controller : ControllerBase
{
    private SimEfDbContext context;
    private IMapper mapper;
    public Category1Controller(SimEfDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }


    [HttpGet]
    public List<CategoryResponse> GetAll()
    {
        var list = context.Set<Data.Category>().ToList();
        var mapped = mapper.Map<List<CategoryResponse>>(list);
        return mapped;
    }

    [HttpGet("{id}")]
    public CategoryResponse GetById(int id)
    {
        var row = context.Set<Data.Category>().FirstOrDefault(x => x.Id == id);
        var mapped = mapper.Map<CategoryResponse>(row);
        return mapped;
    }

    [HttpPost]
    public CategoryResponse Post([FromBody] CategoryRequest request)
    {
        var entity = mapper.Map<Data.Category>(request); 
        context.Set<Data.Category>().Add(entity);
        context.SaveChanges();

        var mapped = mapper.Map<Data.Category, CategoryResponse>(entity);
        return mapped;
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] CategoryRequest request)
    {
        request.Id = id;
        var entity = mapper.Map<Data.Category>(request);
        context.Set<Data.Category>().Update(entity);
        context.SaveChanges();
    }


    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var row = context.Set<Data.Category>().Where(x => x.Id == id).FirstOrDefault();
        context.Set<Data.Category>().Remove(row);
        context.SaveChanges();
    }

}
