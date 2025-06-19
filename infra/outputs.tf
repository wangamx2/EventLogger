output "sql_server_fqdn" {
  value = azurerm_mssql_server.sql.fully_qualified_domain_name
}

output "mongodb_cluster_uri" {
  value = mongodbatlas_cluster.cluster.connection_strings[0].standard_srv
}
