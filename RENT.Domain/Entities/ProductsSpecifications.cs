namespace RENT.Domain.Entities
{
    public class ProductsSpecifications
    {
        public Guid ProductsSpecificationsId { get; set; }
        public string MaxLoad { get; set; }
        public string Weight { get; set; }
        public string LiftingHeight { get; set; }
        public string Capacity { get; set; }
        public string EnergySource { get; set; }
        public string Speed { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? ProductsId { get; set; }
        public virtual Products Products { get; set; }
    }
}
