using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedisDemo3.Entity
{
    public class Product
    {
        [Key]  // Indica che ProductId è la chiave primaria della tabella
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Specifica che il valore viene generato automaticamente dal database quando un record viene inserito
        public int ProductId { get; set; }

        [StringLength(255)]  // Imposta una lunghezza massima per il nome del prodotto, in linea con le comuni limitazioni di MySQL
        public string? ProductName { get; set; }

        [StringLength(1000)]  // Imposta una lunghezza massima per la descrizione del prodotto
        public string? ProductDescription { get; set; }

        public int? Stock { get; set; }  // Quantità di stock disponibile per il prodotto
    }
}
