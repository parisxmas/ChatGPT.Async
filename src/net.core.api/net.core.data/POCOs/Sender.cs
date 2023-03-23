using System.ComponentModel.DataAnnotations.Schema;
using net.core.data.POCOs.Base;

namespace net.core.data.POCOs;


public class Sender:POCOEntity<Guid>
{
    public string Name { get; set; }
}