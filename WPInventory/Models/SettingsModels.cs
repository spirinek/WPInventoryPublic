using System.ComponentModel.DataAnnotations;

namespace WPInventory.Models
{
    public class UpdateScopeModel : BaseScopeModel
    {
    }

    public class CreateScopeModel : BaseScopeModel
    {
    }

    public class BaseScopeModel
    {
        [Required]
        [MaxLength(100)]
        public string ScopePath { get; set; }
        [Required]
        public bool IsEnabled { get; set; }
    }
}
