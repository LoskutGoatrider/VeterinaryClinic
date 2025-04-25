    using System.ComponentModel.DataAnnotations;
namespace VeterinaryClinic.Model
{
    using System;
    using System.Collections.Generic;
        public partial class client
        {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client()
        {
            this.animal = new HashSet<animal>();
            this.contract = new HashSet<contract>();
        }
                public int id_client { get; set; }
                public string lastname { get; set; }
                public string name { get; set; }
                public string middlename { get; set; }
                public string address { get; set; }
                public string phonenumber { get; set; }
                public string email { get; set; }
                public int id_gender { get; set; }
                public int id_user { get; set; }
    
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
                public virtual ICollection<animal> animal { get; set; }
                public virtual gender gender { get; set; }
                public virtual User User { get; set; }
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
                public virtual ICollection<contract> contract { get; set; }
        }
}
