namespace GestionaleMuseoOfficial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Visite")]
    public partial class Visite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Visite()
        {
            Biglietti = new HashSet<Biglietti>();
        }

        [Key]
        public int IdVisita { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Visita")]
        public string Titolo { get; set; }

        [Column(TypeName = "money")]
        public decimal TariffaOrdinaria { get; set; }

        //[Required]
        public string ImmagineLocandina { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataInizio { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataFine { get; set; }

        public int NumeroMaxBiglietti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Biglietti> Biglietti { get; set; }
    }
}
