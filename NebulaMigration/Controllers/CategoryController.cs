using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NebulaMigration.ViewModels;

namespace NebulaMigration.Controllers
{
    using System;
    using Commands;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Models;

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
        /// Получение списка категорий.
        /// /// </summary>
        /// <returns>Список категорий блюд.</returns>
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
            return this.Ok(data
                .Select(this.mapper.Map<CategoryViewModel>)
                .ToArray());
        }

        /// <summary>
        /// Добавляет новую категорию.
        /// /// </summary>
        /// <returns>Результат выполнения операции.</returns>
        [HttpPost]
#if !DEBUG
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
#endif
        public async Task<ActionResult> Post(CreateCategoryCommand category)
        {
            var currentCategory = await this.db
                .Categories
                .FirstOrDefaultAsync(c => c.Name == category.Name)
                .ConfigureAwait(false);

            if (currentCategory != null)
            {
                return this.Conflict("Категория уже существует!");
            }

            var addedCategory = await this.db.AddAsync(this.mapper.Map<Category>(category)).ConfigureAwait(false);
            var result = await this.db.SaveChangesAsync().ConfigureAwait(false);
            return result > 0
                ? this.CreatedAtAction(nameof(this.Get), new { id = addedCategory.Entity.Id }, new { id = addedCategory.Entity.Id })
                : throw new InvalidOperationException("Не удалось добавить категорию!");
        }
    }
}