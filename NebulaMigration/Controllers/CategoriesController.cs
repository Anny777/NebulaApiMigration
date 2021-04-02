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
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Получение списка категорий.
        /// /// </summary>
        /// <returns>Список категорий блюд.</returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Get()
        {
            var data = await this.db
                .Categories
                .OrderBy(b => b.Name)
                .Select(c => this.mapper.Map<CategoryViewModel>(c))
                .ToListAsync()
                .ConfigureAwait(false);
            
            return this.Ok(data);
        }

        /// <summary>
        /// Добавляет новую категорию.
        /// /// </summary>
        /// <returns>Результат выполнения операции.</returns>
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post(CreateCategoryCommand category)
        {
            var currentCategory = await this.db
                .Categories
                .FirstOrDefaultAsync(c => c.Name.ToUpperInvariant() == category.Name.ToUpperInvariant())
                .ConfigureAwait(false);

            if (currentCategory != null)
            {
                return this.Conflict($"Категория {category.Name} уже существует!");
            }

            var addedCategory = await this.db.AddAsync(this.mapper.Map<Category>(category)).ConfigureAwait(false);
            var result = await this.db.SaveChangesAsync().ConfigureAwait(false);
            return result > 0
                ? this.CreatedAtAction(nameof(this.Get), new { id = addedCategory.Entity.Id },
                    new { id = addedCategory.Entity.Id })
                : throw new InvalidOperationException("Не удалось добавить категорию!");
        }
    }
}