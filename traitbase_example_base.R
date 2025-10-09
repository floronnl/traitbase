### ### ### ### ### ### ### ### ### ### ### 
### EXAMPLE R-SCRIPT FOR QUERING TRAITBASE ###
###               USING BASE R             ###
### ### ### ### ### ### ### ### ### ### ###

## Last updated on 9 OCT 2025
## By Nick van Doormaal (FLORON)

# Define query parameters
my_taxon_group <- "R"
my_threat_status <- "KW"

# Define the endpoint with query parameters
url <- paste0(
  "https://www.traitbase.nl/api/taxa?taxonGroup=", my_taxon_group,
  "&threatStatus=", my_threat_status
)

# Read the CSV response directly from the API
taxa_df <- read.csv2(url)

# Show first rows
head(taxa_df)


# To access endpoints that require an API-key, the approach is a bit more complicated.
# Here is a method using the 'httr'-package (although it's generally recommended to use the httr2-package instead)
require(httr)

# Define query parameters
my_api_key <- "5cf12c8e-7c60-4750-a0b4-a64ca1521cc4"
query <- list(taxonGroup = "Vaatplanten", taxonCategory = "snlsoort")

# Define the endpoint with query parameters
url <- "https://www.traitbase.nl/api/habitatClassesTaxa"
response <- GET(url, add_headers(`X-API-KEY` = my_api_key), query = query)

# Read the CSV response directly from the API
habitat_taxa <- read.csv2(text = content(response, "text", encoding = "UTF-8"))

# Show first rows
head(habitat_taxa)

# Practical example
# Visualise number of species per threat status and taxa group
