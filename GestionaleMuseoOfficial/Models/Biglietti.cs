namespace GestionaleMuseoOfficial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Biglietti")]
    public partial class Biglietti
    {
        [Key]
        [Display(Name = "Numero Biglietto")]
        public int IdBiglietto { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Data Validità")]
        public DateTime DataValidita { get; set; }

        public int IdVisita { get; set; }

        public int IdCategorieVisitatori { get; set; }

        [Column(TypeName = "money")]
        [DisplayName("Prezzo Totale")]
        public decimal PrezzoTotale { get; set; }

        public int IdCliente { get; set; }

        public virtual CategorieVisitatori CategorieVisitatori { get; set; }

        public virtual Clienti Clienti { get; set; }

        public virtual Visite Visite { get; set; }
    }
}
