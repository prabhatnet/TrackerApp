using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerApplication
{
    class TrackerDataFoo1
    {
        public int CompanyId { get; set; } // Foo1: PartnerId, Foo2: CompanyId
        public string CompanyName { get; set; } // Foo1: PartnerName, Foo2: Company

        public List<Tracker> Trackers { get; set; }
    }

    class Tracker
    {
        public int? TrackerId { get; set; } // Foo1: Id, Foo2: DeviceID
        public string TrackerName { get; set; } // Foo1: Model, Foo2: Name
        public DateTime? StartDate { get; set; } // Foo1: ShipmentStartDtm, Foo2: StartDateTime

        public List<Sensor> Sensors { get; set; }
        
    }

    class Sensor
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<Crumb> Crumbs { get; set; }
    }

    class Crumb
    {
        public DateTime? CreatedDtm { get; set; }
        public decimal? Value { get; set; }
    }
}
