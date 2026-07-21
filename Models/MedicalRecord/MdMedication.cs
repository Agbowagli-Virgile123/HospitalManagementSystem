using HospitalManagementSystem.Components.Pages;

namespace HospitalManagementSystem.Models.MedicalRecord
{
    public class MdPostMedication
    {
        public string? MedicationName {get; set;}
        public string? GenericName {get; set;}
        public string? BrandName {get; set;}
        public string? Category { get; set; }
        public string? Manufacturer {get;set;}
        public string? DosageStrength {get;set;}
        public double Unit {get;set;}
        public double Quantity {get;set;}
        public double MinimumStock {get;set;}
        public string? BatchNumber {get;set;}
        public DateTime ExpiryDate { get; set; } = DateTime.Now;
        public string? StorageLocation {get;set;}
        public double CostPrice {get;set;}
        public double SellingPrice {get;set;}
        public string? Description {get;set;}
        public string? StorageInstructions {get;set;}
        public string? SideEffects { get; set; }

    }

    public class MdGetMedication : MdPostMedication
    {
        public string? Id {get; set;}
    }
}
