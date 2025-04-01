using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labs.API.Data;
using Labs.Domain.Entities;
using Labs.Domain.Models;

namespace Labs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetFoodsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PetFoodsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PetFoods
        [HttpGet]
        public async Task<ActionResult<ResponseData<ProductListModel<PetFood>>>> GetPetFoods(string? category, int pageNo = 1, int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ProductListModel<PetFood>>();

            // Фильтрация по категории загрузка данных категории
            var data = _context.PetFoods.Include(d => d.Category).Where(d => String.IsNullOrEmpty(category) || d.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages) pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ProductListModel<PetFood>()
            {
                Items = await data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // Поместить данные в объект результата
            result.Data = listData;

            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }

        // GET: api/PetFoods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PetFood>> GetPetFood(int id)
        {
            var petFood = await _context.PetFoods.FindAsync(id);

            if (petFood == null)
            {
                return NotFound();
            }

            return petFood;
        }

        // PUT: api/PetFoods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPetFood(int id, PetFood petFood)
        {
            if (id != petFood.Id)
            {
                return BadRequest();
            }

            _context.Entry(petFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetFoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PetFoods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PetFood>> PostPetFood(PetFood petFood)
        {
            _context.PetFoods.Add(petFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPetFood", new { id = petFood.Id }, petFood);
        }

        // DELETE: api/PetFoods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetFood(int id)
        {
            var petFood = await _context.PetFoods.FindAsync(id);
            if (petFood == null)
            {
                return NotFound();
            }

            _context.PetFoods.Remove(petFood);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetFoodExists(int id)
        {
            return _context.PetFoods.Any(e => e.Id == id);
        }
    }
}
