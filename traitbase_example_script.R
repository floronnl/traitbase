### ### ### ### ### ### ### ### ### ### ### 
### EXAMPLE R-SCRIPT FOR USING TRAITBASE ###
### ### ### ### ### ### ### ### ### ### ###

## Last updated on 1 OCT 2025
## By Nick van Doormaal (FLORON)

# Setup----
library(httr2)

# Define the API endpoint URL----
api_url <- request("https://www.traitbase.nl/api")

# Open request (no API-key needed) ----
## Get taxa list of reptiles with red list status 'KW'----
### Build the request----
request_open <- api_url |>
  req_url_path_append("taxa") |> 
  req_url_query(taxonGroup = "R", threatStatus = "KW") |>   # Define query
  req_perform()

### Get the response----
response_taxa <- resp_body_string(request_open)
taxa_df <- read.csv2(text = response_taxa)

## Get all habitat codes that are part of the habitat classification 'beheertype'----
### Build the request----
request_open <- api_url |>
  req_url_path_append("habitatClassesTaxa", "habitatCodes") |> 
  req_url_query(habitat_classification = "beheertype") |>   # Define query
  req_perform()

### Get the response----
response_habitatcodes <- resp_body_string(request_open)
habitatcodes_df <- read.csv2(text = response_habitatcodes)

## Traits table for plants, mosses etc----
request_open <- api_url |>
  req_url_path_append("traits", "traitsPivot") |> 
  req_perform()

### Get the response----
response_pivot <- resp_body_string(request_open) |> 
  readr::read_csv2()

### ### ###

# Request with API key----
my_api_key <- " YOUR API KEY "

## Habitat classes taxa----
### Build request and add API-key----
request_api <- api_url |> 
  req_headers(`X-API-KEY` = my_api_key) |> 
  req_url_path_append("habitatClassesTaxa") |> 
  req_url_query(taxon_class = "Vaatplanten", taxon_category = "snlsoort") |> 
  req_perform()

### Get the response----
response_habitat_taxa <-
  resp_body_string(request_api) |> 
  readr::read_csv2()

## Get traits of one specific species----
### Build request and add API-key----
request_api <- api_url |>
  req_headers(`X-API-KEY` = my_api_key) |>
  req_url_path_append("traits") |> 
  req_url_query(species_id = 26) |> 
  req_perform()

### Get response----
response_traits <- resp_body_string(request_api) |> 
  readr::read_csv2()

### ### ###

# Calculate Ellenberg-values for certain species----
## Example ----
### List all taxon ids
taxon_ids <- c(26, 35, 498, 991)

### Create custom function to obtain data----
get_taxon_traits <- function(taxon_id) {
  api_url |> 
    req_url_path_append("traits", "traitsSingleTaxon") |> 
    req_url_query(taxonId = taxon_id) |> 
    req_perform() |> 
    resp_body_string() |> 
    readr::read_csv2() |> 
    dplyr::select(taxon_id, contains("ellenberg"))
}
### Loop function over species-vector----
taxon_traits <-
  purrr::map(
    taxon_ids, get_taxon_traits
  ) |> 
  dplyr::bind_rows() |> 
  type.convert()

### Calculate Ellenberg-value----
taxon_traits |> 
  dplyr::summarise(
    mean_value = mean(as.numeric(ellenberg_licht), na.rm = TRUE)
  )

