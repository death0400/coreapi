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
    [Route("api/ReadOnly/[controller]")]
    [ReadRepositoryControllerNameAttribute]
    public class ReadRepositoryController<T,TKey> : ControllerBase
        where T : class
    {
        private IReadRepository<T> repository;
        public ReadRepositoryController(IReadRepository<T> repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.GetList());
        }
        [HttpGet("{key}")]
        public IActionResult Get(TKey key)
        {
            var entity = repository.Get(key);
            return Ok(entity);
        }
    }
}
