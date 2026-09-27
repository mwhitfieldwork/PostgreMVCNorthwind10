using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class GetCategoryDto
    {
        
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public string CategoryName { get; set; } = null!;
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
