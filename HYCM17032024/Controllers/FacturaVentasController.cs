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
                        Problem("Entity set 'Practica20240305DBContext.FacturaVentas'  is null.");
        }

        // GET: FacturaVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas
                .Include(s => s.DetFacturaVenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVenta == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(facturaVenta);
        }

        // GET: FacturaVentas/Create
        public IActionResult Create()
        {
            var facturaVenta = new FacturaVenta();
            facturaVenta.FechaVenta = DateTime.Now;
            facturaVenta.TotalVenta = 0;
            facturaVenta.DetFacturaVenta = new List<DetFacturaVenta>();
            facturaVenta.DetFacturaVenta.Add(new DetFacturaVenta
            {
                Cantidad = 1
            });
            ViewBag.Accion = "Create";
            return View(facturaVenta);
        }

        // POST: FacturaVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVenta facturaVenta)
        {
            facturaVenta.TotalVenta = facturaVenta.DetFacturaVenta.Sum(s => s.Cantidad * s.PrecioUnitario);
            _context.Add(facturaVenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            // return View(facturaVenta);
        }
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVenta facturaVenta, string accion)
        {
            facturaVenta.DetFacturaVenta.Add(new DetFacturaVenta { Cantidad = 1 });
            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }
        public ActionResult EliminarDetalles([Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVenta facturaVenta,
           int index, string accion)
        {
            var det = facturaVenta.DetFacturaVenta[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                facturaVenta.DetFacturaVenta.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, facturaVenta);
        }
        // GET: FacturaVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas
                .Include(s => s.DetFacturaVenta)
                .FirstAsync(s => s.Id == id);
            if (facturaVenta == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(facturaVenta);
        }

        // POST: FacturaVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaVenta,Correlativo,Cliente,TotalVenta,DetFacturaVenta")] FacturaVenta facturaVenta)
        {
            if (id != facturaVenta.Id)
            {
                return NotFound();
            }

            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.FacturaVentas
                        .Include(s => s.DetFacturaVenta)
                        .FirstAsync(s => s.Id == facturaVenta.Id);
                facturaUpdate.Correlativo = facturaVenta.Correlativo;
                facturaUpdate.TotalVenta = facturaVenta.DetFacturaVenta.Where(s => s.Id > -1).Sum(s => s.PrecioUnitario * s.Cantidad);
                facturaUpdate.Cliente = facturaVenta.Cliente;
                facturaUpdate.FechaVenta = facturaVenta.FechaVenta;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = facturaVenta.DetFacturaVenta.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.DetFacturaVenta.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = facturaVenta.DetFacturaVenta.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.DetFacturaVenta.FirstOrDefault(s => s.Id == d.Id);
                    det.Cantidad = d.Cantidad;
                    det.PrecioUnitario = d.PrecioUnitario;
                    det.Producto = d.Producto;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = facturaVenta.DetFacturaVenta.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.DetFacturaVenta.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);

                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(facturaUpdate);
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

        // GET: FacturaVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacturaVentas == null)
            {
                return NotFound();
            }

            var facturaVenta = await _context.FacturaVentas
                .Include(s => s.DetFacturaVenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facturaVenta == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(facturaVenta);
        }

        // POST: FacturaVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacturaVentas == null)
            {
                return Problem("Entity set 'Practica20240305DBContext.FacturaVentas'  is null.");
            }
            var facturaVenta = await _context.FacturaVentas
                .FindAsync(id);
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
