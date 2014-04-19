using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliencube.TextEncodingConverter.ViewModels
{
    /// <summary>
    /// This represents the view model containing encoding information.
    /// </summary>
    public class EncodingInfoViewModel
    {
        /// <summary>
        /// Gets or sets the code page.
        /// </summary>
        public int CodePage { get; set; }

        /// <summary>
        /// Gets or sets the IANA encoding name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the human readable encoding name.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
