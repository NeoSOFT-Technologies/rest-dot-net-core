
using System;
using System.Collections.Generic;
using System.Text;

namespace GloboTicket.TicketManagement.Domain.Common
{
    public abstract class Document : IDocument
    {
        public Guid Id { get; set; }
    }
    }
