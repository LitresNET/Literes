using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public enum RequestType
{
    Create,
    Update, 
    Delete,
}