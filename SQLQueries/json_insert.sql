INSERT INTO "Json_BoldInsights" (data)
SELECT jsonb_array_elements(CAST(@data AS jsonb))