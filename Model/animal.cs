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
    
    public partial class animal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public animal()
        {
            this.contract = new HashSet<contract>();
        }
    
        public int id_animal { get; set; }
        public string nickname { get; set; }
        public string color { get; set; }
        public Nullable<int> id_gender { get; set; }
        public Nullable<System.DateTime> datebirth { get; set; }
        public int id_typeanimal { get; set; }
        public int id_client { get; set; }
    
        public virtual client client { get; set; }
        public virtual gender gender { get; set; }
        public virtual typeanimal typeanimal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contract> contract { get; set; }
    }
}
