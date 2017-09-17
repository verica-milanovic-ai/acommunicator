//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACommunicator
{
    using System;
    using System.Collections.Generic;
    
    public partial class Option
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Option()
        {
            this.EndUsers = new HashSet<EndUser>();
            this.Options1 = new HashSet<Option>();
        }

        public Option(Option option)
        {
            this.Id = option.Id;
            this.Description = option.Description;
            this.Name = option.Name;
            this.FolderID = option.FolderID;
            this.Level = option.Level;
            this.ParentOptionId = option.ParentOptionId;
            this.IsDefault = option.IsDefault;

            this.EndUsers = option.EndUsers;
            this.Options1 = option.Options1;
            this.Option1 = option.Option1;
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string FolderID { get; set; }
        public int Level { get; set; }
        public Nullable<int> ParentOptionId { get; set; }
        public bool IsDefault { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EndUser> EndUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Option> Options1 { get; set; }
        public virtual Option Option1 { get; set; }
    }
}
