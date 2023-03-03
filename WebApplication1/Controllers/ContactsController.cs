using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
            //return View();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        /*[HttpGet]
        [Route("{name:string}")]
        public async Task<IActionResult> GetContactWith([FromRoute] string name)
        {
            var contact = await dbContext.Contacts.FindAsync(name);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }*/

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                id = Guid.NewGuid(),
                address = addContactRequest.address,
                email = addContactRequest.email,
                phone = addContactRequest.phone,
                name = addContactRequest.name,
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.name = updateContactRequest.name;
                contact.address = updateContactRequest.address;
                contact.email = updateContactRequest.email;
                contact.phone = updateContactRequest.phone;

                dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContacts()
        {
            var contact = dbContext.Contacts;
            dbContext.RemoveRange(contact);
            await dbContext.SaveChangesAsync();
            return Ok("deleted");
        }
    }
}
