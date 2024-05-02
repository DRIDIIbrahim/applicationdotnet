using System;
using System.Collections.Generic;

namespace projett.Models
{
    public partial class User
    {
        public User()
        {
            Commandes = new HashSet<Commande>();
        }

        public int UserId { get; set; }
        public string? Nom { get; set; }
        public string? MotsDePasse { get; set; }

        public virtual ICollection<Commande> Commandes { get; set; }
    }
}
