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
    
    public partial class provisionservices
    {
        public int id_provisionservices { get; set; }
        public int quantity { get; set; }
        public int id_employee { get; set; }
        public int id_service { get; set; }
        public int id_contract { get; set; }
    
        public virtual contract contract { get; set; }
        public virtual employees employees { get; set; }
        public virtual services services { get; set; }
    }
}
