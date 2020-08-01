using System;

namespace RedLeg.Forms
{
    public class EquipmentReceipt
    {
        public String Unit { get; set; }

        public String ReceiptNumber { get; set; }

        public String StockNumber { get; set; }

        public String SerialNumber { get; set; }

        public String Description { get; set; }

        public String From { get; set; }

        public String Name { get; set; }

        public Rank? Grade { get; set; }
    }
}