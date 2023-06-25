using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MaterializedViews.API.Models
{
    [Table("EventType", Schema = "Source")]
    public class EventType
    {
        [Key, NotNull]
        public int EventTypeID { get; set; }
        [MaxLength(20), NotNull]
        public string EventTypeCode { get; set; }
        [MaxLength(100), NotNull]
        public string EventTypeDesc { get; set; }
    }
}
