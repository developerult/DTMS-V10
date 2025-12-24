using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vBookLoadBoardDropped
    {
        public Models.vBookLoadBoard draggedObject { get; set; }
        public Models.vBookLoadBoard droppedObject { get; set; }
        public bool isDraggingUp { get; set; }

    }
}