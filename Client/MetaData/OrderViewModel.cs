using System.Text.Json.Serialization;
#nullable disable
namespace Client.MetaData
{

    public class OrderViewModel
    {
  
        public int Id { get; set; }

      
        public string UserId { get; set; }
   
        public decimal Total { get; set; }    
 
        public DateTime OrderDate { get; set; }
       
        public virtual ICollection<OrderItemViewModel> OrderItemViewModels { get; set; } = new List<OrderItemViewModel>();
       
    }
}
