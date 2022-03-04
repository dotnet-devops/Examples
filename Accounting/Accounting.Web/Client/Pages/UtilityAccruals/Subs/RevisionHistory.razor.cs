using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Client.Pages.UtilityAccruals.Subs
{
    public partial class RevisionHistory
    {
        [Parameter]
        public IEnumerable<AdjustmentRevision> AdjustmentRevisions { get; set; }
    }
}
