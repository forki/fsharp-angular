﻿using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using fsharp_angular_web.Model;
using Microsoft.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace fsharp_angular_web.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoDbContext _db;

        public TodoController(TodoDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            if (!(await _db.Items.AnyAsync()))
            {
                _db.Items.Add(new TodoItem() { Title = "try Angular" });
                _db.Items.Add(new TodoItem() { Title = "use React" });

                await _db.SaveChangesAsync();
            }

            return _db.Items.OrderBy(i => i.Id);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(Int32 id)
        {
            var item = await _db.Items.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoItem item)
        {
            if (item != null)
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = item.Id },
                    item
                );
            }

            return HttpBadRequest();
        }
    }
}