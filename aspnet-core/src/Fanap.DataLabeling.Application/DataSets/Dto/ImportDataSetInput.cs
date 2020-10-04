using Fanap.DataLabeling.Datasets;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.DataSets
{
    [Serializable]
    public class ImportInput
    {
        [Required]
        public string FolderPath { get; set; }
        [Required]
        public Guid DataSetId { get; set; }
    }
}