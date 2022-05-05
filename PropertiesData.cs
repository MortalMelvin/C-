using System;

namespace properties_data
{
    class PropertiesData
    {
        public string PropertyId { get; set; }
        public string OwnerId { get; set; }
        public string CompanyId { get; set; }
        public string HandoverDate { get; set; }
        public string DocumentId { get; set; }
		public string DocumentFileName { get; set; }
		public string DocumentUploadedBy { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentMetadata { get; set; }
    }
}