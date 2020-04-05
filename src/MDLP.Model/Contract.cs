using System;

namespace MDLP.Gate.Contract
{
    public class GateMessage
    {
        public string message_uuid { get; set; }
        public string message_type { get; set; }
        public MessageBody message_body { get; set; }
    }
    public class MessageBody
    {
        public string source_company_id { get; set; }
        public string target_company_id { get; set; }
        public string source_warehouse_id { get; set; }
        public string target_warehouse_id { get; set; }
        public string doc_type { get; set; }
        public DateTime doc_date { get; set; }
        public string doc_number { get; set; }
        public Sscc[] sscc { get; set; }
        public Sgtin[] sgtin { get; set; }
    }
    public class Sscc
    {
        public string sscc { get; set; }
        public float price { get; set; }
        public float vat { get; set; }
    }
    public class Sgtin
    {
        public string sgtin { get; set; }
        public int price { get; set; }
        public int vat { get; set; }
    }
}