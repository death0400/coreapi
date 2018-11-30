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
    [RepositoryControllerName]
    public class RepositoryController<T,TKey> : ControllerBase
        where T : class
    {
        private IRepository<T> repository;
        public RepositoryController(IRepository<T> repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.GetList());
        }
        [HttpGet("get/{key}")]
        public IActionResult Get(TKey key)
        {
            var entity = repository.Get(key);
            return Ok(entity);
        }
        [HttpGet("take/{amount}")]
        public IActionResult Get(int amount)
        {
            var entitys = repository.GetList(amount);
            return Ok(entitys);
        }
        [HttpPost("create")]

        public IActionResult Create(T t)
        {
            repository.Add(t);
            return Ok(t);
        }
        [HttpPut("update/{key}")]
        public IActionResult Put(T t)
        {
            repository.Update(t);
            return Ok(t);
        }
        [HttpDelete("delete/{key}")]
        public IActionResult Delete(TKey key)
        {
            repository.Delete(key);
            return Ok();
        }
    }
}
