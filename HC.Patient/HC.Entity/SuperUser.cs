using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class SuperUser:MasterBaseEntity
        
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }         
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public string FirstName { get; set; } 
        public string MiddleName { get; set; } 
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsBlock { get; set; }
        public DateTime? BlockDateTime { get; set; } 
                    
    }
}
