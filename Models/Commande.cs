using System;
using System.Collections.Generic;

namespace projett.Models
{
    public partial class Commande
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string? Article { get; set; }
        public string? Qte { get; set; }
        public string? Prix { get; set; }
        public string? Status { get; set; } = "En attente";


        public virtual User? User { get; set; }
    }
}
