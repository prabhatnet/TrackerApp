using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerApplication
{
    class TrackerDataFoo2
    {
        public int CompanyId { get; set; } // Foo1: PartnerId, Foo2: CompanyId
        public string CompanyName { get; set; } // Foo1: PartnerName, Foo2: Company

        public List<Device> Devices { get; set; }

    }

    class Device
    {
        public int? TrackerId { get; set; } // Foo1: Id, Foo2: DeviceID
        public string TrackerName { get; set; } // Foo1: Model, Foo2: Name
        public DateTime? StartDate { get; set; } // Foo1: ShipmentStartDtm, Foo2: StartDateTime
              
        public List<SensorData> SensorDatas { get; set; }
    }


    class SensorData
    {
        public string SensorType { get; set; }
        public DateTime? CreatedDtm { get; set; }
        public decimal? Value { get; set; }

    }
}
