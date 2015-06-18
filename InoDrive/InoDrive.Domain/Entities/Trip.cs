using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class Trip
    {
        public Trip() { }

        public Int32 TripId { get; set; }
        public String UserId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Int32 PeopleCount { get; set; }

        public Boolean IsAllowdedDeviation { get; set; }
        public Boolean IsAllowdedChildren { get; set; }
        public Boolean IsAllowdedPets { get; set; }
        public Boolean IsAllowdedMusic { get; set; }
        public Boolean IsAllowdedEat { get; set; }
        public Boolean IsAllowdedDrink { get; set; }
        public Boolean IsAllowdedSmoke { get; set; }
        public Boolean IsDeleted { get; set; }

        public String About { get; set; }

        public String Car { get; set; }
        public String CarImage { get; set; }
        public String CarImageExtension { get; set; }
        public String CarClass { get; set; }

        [Required, ForeignKey("OriginPlace")]
        public String OriginPlaceId { get; set; }

        [Required, ForeignKey("DestinationPlace")]
        public String DestinationPlaceId { get; set; }

        [Column(TypeName = "Money")]
        public decimal? Pay { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Place OriginPlace { get; set; }
        public virtual Place DestinationPlace { get; set; }

        private ICollection<Comment> _comments;
        public virtual ICollection<Comment> Commnents
        {
            get { return _comments ?? (_comments = new Collection<Comment>()); }
            protected set { _comments = value; }
        }

        private ICollection<WayPoint> _wayPoint;
        public virtual ICollection<WayPoint> WayPoints
        {
            get { return _wayPoint ?? (_wayPoint = new Collection<WayPoint>()); }
            set { _wayPoint = value; }
        }

        private ICollection<Bid> _bids;
        public virtual ICollection<Bid> Bids
        {
            get { return _bids ?? (_bids = new Collection<Bid>()); }
            protected set { _bids = value; }
        }
    }
}
