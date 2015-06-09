using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        
        public Int32? YearOfBirth { get; set; }
        public Int32? YearOfStage { get; set; }

        public String Phone { get; set; }
        public String About { get; set; }
        public String AvatarImage { get; set; }
        public String AvatarImageExtension { get; set; }

        public String Car { get; set; }
        public String CarImage { get; set; }
        public String CarImageExtension { get; set; }
        public String CarClass { get; set;}

        public Boolean Sex { get; set; }

        //public ApplicationUser() { }

        private ICollection<Bid> _bids;
        public virtual ICollection<Bid> Bids
        {
            get { return _bids ?? (_bids = new Collection<Bid>()); }
            protected set { _bids = value; }
        }

        private ICollection<Trip> _trips;
        public virtual ICollection<Trip> Trips
        {
            get { return _trips ?? (_trips = new Collection<Trip>()); }
            protected set { _trips = value; }
        }

        private ICollection<Comment> _comments;
        public virtual ICollection<Comment> Comments
        {
            get { return _comments ?? (_comments = new Collection<Comment>()); }
            protected set { _comments = value; }
        }
    }
}
