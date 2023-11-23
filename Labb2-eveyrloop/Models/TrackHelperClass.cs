using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_eveyrloop.Models
{
    public partial class Track
    {
        public string FormattedTime
        {
            get
            {
                TimeSpan timeSpan = TimeSpan.FromMilliseconds(this.Milliseconds);
                return $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2} Minutes";
            }
        }
    }
}
