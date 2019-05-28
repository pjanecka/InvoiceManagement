using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceManagement.Auth;
using InvoiceManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Controllers
{
    [BasicAuth]
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public ApiController(InvoiceContext context)
        {
            _context = context;
        }

        // GET: api/unpaid
        [HttpGet("unpaid")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetListOfUnpaidInvoices()
        {
            var invoices = _context.Invoices.Include(x => x.Items).ToAsyncEnumerable();
            if (invoices == null) return NotFound();
            var result = await invoices.Where(x => !x.Paid).ToList();
            return result;
        }

        // GET: api/pay/5
        [HttpGet("pay/{id}")]
        public async Task<IActionResult> PayInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null) return NotFound();
            if (invoice.Paid) return BadRequest();
            invoice.Paid = true;
            _context.Update(invoice);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PATCH: api/pay/5
        [HttpPatch("edit/{id}")]
        public async Task<IActionResult> EditInvoice(int id, [FromBody]JsonPatchDocument<Invoice> invoicePatch)
        {
            var invoice = _context.Invoices.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
            if (invoice == null) return NotFound();
            if (ModelState.IsValid)
            {
                invoicePatch.ApplyTo(invoice);
                _context.Update(invoice);
                await _context.SaveChangesAsync();
                return Ok(invoice);
            }
            return BadRequest();
        }
    }
}