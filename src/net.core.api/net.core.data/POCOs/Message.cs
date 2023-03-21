using System.ComponentModel.DataAnnotations.Schema;
using net.core.data.POCOs.Base;

namespace net.core.data.POCOs;

public class Message:POCOEntity<Guid>
{
    public string Text { get; set; }
    
    public Guid SenderId { get; set; }
}