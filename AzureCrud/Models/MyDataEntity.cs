﻿using Azure;
using Azure.Data.Tables;

namespace AzureCrud.Models
{
    public class MyDataEntity:ITableEntity
    {
 public string Id { get; set; }
public string FileExtension { get; set; }
public string FileName { get; set; }
public DateTime FileCreated { get; set; }
public string PartitionKey { get; set; }
public string RowKey { get; set; }
public DateTimeOffset? Timestamp { get; set; }
public ETag ETag { get; set; }
    }
}
