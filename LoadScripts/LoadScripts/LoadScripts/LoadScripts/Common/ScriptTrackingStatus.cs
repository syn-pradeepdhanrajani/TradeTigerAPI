using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadScripts.Common
{
    public enum ScriptTrackingStatus
    {
        None = 1,
        NearPreviousLow,
        NearPreviousHigh,
        CrossedPreviousLow,
        CrossedPreviousHigh,
        CrossedPreviousHighAndLow,
        NearPreviousLowClosingAboveOpenPrice,
        NearPreviousLowClosingBelowOpenPrice,
        OpenDayLowSamePrice,
        OpenDayHighSamePrice,
        NearPreviousHighClosingAboveOpenPrice,
        NearPreviousHighClosingBelowOpenPrice,
    }
}
