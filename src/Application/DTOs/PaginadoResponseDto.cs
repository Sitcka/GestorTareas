using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.DTOs
{
    public class PaginadoResponseDto<T>
    {
        public IEnumerable<T> Elementos { get; set; } = Enumerable.Empty<T>();
        public int TotalElementos { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanoPagina { get; set; }

        public int TotalPaginas =>
            TamanoPagina <= 0 ? 0 : (int)Math.Ceiling((double)TotalElementos / TamanoPagina);
    }
}