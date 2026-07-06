DO $$
BEGIN
    CREATE TEMP TABLE "Temp_BoldInsights" (
        id bigint,
		title text,
		created_on timestamp with time zone,
        assignee text,
        "Amount After Discount" numeric(8,2),
		"Commission" numeric(8,2),
        "Payment Frequency" text
    ) ON COMMIT DROP;

    INSERT INTO "Temp_BoldInsights" (id, title, created_on, assignee, "Amount After Discount", "Commission", "Payment Frequency")
    SELECT
        (j->>'id')::bigint,
        j->>'title',
		(j->>'created_on')::timestamptz,
        j->>'assignee',
        (j->>'"Amount After Discount"')::numeric(8,2),
        (j->>'"Commission"')::numeric(8,2),
        j->>'"Payment Frequency"'
    FROM jsonbarrayelements(@payload::jsonb) AS j;

    INSERT INTO "BoldInsights" AS b (id, title, created_on, assignee, "Amount After Discount", "Commission", "Payment Frequency")
    SELECT id, title, created_on, assignee, "Amount After Discount", "Commission", "Payment Frequency"
    FROM "Temp_BoldInsights"
    ON CONFLICT (id) DO UPDATE
      SET title = EXCLUDED.title,
          created_on = EXCLUDED.created_on,
          assignee = EXCLUDED.assignee,
          "Amount After Discount" = EXCLUDED."Amount After Discount",
          "Commission" = EXCLUDED."Commission",
          "Payment Frequency" = EXCLUDED."Payment Frequency";
END $$