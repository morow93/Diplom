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
    public class User
    {
        public String Id { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? DateOfStage { get; set; }

        public String Phone { get; set; }
        public String About { get; set; }
        public String AvatarImage { get; set; }
        public String AvatarImageExtension { get; set; }

        public String Car { get; set; }
        public String CarImage { get; set; }
        public String CarImageExtension { get; set; }
        public String CarClass { get; set;}

        public User() { }

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

        private ICollection<Like> _likes;
        public virtual ICollection<Like> Likes
        {
            get { return _likes ?? (_likes = new Collection<Like>()); }
            protected set { _likes = value; }
        }
    }
}
