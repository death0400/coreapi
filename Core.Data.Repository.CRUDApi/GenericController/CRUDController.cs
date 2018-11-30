using Core.Data;
using Core.Data.Repository.CRUDApi.GenericController.Base;
using Core.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository.CRUDApi.GenericController
{

    
    [Route("api/[controller]")]
    [ReadRepositoryControllerName]
    public class CRUDController<TContext, T> : ControllerBase
        where T : class
        where TContext: class

    {
        private IUnitOfWork unitOfWork;
        private IRepository<T> repository;
        public CRUDController(IUnitOfWork<TContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            repository = unitOfWork.GetRepository<T>();
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.GetList());
        }
        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var entity = repository.Get(key);
            return Ok(entity);
        }
        [HttpPost]

        public IActionResult Create(T t)// 視情況添加[FromBody] attr
        {
            repository.Add(t);
            return Ok(t);
        }
        [HttpPut("{key}")]
        public IActionResult Put(T t)
        {
            repository.Update(t);
            return Ok(t);
        }
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            repository.Delete(key);
            return Ok();
        }

        public override OkResult Ok()
        {
            unitOfWork.SaveChanges();
            return base.Ok();
        }
        public override OkObjectResult Ok(object value)
        {
            unitOfWork.SaveChanges();
            return base.Ok(value);
        }
    }
}
