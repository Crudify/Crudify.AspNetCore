using Crudify.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crudify.AspNetCore
{
    [Route("api/crud/[controller]/{id?}")]
    [GenericRestfulCrudRepositoryControllerNamingConvention]
    public class GenericRestfulCrudRepositoryController<TEntity, TEntityId> : Controller,

        IRestfulCrudRepositoryController<TEntity, TEntityId>

        where TEntity : class, ICrudEntity<TEntityId>
        where TEntityId : struct
    {
        public ICrudRepository<TEntity, TEntityId> CrudRepository { get; }

        public GenericRestfulCrudRepositoryController(
            ICrudRepository<TEntity, TEntityId> crudRepository)
        {
            CrudRepository = crudRepository;
        }

        [HttpGet]
        public IActionResult Get(TEntityId id)
        {
            var result = CrudRepository.Read(id);

            if (result == null)
                return NotFound(id);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]TEntity entity)
        {
            var createdEntityId = CrudRepository.Create(entity);

            var location = new StringBuilder();
            var requestPathParts = Request.Path.Value.Split('/');

            for (int i = 0; i < requestPathParts.Length - 1; i++)
            {
                location.Append(requestPathParts[i] + '/');
            }
            location.Append(createdEntityId);

            return Created(location.ToString(), CrudRepository.Read(createdEntityId));
        }
        [HttpPut]
        public IActionResult Put([FromBody]TEntity entity)
        {
            var recordToUpdate = CrudRepository.Read(entity.Id);
            if (recordToUpdate == null)
            {
                return Post(entity);
            }
            else
            {
                CrudRepository.Update(entity);
            }

            return Ok(entity);
        }
        [HttpDelete]
        public IActionResult Delete(TEntityId id)
        {
            var recordToDelete = CrudRepository.Read(id);
            if (recordToDelete == null)
                return NotFound(id);

            CrudRepository.Delete(id);

            return Ok();
        }
    }
}
