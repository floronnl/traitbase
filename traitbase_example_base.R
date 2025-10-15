### ### ### ### ### ### ### ### ### ### ### 
### EXAMPLE R-SCRIPT FOR QUERING TRAITBASE ###
###               USING BASE R             ###
### ### ### ### ### ### ### ### ### ### ###

## Last updated on 14 OCT 2025
## By Nick van Doormaal (FLORON)

# Define query parameters
my_habitat_class <- "beheertype"

# Define the endpoint with query parameters
url <- paste0(
  "https://www.traitbase.nl/api/habitatClassesTaxa/habitatCodes?habitatClassification=", my_habitat_class
)

# Read the CSV response directly from the API
habitatcodes_df <- read.csv2(url)

# Show first rows
head(habitatcodes_df)


# To access endpoints that require an API-key, the approach is a bit more complicated.
# Here is a method using the 'httr'-package (although it's generally recommended to use the httr2-package instead)
require(httr)

# Define query parameters
my_api_key <- "my-api-key"
query <- list(taxonGroup = "Vaatplanten", taxonCategory = "snlsoort")

# Define the endpoint with query parameters
url <- "https://www.traitbase.nl/api/habitatClassesTaxa"
response <- GET(url, add_headers(`X-API-KEY` = my_api_key), query = query)

# Read the CSV response directly from the API
habitat_taxa <- read.csv2(text = content(response, "text", encoding = "UTF-8"))

# Show first rows
head(habitat_taxa)

# Practical example
# Visualize number of species per threat status and taxa group

# Define the endpoint with query parameters
url <- paste0(
  "https://www.traitbase.nl/api/taxa"
)

# Read the CSV response directly from the API
taxa_df <- read.csv2(url)

# Filter for only accepted names and taxon with a valid threat status
taxa_clean <- subset(taxa_df, isAcceptedName == -1 & nchar(threatStatus) > 0)

## Make contingency table of taxon group and threat status
xtabs(~ threatStatus + taxaGroup, data = taxa_clean)

# Visualize with ggplot2
library(ggplot2)

ggplot(data = taxa_clean, aes(x = threatStatus)) +
  geom_bar(stat = "count") +
  facet_wrap(~taxaGroup) +
  theme_bw() +
  theme(axis.text.x = element_text(angle = 45, hjust = 1))

  