using System.Collections.Generic;
using System.Linq;

namespace CovertTweeter.Core
{
    public class TwitterResponse
    {
        public bool HasErrors { get { return Errors != null && Errors.Any(); } }

        public List<TwitterError> Errors { get; set; }
    }
}