namespace DefenceDB.EL.Models;

public class ProductImage
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public DefenseProduct Product { get; set; }
    
    public string ImagePath { get; set; }
    
    public bool IsMainImage { get; set; }
    
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
