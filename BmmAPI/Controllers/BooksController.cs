using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using BmmAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

[Route("api/BooksController")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IMongoCollection<Book> _collection;

    public BooksController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("BMM-mongo"); 
        _collection = database.GetCollection<Book>("books");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _collection.Find(_id => true).ToListAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(string id)
    {
        var book = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
        if (book == null) { 
            return NotFound();
        }

        return Ok(book);
    }


    [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> CreateBook(Book book)
    {
        await _collection.InsertOneAsync(book);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(string id, Book book)
    {
        await _collection.ReplaceOneAsync(filter: b => b.Id == id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        await _collection.DeleteOneAsync(b => b.Id == id);
        return NoContent();
    }
}
