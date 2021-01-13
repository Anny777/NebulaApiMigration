using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NebulaMigration.ViewModels;

namespace NebulaMigration.Controllers
{
    /// <summary>
    /// CategoryController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationContext db;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="db">db.</param>
        public CategoryController(IMapper mapper, ApplicationContext db)
        {
            this.db = db ?? throw new System.ArgumentNullException(nameof(db));
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Получение списка блюд
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
#if !DEBUG
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
#endif
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Get()
        {
            var data = await this.db
                             .Categories
                             .OrderBy(b => b.Name)
                             .ToListAsync()
                             .ConfigureAwait(false);
            return this.Ok(data.Select(this.mapper.Map<CategoryViewModel>));
        }
    }
}