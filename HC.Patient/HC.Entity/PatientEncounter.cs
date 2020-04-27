using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounter : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public Nullable<int> PatientID { get; set; }

        [ForeignKey("PatientAppointment")]
        public int? PatientAppointmentId { get; set; }

        [Required]
        [RequiredDate]
        public DateTime DateOfService { get; set; }

        public DateTime StartDateTime { get; set; }

        [RequiredDate]        
        public DateTime EndDateTime { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("Staff")]
        public int? StaffID { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string NonBillableNotes { get; set; }
        public bool? IsBillableEncounter { get; set; }

        [ForeignKey("PatientAddress")]
        public int? PatientAddressID { get; set; }

        [ForeignKey("Location1")]
        public int? OfficeAddressID { get; set; }

        [ForeignKey("Location")]
        public int? ServiceLocationID { get; set; }
        public int Status { get; set; }

        [ForeignKey("MasterNoteType")]
        public int? NotetypeId { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string CustomAddress { get; set; }

        [ForeignKey("MasterPatientLocation1")]
        public int? CustomAddressID { get; set; }

        public virtual MasterPatientLocation MasterPatientLocation1 { get; set; }

        //Foreign key's tables        
        public virtual Staffs Staff { get; set; }        
        public virtual Patients Patient { get; set; }

        [ForeignKey("AppointmentType")]
        public int? AppointmentTypeId { get; set; }

        public virtual AppointmentType AppointmentType { get; set; }

        [Obsolete]
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }                
        public virtual PatientAppointment PatientAppointment { get; set; }        
        public virtual MasterNoteType MasterNoteType { get; set; }        
        public virtual Organization Organization { get; set; }
        public virtual PatientAddress PatientAddress { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
    }
}