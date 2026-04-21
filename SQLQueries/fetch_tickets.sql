SELECT 
a.id,
title,
b.name as assignee,
case
	when (a.custom_fields->>'1440')::float is null 
		then (a.custom_fields->>'1001')::float 
	else ((a.custom_fields->>'1001')::float - (((a.custom_fields->>'1001')::float * (a.custom_fields->>'1440')::float)/100)) 
end as "Amount After Discount",
a.custom_fields->>'1240'::float as "Commission"
FROM public.ticket_detail a
join
users b on b.id = a.assigned_to_user_id
join
ticket_status c on a.ticket_status_id = c.id
where
b.name in ('Tonny Odhiambo Ojwang')
and
is_visible_in_customer_portal is true
and
is_spam is false
and
(
(a.created_on >= '2026-02-01' and a.created_on <= '2026-02-28')
)
order by
a.id