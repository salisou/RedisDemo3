using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisDemo3.Cache;
using RedisDemo3.DBContext;
using RedisDemo3.Entity;

namespace RedisDemo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;

        // Costruttore per iniettare le dipendenze
        public ProductController(DbContextClass dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            // Definisci una chiave per la cache dei prodotti
            var cacheKey = "products";

            // Tenta di recuperare i prodotti dalla cache
            var cacheData = _cacheService.GetData<IEnumerable<Product>>(cacheKey);
            if (cacheData != null)
            {
                return cacheData;
            }

            // Se non ci sono dati nella cache, recuperali dal database e aggiorna la cache
            var expirationTime = DateTimeOffset.Now.AddMinutes(5);
            var products = await _dbContext.Products.ToListAsync();

            _cacheService.SetData(cacheKey, products, expirationTime);
            return products;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProductAsync(Product product)
        {
            if (product == null)
            {
                return BadRequest("Il prodotto non può essere nullo.");
            }
            try
            {
                // Aggiunge il prodotto al database
                var obj = await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                // Invalida la cache dopo aver modificato i dati
                _cacheService.RemoveData("products");

                return CreatedAtAction(nameof(GetProductById), new { id = obj.Entity.ProductId }, obj.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Si è verificato un errore durante l'aggiunta del prodotto: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            // Recupera un prodotto specifico per ID
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound($"Prodotto con ID {id} non trovato.");
            }
            return product;
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<Product>> UpdateProductAsync(Product product)
        {
            if (product == null)
            {
                return BadRequest("Il prodotto non può essere nullo.");
            }

            try
            {
                // Verifica se il prodotto esiste nel database
                var existingProduct = await _dbContext.Products.FindAsync(product.ProductId);
                if (existingProduct == null)
                {
                    return NotFound($"Prodotto con ID {product.ProductId} non trovato.");
                }

                // Aggiorna i campi del prodotto esistente con i valori del nuovo prodotto
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductDescription = product.ProductDescription;
                existingProduct.Stock = product.Stock;

                // Aggiorna il prodotto nel DbContext e salva i cambiamenti
                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                // Invalida la cache per garantire che i dati siano aggiornati
                _cacheService.RemoveData("products");

                // Restituisce il prodotto aggiornato
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Si è verificato un errore durante l'aggiornamento del prodotto: {ex.Message}");
            }
        }

        [HttpDelete("deleteById/{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            try
            {
                // Cerca il prodotto nel database utilizzando l'ID fornito
                var productToDelete = await _dbContext.Products.FindAsync(id);
                if (productToDelete == null)
                {
                    return NotFound($"Prodotto con ID {id} non trovato.");
                }

                // Rimuove il prodotto dal DbContext
                _dbContext.Products.Remove(productToDelete);

                // Salva le modifiche nel database
                await _dbContext.SaveChangesAsync();

                // Invalida la cache per garantire che i dati siano aggiornati
                _cacheService.RemoveData("products");

                // Restituisce un codice di stato OK (200) per indicare che la rimozione è stata completata con successo
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Si è verificato un errore durante la rimozione del prodotto: {ex.Message}");
            }
        }
    }

}
