INSERT INTO "BoldInsights" (id,title,assignee,"Amount After Discount","Commission","Payment Frequency")
SELECT id,title,assignee,"Amount After Discount","Commission","Payment Frequency" FROM "Temp_BoldInsights"
ON CONFLICT (id)
DO UPDATE SET
    title=EXCLUDED.title,
    assignee=EXCLUDED.assignee,
    "Amount After Discount"=EXCLUDED."Amount After Discount",
    "Commission"=EXCLUDED."Commission",
    "Payment Frequency"=EXCLUDED."Payment Frequency"