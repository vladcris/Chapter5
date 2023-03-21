using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialOffers.Events;

public record EventFeedEvent(   long SequenceNumber, 
                                DateTimeOffset OccuredAt,
                                string Name, 
                                object Content);