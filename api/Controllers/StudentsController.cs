using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
       public StudentsController(AppDbContext context)
       {
        _context = context;
       }
       
       [HttpGet]

        
        public async Task<IEnumerable<Student>> GetStudents()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();
            return students;
        }

        [HttpPost]

        public async Task<IActionResult> Create(Student student)   

        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.AddAsync(student);
            
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    
    [HttpDelete("{id:int}")]

    public async Task<IActionResult> Delete(int id)
    {
       var student = await _context.Students.FindAsync(id);
       if(student == null) 
       {
        return NotFound();
       }

       _context.Remove(student);

       var result = await _context.SaveChangesAsync();

        if(result > 0)
        {
            return Ok("Student was deleted");
        }
        return BadRequest("Unable to delete student");

    }

    //single student

    [HttpGet("{id:int}")]

    public async Task<ActionResult<Student>> GetStudents(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound("who dis");
        }
        return Ok(student);
    }


//update/put
    [HttpPut("{id:int}")]

    public async Task<IActionResult> EditStudent(int id, Student student)
    {
        var studentFromDb = await _context.Students.FindAsync(id);
         if (studentFromDb == null)
         {
            return BadRequest("Student not found");
         }
         studentFromDb.Name = student.Name;
         studentFromDb.Email = student.Email;
         studentFromDb.PhoneNumber = student.PhoneNumber;
         studentFromDb.Address = student.Address;

         var result = await _context.SaveChangesAsync();

         if(result > 0)
         {
            return Ok("Student was edited");
         }
         return BadRequest("Unable to update data");
    }
    
    }
}