using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManagement.Models;
using System.Collections.Generic;

namespace InvoiceManagement.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly InvoiceContext _context;

        public InvoicesController(InvoiceContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoices.Include(x => x.Items).ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,Company,BillTo,Items,Paid")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (invoice.Items == null) invoice.Items = new List<Item>();
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,Company,BillTo,Items,Paid")] Invoice invoice)
        {
            if (id != invoice.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _context.Invoices.FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)  return NotFound();
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
            _context.Items.RemoveRange(invoice.Items);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id) => _context.Invoices.Any(e => e.Id == id);

        [HttpGet]
        public async Task<IActionResult> AddItem(int invoiceId)
        {
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == invoiceId);
            try
            {
                invoice.Items.Add(new Item());
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Edit), new { id = invoiceId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItem(int invoiceId, int itemId)
        {
            var invoice = await _context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == invoiceId);
            try
            {
                var item = invoice.Items.FirstOrDefault(x => x.Id == itemId);
                invoice.Items.Remove(item);
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Edit), new { id = invoiceId });
        }
    }
}
