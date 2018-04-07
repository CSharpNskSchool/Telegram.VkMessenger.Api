using System.ComponentModel;

namespace VkConnector.Model.Entities
{
    public class ApiResponse
    {
        [DefaultValue(true)]
        public bool Ok { get; set; }
        
        public string Description { get; set; }
    }
}