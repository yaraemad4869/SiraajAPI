-- Update packages with USD prices
UPDATE Packages SET PriceUsd = 96, OriginalPriceUsd = NULL, DefaultCurrency = 'SAR' 
WHERE Price = 360 AND PriceUsd IS NULL;

UPDATE Packages SET PriceUsd = 120, OriginalPriceUsd = 144, DefaultCurrency = 'SAR' 
WHERE Price = 450 AND PriceUsd IS NULL;

UPDATE Packages SET PriceUsd = 200, OriginalPriceUsd = 240, DefaultCurrency = 'SAR' 
WHERE Price = 750 AND PriceUsd IS NULL;

-- Set DefaultCurrency for any package that has it as NULL
UPDATE Packages SET DefaultCurrency = 'SAR' WHERE DefaultCurrency IS NULL;

-- Show results
SELECT NameAr, Price, PriceUsd, OriginalPrice, OriginalPriceUsd, DefaultCurrency 
FROM Packages WHERE IsActive = 1;
