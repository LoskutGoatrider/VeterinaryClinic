//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VeterinaryClinic.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class employees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employees()
        {
            this.provisionservices = new HashSet<provisionservices>();
        }
    
        public int id_employee { get; set; }

        public string lastname { get; set; }
        public string name { get; set; }
        public string middlename { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public System.DateTime datebirth { get; set; }
        public string passportseries { get; set; }
        public string passportnumber { get; set; }
        public decimal salary { get; set; }
        public int id_gender { get; set; }
        public int id_user { get; set; }
    
        public virtual gender gender { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<provisionservices> provisionservices { get; set; }
    }
}
