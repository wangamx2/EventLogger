terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
    mongodbatlas = {
      source  = "mongodb/mongodbatlas"
      version = "~> 1.14"
    }
  }

  required_version = ">= 1.3"
}

provider "azurerm" {
  features {}
}

provider "mongodbatlas" {
  public_key  = var.mongodb_atlas_public_key
  private_key = var.mongodb_atlas_private_key
}

resource "azurerm_mssql_server" "sql" {
  name                         = "sql-eventlog-server"
  resource_group_name          = azurerm_resource_group.main.name
  location                     = azurerm_resource_group.main.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_user
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_mssql_database" "db" {
  name                = "eventlogdb"
  server_id           = azurerm_mssql_server.sql.id
  sku_name            = "Basic"
  collation           = "SQL_Latin1_General_CP1_CI_AS"
}

resource "mongodbatlas_project" "eventlog" {
  name   = "eventlog-project"
  org_id = var.mongodb_atlas_org_id
}

resource "mongodbatlas_cluster" "cluster" {
  project_id   = mongodbatlas_project.eventlog.id
  name         = "eventlog-cluster"
  cluster_type = "REPLICASET"
  provider_name = "AWS"
  backing_provider_name = "AWS"
  provider_region_name  = "US_EAST_1"
  provider_instance_size_name = "M0" # Free tier
}
