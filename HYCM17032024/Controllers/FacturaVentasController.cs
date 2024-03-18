using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HYCM17032024.Models;

namespace HYCM17032024.Controllers
{
    public class FacturaVentasController : Controller
    {
        private readonly HYCM17032024Context _context;

        public FacturaVentasController(HYCM17032024Context context)
        {
            _context = context;
        }

        // GET: FacturaVentas
        public async Task<IActionResult> Index()
        {
              return _context.FacturaVentas != null ? 
                          View(await _context.FacturaVentas.ToListAsync()) :
                          Problem("Entity set 'HYCM17032024Context.FacturaVentas'  is null.");
        }

        // GET: FacturaVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVenta == null)
            {
                return NotFound();
            }

            return View(facturaVenta);
        }

        // GET: FacturaVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FacturaVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta")] FacturaVenta facturaVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facturaVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facturaVenta);
        }

        // GET: FacturaVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas.FindAsync(id);
            if (facturaVenta == null)
            {
                return NotFound();
            }
            return View(facturaVenta);
        }

        // POST: FacturaVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta")] FacturaVenta facturaVenta)
        {
            if (id != facturaVenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facturaVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaVentaExists(facturaVenta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(facturaVenta);
        }

        // GET: FacturaVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVenta == null)
            {
                return NotFound();
            }

            return View(facturaVenta);
        }

        // POST: FacturaVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacturaVentas == null)
            {
                return Problem("Entity set 'HYCM17032024Context.FacturaVentas'  is null.");
            }
            var facturaVenta = await _context.FacturaVentas.FindAsync(id);
            if (facturaVenta != null)
            {
                _context.FacturaVentas.Remove(facturaVenta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaVentaExists(int id)
        {
          return (_context.FacturaVentas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
