using System;
using System.Collections.Generic;

namespace HYCM17032024.Models
{
    public partial class FacturaVenta
    {
        public FacturaVenta()
        {
            DetFacturaVenta = new HashSet<DetFacturaVenta>();
        }

        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Correlativo { get; set; } = null!;
        public string Cliente { get; set; } = null!;
        public decimal TotalVenta { get; set; }

        public virtual ICollection<DetFacturaVenta> DetFacturaVenta { get; set; }
    }
}
