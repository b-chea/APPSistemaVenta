﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.BDContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly BdventaContext _bdContext;

        public VentaRepository(BdventaContext bdContext) : base(bdContext)
        {
            _bdContext = bdContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            
            using (var transaction = _bdContext.Database.BeginTransaction()) 
            {
                try
                {
                    foreach(DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _bdContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _bdContext.Productos.Update(producto_encontrado);

                    }
                    await _bdContext.SaveChangesAsync();

                    NumeroDocumento correlativo = _bdContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;  
                    
                    _bdContext.NumeroDocumentos.Update(correlativo);
                    await _bdContext.SaveChangesAsync();
                    int CantidadDigitos = 5;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length  - CantidadDigitos, CantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;
                    await _bdContext.Venta.AddAsync(modelo);
                    await _bdContext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();//reestablece todo, si sale error(devuelve todo como estaba antes en la lógica de los productos)
                    throw;// devuelve el error
                }

                return ventaGenerada;

            }
        }
    }
}