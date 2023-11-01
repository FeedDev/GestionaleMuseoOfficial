namespace GestionaleMuseoOfficial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategorieVisitatori")]
    public partial class CategorieVisitatori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategorieVisitatori()
        {
            Biglietti = new HashSet<Biglietti>();
        }

        [Key]
        public int IdCategorieVisitatori { get; set; }

        [Required]
        [StringLength(50)]
        public string Descrizione { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Tipo Documento")]
        public string TipoDocumento { get; set; }

        [DisplayName("Sconto")]
        public int PercentualeSconto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Biglietti> Biglietti { get; set; }
    }
}
