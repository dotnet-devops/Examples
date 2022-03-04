using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityAccrual.Shared.Definitions
{
    public enum CloseCondition
    {
        None,
        Cancelled,
        Error,
        InvalidUser,
        Success
    }

    public enum Partition
    {
        None,
        Adjustment,
        Budget,
        Utility
    }

    public enum RevisionStatus
    {
        New,
        Pending,
        Approved,
        Rejected,
        Superceded,
        Original
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
