using Crudify.Core;
using Microsoft.AspNetCore.Mvc;

namespace Crudify.AspNetCore
{
    public interface IRestfulCrudRepositoryController<TEntity, TEntityId>
        where TEntity : class, ICrudEntity<TEntityId>
        where TEntityId : struct
    {
        IActionResult Post([FromBody] TEntity entity);

        IActionResult Get(TEntityId id);

        IActionResult Put([FromBody] TEntity entity);

        IActionResult Delete(TEntityId id);
    }
}