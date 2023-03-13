using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Practical4.Models
{
    public class Response
    {
        public string Status { get; set; }
        public dynamic Data { get; set; }
    }
}
