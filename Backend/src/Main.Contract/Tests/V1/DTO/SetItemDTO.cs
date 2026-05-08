namespace Main.Contract.Tests.V1.DTO;

public class SetItemDTO
{
    public Guid Id { get; set; }
    
    public Guid SetId { get; set; }
    
    public string Term { get; set; }
    
    public string Description { get; set; }
}