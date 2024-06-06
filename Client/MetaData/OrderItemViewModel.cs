using System.Text.Json.Serialization;
#nullable disable
namespace Client.MetaData
{

    public class OrderItemViewModel
    {
   
        public int Id { get; set; }
      
        public int? OrderId { get; set; }
       
      
        public int? ProductId { get; set; }
    
        public int Quantity { get; set; }
    
        public virtual OrderViewModel Order { get; set; }
        
        public virtual ProductViewModel Product { get; set; }
    }
}
